using UnityEngine;

public class MapSelectionManager : MonoBehaviour
{
    #region Singleton
    public static MapSelectionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ qua các scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [Header("Map Selection Settings")]
    [Tooltip("Tỷ lệ % chọn Generated Map (0-100)\n" +
             "0 = 100% Custom Map\n" +
             "50 = 50/50 Random\n" +
             "100 = 100% Generated Map")]
    [Range(0, 100)]
    [SerializeField] private int generatedMapChance = 50;
    
    [Tooltip("Cho phép player chọn loại map thủ công (qua UI button)")]
    [SerializeField] private bool allowManualSelection = false;

    [Header("Scene Names")]
    [Tooltip("Scene load map tự làm")]
    [SerializeField] private string customMapScene = "LoadMap";
    
    [Tooltip("Scene load map generate")]
    [SerializeField] private string generatedMapScene = "LoadMap_RenderMap";

    [Header("Debug Info (Read Only)")]
    [Tooltip("Loại map hiện tại đã chọn")]
    [SerializeField] private MapType selectedMapType = MapType.None;
    
    [Tooltip("Đã chọn map chưa")]
    [SerializeField] private bool hasSelectedMap = false;

    public enum MapType
    {
        None,
        Custom,      // Map tự làm
        Generated    // Map generate
    }

    #region Public API

    /// <summary>
    /// Lấy loại map đã được chọn
    /// </summary>
    public MapType GetSelectedMapType()
    {
        if (!hasSelectedMap)
        {
            SelectRandomMap();
        }
        return selectedMapType;
    }

    /// <summary>
    /// Lấy tên scene loading tương ứng với loại map đã chọn
    /// </summary>
    public string GetLoadingSceneName()
    {
        if (!hasSelectedMap)
        {
            SelectRandomMap();
        }

        return selectedMapType == MapType.Custom ? customMapScene : generatedMapScene;
    }

    /// <summary>
    /// Random chọn loại map dựa trên tỷ lệ
    /// </summary>
    public void SelectRandomMap()
    {
        int randomValue = Random.Range(0, 100);
        
        if (randomValue < generatedMapChance)
        {
            selectedMapType = MapType.Generated;
            Debug.Log($"[MapSelection] 🎲 Random selected: GENERATED MAP ({generatedMapChance}% chance, rolled {randomValue})");
        }
        else
        {
            selectedMapType = MapType.Custom;
            Debug.Log($"[MapSelection] 🎲 Random selected: CUSTOM MAP ({100 - generatedMapChance}% chance, rolled {randomValue})");
        }

        hasSelectedMap = true;
    }

    /// <summary>
    /// Chọn loại map thủ công (dành cho UI button)
    /// </summary>
    public void SelectMapManually(MapType mapType)
    {
        if (!allowManualSelection)
        {
            Debug.LogWarning("[MapSelection] Manual selection is disabled! Enable 'Allow Manual Selection' in Inspector.");
            return;
        }

        selectedMapType = mapType;
        hasSelectedMap = true;
        
        Debug.Log($"[MapSelection] 👆 Manually selected: {mapType}");
    }

    /// <summary>
    /// Reset selection (dùng khi quay lại main menu)
    /// </summary>
    public void ResetSelection()
    {
        selectedMapType = MapType.None;
        hasSelectedMap = false;
        Debug.Log("[MapSelection] Selection reset");
    }

    /// <summary>
    /// Lấy tỷ lệ % map generate
    /// </summary>
    public int GetGeneratedMapChance()
    {
        return generatedMapChance;
    }

    /// <summary>
    /// Đặt tỷ lệ % map generate (0-100)
    /// </summary>
    public void SetGeneratedMapChance(int chance)
    {
        generatedMapChance = Mathf.Clamp(chance, 0, 100);
        Debug.Log($"[MapSelection] Generated map chance set to {generatedMapChance}%");
    }

    /// <summary>
    /// Kiểm tra xem đã chọn map chưa
    /// </summary>
    public bool HasSelectedMap()
    {
        return hasSelectedMap;
    }

    #endregion

    #region Lifecycle Events

    void OnEnable()
    {
        Debug.Log($"[MapSelection] Manager initialized | Generated chance: {generatedMapChance}%");
    }

    void OnDestroy()
    {
        // Cleanup khi destroy
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion

    #region Debug Tools (Context Menu Only)

    [ContextMenu("Debug - Random Select Now")]
    void DebugRandomSelect()
    {
        SelectRandomMap();
        Debug.Log($"[MapSelection] Will load scene: {GetLoadingSceneName()}");
    }

    [ContextMenu("Debug - Force Select Custom Map")]
    void DebugSelectCustom()
    {
        bool originalSetting = allowManualSelection;
        allowManualSelection = true;
        SelectMapManually(MapType.Custom);
        allowManualSelection = originalSetting;
    }

    [ContextMenu("Debug - Force Select Generated Map")]
    void DebugSelectGenerated()
    {
        bool originalSetting = allowManualSelection;
        allowManualSelection = true;
        SelectMapManually(MapType.Generated);
        allowManualSelection = originalSetting;
    }

    [ContextMenu("Debug - Reset Selection")]
    void DebugResetSelection()
    {
        ResetSelection();
    }

    [ContextMenu("Debug - Show Current Status")]
    void DebugShowStatus()
    {
        Debug.Log("=== MAP SELECTION STATUS ===");
        Debug.Log($"Selected Type: {selectedMapType}");
        Debug.Log($"Has Selected: {hasSelectedMap}");
        Debug.Log($"Generated Chance: {generatedMapChance}%");
        Debug.Log($"Custom Chance: {100 - generatedMapChance}%");
        Debug.Log($"Scene To Load: {(hasSelectedMap ? GetLoadingSceneName() : "Not yet selected")}");
        Debug.Log($"Manual Selection: {(allowManualSelection ? "Enabled" : "Disabled")}");
    }

    #endregion
}