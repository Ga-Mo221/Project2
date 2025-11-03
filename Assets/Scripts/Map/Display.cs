using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour, IDetectable
{
    [Header("Model Settings")]
    [SerializeField] private ModelType modelType;

    [Header("Visual Elements")]
    [SerializeField] private GameObject miniMapIcon;
    [SerializeField] private GameObject outLine;
    [SerializeField] private GameObject _light;

    [Header("Script References")]
    [SerializeField] private Item item;
    [SerializeField] private AnimalAI animal;
    [SerializeField] private EnemyAI enemy;
    [SerializeField] private EnemyHouseHealth house;
    [SerializeField] private Rada rada;

    // State
    private bool isDead = false;
    private bool isCurrentlyVisible = false;
    private HashSet<Rada> seers = new HashSet<Rada>();

    // Cached position
    private Vector2 lastPosition;
    
    // Registration flag
    private bool isRegistered = false;

    // IDetectable implementation
    public Vector2 Position => transform.position;
    public bool IsValid => !isDead && gameObject.activeInHierarchy;

    public bool HasSeemer(Rada seemer) => seers.Contains(seemer);

    void Start()
    {
        RegisterWithManager();
        lastPosition = transform.position;
        HideAllVisuals();
    }

    void OnEnable()
    {
        // Đợi 1 frame để đảm bảo DetectionManager đã sẵn sàng
        if (Application.isPlaying && !isRegistered)
        {
            Invoke(nameof(RegisterWithManager), 0.1f);
        }
        
        lastPosition = transform.position;
        HideAllVisuals();
    }

    void OnDisable()
    {
        UnregisterFromManager();
        HideAllVisuals();
    }

    void OnDestroy()
    {
        UnregisterFromManager();
    }

    private void RegisterWithManager()
    {
        if (isRegistered) return;
        
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.RegisterDetectable(this);
            isRegistered = true;
            //Debug.Log($"[Display] {gameObject.name} ({modelType}) registered successfully");
        }
        else
        {
            //Debug.LogWarning($"[Display] {gameObject.name} - DetectionManager not found! Retrying...");
            // Thử lại sau 0.5s
            Invoke(nameof(RegisterWithManager), 0.5f);
        }
    }

    private void UnregisterFromManager()
    {
        if (!isRegistered) return;
        
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.UnregisterDetectable(this);
            isRegistered = false;
            //Debug.Log($"[Display] {gameObject.name} unregistered");
        }
    }

    public void AddSeemer(Rada seemer)
    {
        if (seers.Add(seemer))
        {
            //Debug.Log($"[Display] {gameObject.name} - Radar added. Total seers: {seers.Count}");
            UpdateVisibility();
        }
    }

    public void RemoveSeemer(Rada seemer)
    {
        if (seers.Remove(seemer))
        {
            //Debug.Log($"[Display] {gameObject.name} - Radar removed. Total seers: {seers.Count}");
            UpdateVisibility();
        }
    }

    void Update()
    {
        // Đảm bảo đã đăng ký
        if (!isRegistered && DetectionManager.Instance != null)
        {
            RegisterWithManager();
        }

        // Update position trong grid nếu di chuyển
        Vector2 currentPos = transform.position;
        if (Vector2.Distance(currentPos, lastPosition) > 0.5f)
        {
            if (DetectionManager.Instance != null && isRegistered)
            {
                DetectionManager.Instance.UpdateDetectablePosition(this, lastPosition, currentPos);
            }
            lastPosition = currentPos;
        }

        // Check death state
        CheckDeathState();
    }

    // Hàm kiểm tra có được phát hiện không (ít nhất 1 radar)
    public bool checkDetec()
    {
        return seers.Count > 0 && !isDead;
    }

    private void UpdateVisibility()
    {
        bool shouldBeVisible = seers.Count > 0 && !isDead;

        if (shouldBeVisible != isCurrentlyVisible)
        {
            isCurrentlyVisible = shouldBeVisible;
            if (shouldBeVisible)
            {
                //Debug.Log($"[Display] {gameObject.name} - Showing visuals");
                ShowVisuals();
            }
            else
            {
                //Debug.Log($"[Display] {gameObject.name} - Hiding visuals");
                HideVisuals();
            }
        }
    }

    private void CheckDeathState()
    {
        bool wasDead = isDead;

        switch (modelType)
        {
            case ModelType.Animal:
                isDead = animal != null && animal.getDie();
                break;
            case ModelType.Enemy:
                isDead = enemy != null && enemy.getDie();
                break;
            case ModelType.EnemyHouse:
                isDead = house != null && house._Die;
                break;
            case ModelType.Tree:
            case ModelType.Rock:
                isDead = item != null && item.getDie();
                break;
        }

        if (isDead && !wasDead)
        {
            //Debug.Log($"[Display] {gameObject.name} died - hiding all visuals");
            HideAllVisuals();
        }
    }

    private void ShowVisuals()
    {
        switch (modelType)
        {
            case ModelType.Tree:
            case ModelType.Rock:
                if (outLine != null && !isDead) outLine.SetActive(true);
                if (item != null) item._detec = true;
                break;

            case ModelType.Gold:
                if (_light != null) _light.SetActive(true);
                if (outLine != null) outLine.SetActive(true);
                if (item != null) item._detec = true;
                break;

            case ModelType.Animal:
            case ModelType.Enemy:
            case ModelType.EnemyHouse:
                if (miniMapIcon != null) miniMapIcon.SetActive(true);
                if (outLine != null) outLine.SetActive(true);
                break;

            case ModelType.Deco:
                if (_light != null) _light.SetActive(true);
                break;
        }
    }

    private void HideVisuals()
    {
        if (miniMapIcon != null) miniMapIcon.SetActive(false);
        if (outLine != null) outLine.SetActive(false);
        if (_light != null) _light.SetActive(false);

        if (item != null) item._detec = false;
    }

    private void HideAllVisuals()
    {
        HideVisuals();
        seers.Clear();
        isCurrentlyVisible = false;
    }
}

public enum ModelType
{
    Tree,
    Rock,
    Gold,
    Animal,
    Enemy,
    EnemyHouse,
    Deco
}
