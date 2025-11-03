using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Quản lý việc chọn buff cho Castle
/// Tối ưu: Cache, Dictionary, Event-driven, Single Responsibility
/// </summary>
public class ChosenBuff : MonoBehaviour
{
    #region Serialized Fields
    
    [Header("Buff Circle Preview")]
    [SerializeField] private GameObject buffCircleEffect;
    [Tooltip("Offset để BuffCircle hiển thị đẹp hơn (nếu cần)")]
    [SerializeField] private Vector3 circleOffset = Vector3.zero;

    [Header("Sprites")]
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteSelected;

    [Header("Buff UI Elements")]
    [SerializeField] private BuffUIElement healingBuff;
    [SerializeField] private BuffUIElement farmBuff;
    [SerializeField] private BuffUIElement furyBuff;

    [Header("Info Panel")]
    [SerializeField] private GameObject buffInfoPanel;
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private TextMeshProUGUI buffContentText;
    [SerializeField] private TextMeshProUGUI price;
    
    [Header("Colors")]
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private Color farmColor = Color.yellow;
    [SerializeField] private Color furyColor = Color.red;

    [Header("Settings")]
    [SerializeField] private float tooltipDelaySeconds = 1f;
    [SerializeField] private LayerMask placementLayerMask = -1;

    [Header("Cooldown Settings")]
    [Tooltip("Thời gian cooldown cho Healing Buff (giây)")]
    [SerializeField] private float healingCooldown = 30f;
    [Tooltip("Thời gian cooldown cho Farm Buff (giây)")]
    [SerializeField] private float farmCooldown = 45f;
    [Tooltip("Thời gian cooldown cho Fury Buff (giây)")]
    [SerializeField] private float furyCooldown = 60f;
    [Tooltip("Màu overlay khi buff đang cooldown")]
    [SerializeField] private Color cooldownOverlayColor = new Color(0f, 0f, 0f, 0.6f);

    #endregion

    #region Private Fields

    private Dictionary<int, BuffUIElement> buffElements;
    private Dictionary<int, BuffConfig> buffConfigs;
    private Dictionary<int, GameObject> activeBuffInstances;
    private Dictionary<int, BuffCooldownData> buffCooldowns; // NEW: Cooldown tracking
    
    private int currentBuffID = -1;
    private int lastHoveredID = -1;
    private Coroutine tooltipCoroutine;

    // Cache references
    private Castle castle;
    private CursorManager cursorManager;
    private LocalizationManager localizationManager;
    private GameManager gameManager;
    private Camera mainCamera;

    // Input optimization
    private readonly KeyCode[] cancelKeys = new KeyCode[]
    {
        KeyCode.Q, KeyCode.W, KeyCode.E,
        KeyCode.A, KeyCode.S,
        KeyCode.Tab, KeyCode.V, KeyCode.U, KeyCode.C
    };

    #endregion

    #region Unity Lifecycle

    void Start()
    {
        InitializeReferences();
        InitializeBuffData();
        InitializeCooldowns(); // NEW
        SetBuff(0);
        
        activeBuffInstances = new Dictionary<int, GameObject>();
        
        if (buffCircleEffect != null)
            buffCircleEffect.SetActive(false);
    }

    void Update()
    {
        if (!ValidateReferences()) return;

        UpdateCooldowns(); // NEW: Update cooldown timers
        HandleInput();
        HandleTooltip();
        HandleBuffPlacement();
    }

    void OnDestroy()
    {
        if (tooltipCoroutine != null)
            StopCoroutine(tooltipCoroutine);
            
        // Cleanup spawned buffs
        foreach (var kvp in activeBuffInstances)
        {
            if (kvp.Value != null)
                Destroy(kvp.Value);
        }
        activeBuffInstances.Clear();
    }

    #endregion

    #region Initialization

    void InitializeReferences()
    {
        castle = Castle.Instance;
        cursorManager = CursorManager.Instance;
        localizationManager = LocalizationManager.Instance;
        gameManager = GameManager.Instance;
        mainCamera = CameraInfo.Instance?.cameraMain;
        
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void InitializeBuffData()
    {
        buffElements = new Dictionary<int, BuffUIElement>
        {
            { 1, healingBuff },
            { 2, farmBuff },
            { 3, furyBuff }
        };

        buffConfigs = new Dictionary<int, BuffConfig>
        {
            { 1, new BuffConfig
            {
                buffID = 1,
                nameKey = "UI.Buff.Healing.name",
                contentKey = "UI.Buff.Healing.content",
                color = healColor,
                cooldownTime = healingCooldown, // NEW
                getPrefab = () => gameManager?._healing_Buff_Brefab
            }},
            { 2, new BuffConfig
            {
                buffID = 2,
                nameKey = "UI.Buff.Farm.name",
                contentKey = "UI.Buff.Farm.content",
                color = farmColor,
                cooldownTime = farmCooldown, // NEW
                getPrefab = () => gameManager?._farm_Buff_Brefab
            }},
            { 3, new BuffConfig
            {
                buffID = 3,
                nameKey = "UI.Buff.Fury.name",
                contentKey = "UI.Buff.Fury.content",
                color = furyColor,
                cooldownTime = furyCooldown, // NEW
                getPrefab = () => gameManager?._fury_Buff_Brefab
            }}
        };
    }

    void InitializeCooldowns()
    {
        buffCooldowns = new Dictionary<int, BuffCooldownData>();

        foreach (var kvp in buffConfigs)
        {
            int buffID = kvp.Key;
            BuffConfig config = kvp.Value;

            buffCooldowns[buffID] = new BuffCooldownData
            {
                buffID = buffID,
                maxCooldown = config.cooldownTime,
                currentCooldown = 0f,
                isOnCooldown = false
            };
        }

        // Setup cooldown overlays
        SetupCooldownVisuals();
    }

    void SetupCooldownVisuals()
    {
        foreach (var kvp in buffElements)
        {
            BuffUIElement element = kvp.Value;
            
            if (element.cooldownOverlay != null)
            {
                element.cooldownOverlay.color = cooldownOverlayColor;
                element.cooldownOverlay.fillAmount = 0f;
                element.cooldownOverlay.gameObject.SetActive(true);
            }

            if (element.cooldownText != null)
            {
                element.cooldownText.gameObject.SetActive(false);
            }
        }
    }

    bool ValidateReferences()
    {
        if (castle == null) castle = Castle.Instance;
        if (cursorManager == null) cursorManager = CursorManager.Instance;
        if (mainCamera == null) mainCamera = Camera.main;
        
        return castle != null && cursorManager != null && mainCamera != null;
    }

    #endregion

    #region Cooldown System

    void UpdateCooldowns()
    {
        foreach (var kvp in buffCooldowns)
        {
            int buffID = kvp.Key;
            BuffCooldownData cooldownData = kvp.Value;

            if (!cooldownData.isOnCooldown) continue;

            // Decrease cooldown timer
            cooldownData.currentCooldown -= Time.deltaTime;

            if (cooldownData.currentCooldown <= 0f)
            {
                // Cooldown finished
                cooldownData.currentCooldown = 0f;
                cooldownData.isOnCooldown = false;
                OnCooldownComplete(buffID);
            }

            // Update visual
            UpdateCooldownVisual(buffID, cooldownData);
        }
    }

    void UpdateCooldownVisual(int buffID, BuffCooldownData cooldownData)
    {
        if (!buffElements.ContainsKey(buffID)) return;

        BuffUIElement element = buffElements[buffID];

        if (cooldownData.isOnCooldown)
        {
            // Calculate fill amount (1 = full cooldown, 0 = ready)
            float fillAmount = cooldownData.currentCooldown / cooldownData.maxCooldown;
            
            // Update overlay
            if (element.cooldownOverlay != null)
            {
                element.cooldownOverlay.fillAmount = fillAmount;
            }

            // Update text
            if (element.cooldownText != null)
            {
                element.cooldownText.gameObject.SetActive(true);
                int secondsRemaining = Mathf.CeilToInt(cooldownData.currentCooldown);
                element.cooldownText.text = secondsRemaining.ToString();
            }
        }
        else
        {
            // Cooldown complete
            if (element.cooldownOverlay != null)
            {
                element.cooldownOverlay.fillAmount = 0f;
            }

            if (element.cooldownText != null)
            {
                element.cooldownText.gameObject.SetActive(false);
            }
        }
    }

    void StartCooldown(int buffID)
    {
        if (!buffCooldowns.ContainsKey(buffID)) return;

        BuffCooldownData cooldownData = buffCooldowns[buffID];
        cooldownData.currentCooldown = cooldownData.maxCooldown;
        cooldownData.isOnCooldown = true;

        // Immediate visual update
        UpdateCooldownVisual(buffID, cooldownData);
    }

    void OnCooldownComplete(int buffID)
    {
        // Optional: Play sound, show notification, etc.
        if (buffElements.ContainsKey(buffID))
        {
            // Could add a "ready" animation here
        }
    }

    bool IsBuffOnCooldown(int buffID)
    {
        if (!buffCooldowns.ContainsKey(buffID)) return false;
        return buffCooldowns[buffID].isOnCooldown;
    }

    float GetCooldownRemaining(int buffID)
    {
        if (!buffCooldowns.ContainsKey(buffID)) return 0f;
        return buffCooldowns[buffID].currentCooldown;
    }

    #endregion

    #region Buff Placement System

    void HandleBuffPlacement()
    {
        bool isAnyBuffSelected = castle._buff_1 || castle._buff_2 || castle._buff_3;

        if (!isAnyBuffSelected)
        {
            // No buff selected - hide circle
            if (buffCircleEffect != null && buffCircleEffect.activeSelf)
                buffCircleEffect.SetActive(false);
            return;
        }

        // Get world position from mouse
        Vector3 worldPos = GetMouseWorldPosition();
        
        if (worldPos == Vector3.zero)
        {
            // Invalid position - hide circle
            if (buffCircleEffect != null && buffCircleEffect.activeSelf)
                buffCircleEffect.SetActive(false);
            return;
        }

        // Show and update circle position
        if (buffCircleEffect != null)
        {
            buffCircleEffect.SetActive(true);
            buffCircleEffect.transform.position = worldPos + circleOffset;
        }

        // Handle placement on click
        if (Input.GetMouseButtonDown(0) && !cursorManager.ChoseUI)
        {
            PlaceBuffAtPosition(worldPos);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) return Vector3.zero;

        // Option 1: Raycast to ground
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementLayerMask))
        {
            return hit.point;
        }

        // Option 2: Use fixed Z plane (for 2D games)
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    void PlaceBuffAtPosition(Vector3 worldPosition)
    {
        if (gameManager == null) return;

        int selectedBuffID = GetSelectedBuffID();
        if (selectedBuffID <= 0) return;

        // Check cooldown before placing
        if (IsBuffOnCooldown(selectedBuffID))
        {
            // Optional: Show "buff is on cooldown" message
            Debug.Log($"[ChosenBuff] Buff {selectedBuffID} is on cooldown! {GetCooldownRemaining(selectedBuffID):F1}s remaining");
            return;
        }

        // Get or create buff instance
        GameObject buffInstance = GetOrCreateBuffInstance(selectedBuffID);
        
        if (buffInstance != null)
        {
            buffInstance.transform.position = worldPosition;
            
            if (!buffInstance.activeSelf)
            {
                buffInstance.SetActive(true);
                switch (selectedBuffID)
                {
                    case 1: castle._meat -= healingBuff.meat; break;
                    case 2: castle._meat -= farmBuff.meat; break;
                    case 3: castle._meat -= furyBuff.meat; break;
                }
                gameManager.UIupdateReferences();
            }
            
            SpellBuff spellBuff = buffInstance.GetComponent<SpellBuff>();
            if (spellBuff != null)
            {
                spellBuff.SendMessage("OnPlaced", SendMessageOptions.DontRequireReceiver);
            }

            // Start cooldown after successful placement
            StartCooldown(selectedBuffID);
        }

        // Deselect buff after placement
        SetBuff(0);
    }

    GameObject GetOrCreateBuffInstance(int buffID)
    {
        // Check if instance already exists
        if (activeBuffInstances.ContainsKey(buffID) && activeBuffInstances[buffID] != null)
        {
            return activeBuffInstances[buffID];
        }

        // Create new instance
        GameObject prefab = GetBuffPrefab(buffID);
        if (prefab == null)
        {
            Debug.LogWarning($"[ChosenBuff] Buff prefab not found for ID {buffID}");
            return null;
        }

        GameObject instance = Instantiate(prefab);
        instance.name = $"Buff_{buffID}_{prefab.name}";
        
        // Store reference
        activeBuffInstances[buffID] = instance;
        
        return instance;
    }

    GameObject GetBuffPrefab(int buffID)
    {
        if (!buffConfigs.ContainsKey(buffID))
            return null;

        return buffConfigs[buffID].getPrefab?.Invoke();
    }

    int GetSelectedBuffID()
    {
        if (castle._buff_1) return 1;
        if (castle._buff_2) return 2;
        if (castle._buff_3) return 3;
        return 0;
    }

    #endregion

    #region Input Handling

    void HandleInput()
    {
        // Mouse click selection
        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseSelection();
        }

        // Right click or cancel keys = deselect
        if (Input.GetMouseButtonDown(1) || IsAnyCancelKeyPressed())
        {
            SetBuff(0);
        }

        // Number key selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetBuff(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetBuff(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetBuff(3);
    }

    void HandleMouseSelection()
    {
        if (!cursorManager.ChoseUI) 
        {
            // Don't deselect if clicking on game world (for buff placement)
            return;
        }

        int clickedID = cursorManager.ID;

        foreach (var kvp in buffElements)
        {
            if (kvp.Value.choseUI.getID() == clickedID)
            {
                SetBuff(kvp.Key);
                return;
            }
        }

        SetBuff(0);
    }

    bool IsAnyCancelKeyPressed()
    {
        foreach (KeyCode key in cancelKeys)
        {
            if (Input.GetKeyDown(key))
                return true;
        }
        return false;
    }

    #endregion

    #region Tooltip Handling

    void HandleTooltip()
    {
        int hoveredID = GetHoveredBuffID();

        if (hoveredID != lastHoveredID)
        {
            lastHoveredID = hoveredID;

            if (tooltipCoroutine != null)
            {
                StopCoroutine(tooltipCoroutine);
                tooltipCoroutine = null;
            }

            if (hoveredID > 0)
            {
                tooltipCoroutine = StartCoroutine(ShowTooltipDelayed(hoveredID));
            }
            else
            {
                HideTooltip();
            }
        }
    }

    int GetHoveredBuffID()
    {
        if (!cursorManager.ChoseUI) return -1;

        int cursorID = cursorManager.ID;

        foreach (var kvp in buffElements)
        {
            if (kvp.Value.choseUI.getID() == cursorID)
                return kvp.Key;
        }

        return -1;
    }

    IEnumerator ShowTooltipDelayed(int buffID)
    {
        yield return new WaitForSeconds(tooltipDelaySeconds);
        ShowTooltip(buffID);
        tooltipCoroutine = null;
    }

    void ShowTooltip(int buffID)
    {
        if (!buffConfigs.ContainsKey(buffID))
        {
            HideTooltip();
            return;
        }

        BuffConfig config = buffConfigs[buffID];
        GameObject prefab = config.getPrefab?.Invoke();

        if (prefab == null)
        {
            HideTooltip();
            return;
        }

        SpellBuff spellBuff = prefab.GetComponent<SpellBuff>();
        if (spellBuff == null)
        {
            HideTooltip();
            return;
        }

        string name = GetLocalizedText(config.nameKey);
        string contentTemplate = GetLocalizedText(config.contentKey);
        string content = FormatBuffContent(buffID, spellBuff, contentTemplate, config.color);
        string value = "";
        switch (buffID)
        {
            case 1: value = healingBuff.meat.ToString(); break;
            case 2: value = farmBuff.meat.ToString(); break;
            case 3: value = furyBuff.meat.ToString(); break;
        }
        buffNameText.text = name;
        buffNameText.color = config.color;
        buffContentText.text = content;
        price.text = value;
        buffInfoPanel.SetActive(true);
    }

    void HideTooltip()
    {
        buffInfoPanel.SetActive(false);
    }

    string FormatBuffContent(int buffID, SpellBuff spellBuff, string template, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);

        switch (buffID)
        {
            case 1: // Healing
                return string.Format(template,
                    Colorize(spellBuff.getHealValue() + "HP", hexColor),
                    Colorize(spellBuff.getHealInterval().ToString(), hexColor),
                    Colorize(spellBuff.getBuffTime().ToString(), hexColor));

            case 2: // Farm
                return string.Format(template,
                    Colorize("x" + spellBuff.getBuffSpeedAttack(), hexColor),
                    Colorize(spellBuff.getBuffTime().ToString(), hexColor));

            case 3: // Fury
                return string.Format(template,
                    Colorize("x" + spellBuff.getBuffSpeedAttack(), hexColor),
                    Colorize("x" + spellBuff.getBuffDamage(), hexColor),
                    Colorize(spellBuff.getBuffTime().ToString(), hexColor));

            default:
                return template;
        }
    }

    string Colorize(string text, string hexColor)
    {
        return $"<color=#{hexColor}>{text}</color>";
    }

    string GetLocalizedText(string key)
    {
        if (localizationManager == null)
            localizationManager = LocalizationManager.Instance;

        return localizationManager != null 
            ? localizationManager.Get(key) 
            : $"[{key}]";
    }

    #endregion

    #region Buff Selection

    public void SetBuff(int buffID)
    {
        if (castle == null) return;

        bool canUseBuff = true;
        switch (buffID)
        {
            case 1: if (healingBuff.meat > castle._meat) canUseBuff = false; break;
            case 2: if (farmBuff.meat > castle._meat) canUseBuff = false; break;
            case 3: if (furyBuff.meat > castle._meat) canUseBuff = false; break;
        }

        if (!canUseBuff) return;

        // Check if buff is on cooldown
        if (buffID > 0 && IsBuffOnCooldown(buffID))
        {
            // Don't allow selection if on cooldown
            Debug.Log($"[ChosenBuff] Cannot select Buff {buffID} - on cooldown!");
            return;
        }

        castle._buff_1 = false;
        castle._buff_2 = false;
        castle._buff_3 = false;

        switch (buffID)
        {
            case 1: castle._buff_1 = true; break;
            case 2: castle._buff_2 = true; break;
            case 3: castle._buff_3 = true; break;
        }

        currentBuffID = buffID;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        foreach (var kvp in buffElements)
        {
            int id = kvp.Key;
            BuffUIElement element = kvp.Value;

            bool isSelected = (id == currentBuffID);
            element.background.sprite = isSelected ? spriteSelected : spriteNormal;
        }
    }

    #endregion

    #region Public API

    public int GetCurrentBuffID() => currentBuffID;

    public bool IsBuffSelected(int buffID) => currentBuffID == buffID;

    public void DeselectBuff() => SetBuff(0);

    /// <summary>
    /// Remove an active buff instance (call this when buff expires)
    /// </summary>
    public void RemoveBuffInstance(int buffID)
    {
        if (activeBuffInstances.ContainsKey(buffID))
        {
            if (activeBuffInstances[buffID] != null)
                Destroy(activeBuffInstances[buffID]);
            
            activeBuffInstances.Remove(buffID);
        }
    }

    /// <summary>
    /// Get active buff instance for external reference
    /// </summary>
    public GameObject GetActiveBuffInstance(int buffID)
    {
        return activeBuffInstances.ContainsKey(buffID) ? activeBuffInstances[buffID] : null;
    }

    /// <summary>
    /// Check if a buff is currently on cooldown
    /// </summary>
    public bool IsOnCooldown(int buffID) => IsBuffOnCooldown(buffID);

    /// <summary>
    /// Get remaining cooldown time in seconds
    /// </summary>
    public float GetRemainingCooldown(int buffID) => GetCooldownRemaining(buffID);

    /// <summary>
    /// Manually start cooldown (useful for testing or special cases)
    /// </summary>
    public void TriggerCooldown(int buffID) => StartCooldown(buffID);

    /// <summary>
    /// Reset cooldown (admin/debug function)
    /// </summary>
    public void ResetCooldown(int buffID)
    {
        if (buffCooldowns.ContainsKey(buffID))
        {
            buffCooldowns[buffID].currentCooldown = 0f;
            buffCooldowns[buffID].isOnCooldown = false;
            UpdateCooldownVisual(buffID, buffCooldowns[buffID]);
        }
    }

    /// <summary>
    /// Reset all cooldowns (debug/cheat function)
    /// </summary>
    public void ResetAllCooldowns()
    {
        foreach (var buffID in buffCooldowns.Keys)
        {
            ResetCooldown(buffID);
        }
    }

    #endregion
}

#region Helper Classes

[System.Serializable]
public class BuffUIElement
{
    [Header("Price")]
    public int meat;

    [Header("Visual Elements")]
    public Image icon;
    public Image background;
    public ChoseUI choseUI;
    
    [Header("Cooldown Visual")]
    [Tooltip("Image overlay để hiển thị cooldown (fillAmount 1→0)")]
    public Image cooldownOverlay;
    [Tooltip("Text hiển thị số giây còn lại")]
    public TextMeshProUGUI cooldownText;
}

public class BuffConfig
{
    public int buffID;
    public string nameKey;
    public string contentKey;
    public Color color;
    public float cooldownTime; // NEW
    public System.Func<GameObject> getPrefab;
}

/// <summary>
/// Tracks cooldown state for each buff
/// </summary>
public class BuffCooldownData
{
    public int buffID;
    public float maxCooldown;
    public float currentCooldown;
    public bool isOnCooldown;
}

#endregion