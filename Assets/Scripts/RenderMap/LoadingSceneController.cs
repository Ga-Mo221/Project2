using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loading Scene Controller - Quản lý màn hình loading với Additive Scene Loading
/// CÁCH HOẠT ĐỘNG:
/// 1. Loading Scene hiển thị UI
/// 2. Game Scene được load ADDITIVE (cùng lúc với Loading Scene)
/// 3. Generators trong Game Scene chạy và notify về Loading Scene
/// 4. Khi xong → Unload Loading Scene, giữ lại Game Scene
/// </summary>
public class LoadingSceneController : MonoBehaviour
{
    #region Singleton
    public static LoadingSceneController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [SerializeField] private GetMainCam _getMainCam;

    [Header("=== Scene Settings ===")]
    [Tooltip("Tên scene game sẽ load\nMặc định: Game_RenderMap")]
    [SerializeField] private string gameSceneName = "Game_RenderMap";
    
    [Tooltip("Thời gian chờ trước khi bắt đầu load game scene (giây)")]
    [SerializeField] private float delayBeforeLoadGameScene = 0.5f;
    
    [Tooltip("Thời gian hiển thị 'Complete' trước khi unload loading scene (giây)")]
    [SerializeField] private float delayBeforeUnloadLoading = 1.5f;

    [Header("=== Loading UI - Public Variables ===")]
    [Tooltip("Các biến này để UI component đọc")]
    public string statusText = "Initializing...";
    public string detailText = "Starting up...";
    public float currentProgress = 0f;

    [Header("=== Progress Weights ===")]
    [Range(0f, 1f)]
    [Tooltip("Tỷ trọng load scene (10%)")]
    [SerializeField] private float sceneLoadWeight = 0.1f;
    
    [Range(0f, 1f)]
    [Tooltip("Tỷ trọng terrain generation (30%)")]
    [SerializeField] private float terrainWeight = 0.2f;
    
    [Range(0f, 1f)]
    [Tooltip("Tỷ trọng border generation (20%)")]
    [SerializeField] private float borderWeight = 0.2f;

    [Range(0f, 1f)]
    [Tooltip("Tỷ trọng prefab spawning (40%)")]
    [SerializeField] private float spawnWeight = 0.3f;
    
    [Range(0f, 1f)]
    [Tooltip("Tỷ trọng game ready (20%)")]
    [SerializeField] private float readyWeight = 0.2f;

    [Header("=== Debug ===")]
    [SerializeField] private bool showDebugLogs = true;

    // Progress tracking
    private bool isSceneLoaded = false;
    private bool isTerrainComplete = false;
    private bool isBorderComplete = false;
    private bool isSpawnComplete = false;
    private bool isGameReadyComplete = false;
    private bool isUnloadingLoading = false;

    // Scene references
    private Scene loadingScene;
    private Scene gameScene;

    string key = "";
    string txt1 = "";
    string txt2 = "";

    void Start()
    {
        loadingScene = gameObject.scene;
        LogInfo("=== LOADING SYSTEM STARTED ===");
        key = "LadingScene.GenerateMap.startstatus";
        txt1 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        key = "LadingScene.GenerateMap.startdetail";
        txt2 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        UpdateUI(txt1, txt2);
        
        // Bắt đầu load game scene
        StartCoroutine(LoadGameSceneAdditive());
    }

    #region Scene Loading

    IEnumerator LoadGameSceneAdditive()
    {
        // Delay nhỏ để UI kịp render
        yield return new WaitForSeconds(delayBeforeLoadGameScene);

        LogInfo($"Loading game scene: {gameSceneName}");
        key = "LadingScene.GenerateMap.loadscene1status";
        txt1 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        key = "LadingScene.GenerateMap.loadscene1detail";
        txt2 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        UpdateUI(txt1, $"{txt2} {gameSceneName}...");

        // Load scene ADDITIVE (không xóa Loading scene)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        if (asyncLoad == null)
        {
            Debug.LogError($"[LoadingScene] ❌ Failed to load scene: {gameSceneName}");
            Debug.LogError($"[LoadingScene] Make sure scene is in Build Settings!");
            yield break;
        }

        // Theo dõi tiến trình load scene (0% -> 10%)
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            currentProgress = progress * sceneLoadWeight;
            
            key = "LadingScene.GenerateMap.loadscene2status";
            txt1 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.loadscene2detail";
            txt2 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            UpdateUI(txt1, $"{txt2} {progress * 100:F0}%");
            
            yield return null;
        }

        // Scene đã load xong
        gameScene = SceneManager.GetSceneByName(gameSceneName);
        
        if (!gameScene.IsValid())
        {
            Debug.LogError($"[LoadingScene] ❌ Game scene not valid after loading!");
            yield break;
        }

        // Set game scene làm active (camera, lighting, v.v. sẽ dùng scene này)
        SceneManager.SetActiveScene(gameScene);
        _getMainCam.setupCamera();

        isSceneLoaded = true;
        currentProgress = sceneLoadWeight;
        
        LogInfo($"✓ Game scene loaded successfully");
        LogInfo($"Current scenes: Loading={loadingScene.name}, Game={gameScene.name}");

        key = "LadingScene.GenerateMap.startloadscenestatus";
        txt1 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        key = "LadingScene.GenerateMap.startloadscenedetail";
        txt2 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        UpdateUI(txt1, txt2);

        // Từ đây, các script trong Game Scene bắt đầu chạy
        // và sẽ gọi NotifyXXX() methods
    }

    #endregion

    #region Public Notification API

    /// <summary>
    /// Được gọi từ LayeredTerrainGenerator khi terrain xong
    /// </summary>
    public void NotifyTerrainComplete()
    {
        if (isTerrainComplete)
        {
            LogWarning("Terrain already complete!");
            return;
        }

        if (!isSceneLoaded)
        {
            LogWarning("Terrain complete but scene not loaded yet?");
        }

        isTerrainComplete = true;
        UpdateProgress();
        LogInfo("✓ Terrain Generation Complete");
    }

    /// <summary>
    /// Được gọi từ ImprovedBorderGenerator khi border xong
    /// </summary>
    public void NotifyBorderComplete()
    {
        if (isBorderComplete)
        {
            LogWarning("Border already complete!");
            return;
        }

        isBorderComplete = true;
        UpdateProgress();
        LogInfo("✓ Border Generation Complete");
    }

    /// <summary>
    /// Được gọi từ PrefabSpawner khi spawn xong
    /// </summary>
    public void NotifySpawnComplete()
    {
        if (isSpawnComplete)
        {
            LogWarning("Spawn already complete!");
            return;
        }

        isSpawnComplete = true;
        UpdateProgress();
        LogInfo("✓ Prefab Spawning Complete");
    }

    /// <summary>
    /// Được gọi khi toàn bộ game (logic, UI, gameplay systems) đã sẵn sàng
    /// </summary>
    public void NotifyGameReadyComplete()
    {
        if (isGameReadyComplete)
        {
            LogWarning("GameReady already complete!");
            return;
        }

        isGameReadyComplete = true;
        UpdateProgress();
        LogInfo("✓ Game Ready Initialization Complete");

        // Khi mọi thứ đều xong → Unload loading scene
        if (IsAllComplete() && !isUnloadingLoading)
        {
            StartCoroutine(UnloadLoadingScene());
        }
    }

    #endregion

    #region Progress Management

    void UpdateProgress()
    {
        // Tính progress: Scene Load (10%) + Terrain (30%) + Border (20%) + Spawn (40%)
        currentProgress = sceneLoadWeight; // Scene đã load = 10%

        string status = "";
        string detail = "";

        // Phase 1: Terrain (10% -> 30%)
        if (isTerrainComplete)
        {
            currentProgress += terrainWeight;
            key = "LadingScene.GenerateMap.rendersatatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.renderdetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }
        else if (isSceneLoaded)
        {
            key = "LadingScene.GenerateMap.sceneloadedstatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.sceneloadeddetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }

        // Phase 2: Borders (30% -> 50%)
        if (isBorderComplete)
        {
            currentProgress += borderWeight;
            
            key = "LadingScene.GenerateMap.boderstatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.boderdetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }
        else if (isTerrainComplete)
        {
            key = "LadingScene.GenerateMap.boderingstatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.boderingdetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }

        // Phase 3: Spawning (50% -> 80%)
        if (isSpawnComplete)
        {
            currentProgress += spawnWeight;
            
            key = "LadingScene.GenerateMap.spawnstatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.spawndetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }
        else if (isBorderComplete)
        {
            key = "LadingScene.GenerateMap.spawningstatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.spawningdetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }

        // Phase 4: Game Ready (80% -> 100%)
        if (isGameReadyComplete)
        {
            currentProgress += readyWeight;
            
            key = "LadingScene.GenerateMap.readystatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.readydetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }
        else if (isSpawnComplete)
        {
            key = "LadingScene.GenerateMap.readyingstatus";
            status = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            key = "LadingScene.GenerateMap.readyingdetail";
            detail = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }

        UpdateUI(status, detail);
    }

    bool IsGenerationComplete()
    {
        return isSceneLoaded && isTerrainComplete && isBorderComplete && isSpawnComplete;
    }

    bool IsAllComplete()
    {
        return IsGenerationComplete() && isGameReadyComplete;
    }

    void UpdateUI(string status, string detail)
    {
        statusText = status;
        detailText = detail;
        
        if (showDebugLogs)
        {
            Debug.Log($"[LoadingScene] {status} | {detail} | {currentProgress * 100:F0}%");
        }
    }

    #endregion

    #region Scene Unloading

    IEnumerator UnloadLoadingScene()
    {
        isUnloadingLoading = true;

        LogInfo("=== GAME READY COMPLETE ===");

        // Hiển thị 100%
        currentProgress = 1f;
        key = "LadingScene.GenerateMap.canopenscenegamestatus";
        txt1 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        key = "LadingScene.GenerateMap.canopenscenegamedetail";
        txt2 = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        UpdateUI(txt1, txt2);

        // Delay để người chơi thấy "Complete"
        yield return new WaitForSeconds(delayBeforeUnloadLoading);

        LogInfo($"Unloading loading scene: {loadingScene.name}");

        // Unload Loading scene (chỉ giữ lại Game scene)
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(loadingScene);

        while (unloadOp != null && !unloadOp.isDone)
        {
            yield return null;
        }

        LogInfo("✓ Loading scene unloaded");
        LogInfo($"Active scene: {SceneManager.GetActiveScene().name}");
        LogInfo("=== LOADING COMPLETE ===");

        // Cleanup singleton
        if (Instance == this)
        {
            Instance = null;
        }

        // Destroy loading controller object
        Destroy(gameObject);
    }

    #endregion

    #region Logging

    void LogInfo(string message)
    {
        if (showDebugLogs)
            Debug.Log($"[LoadingScene] {message}");
    }

    void LogWarning(string message)
    {
        if (showDebugLogs)
            Debug.LogWarning($"[LoadingScene] ⚠ {message}");
    }

    #endregion

    #region Debug Tools

    [ContextMenu("Debug - Force Complete Terrain")]
    void DebugCompleteTerrain()
    {
        NotifyTerrainComplete();
    }

    [ContextMenu("Debug - Force Complete Border")]
    void DebugCompleteBorder()
    {
        NotifyBorderComplete();
    }

    [ContextMenu("Debug - Force Complete Spawn")]
    void DebugCompleteSpawn()
    {
        NotifySpawnComplete();
    }

    [ContextMenu("Debug - Force Complete All")]
    void DebugCompleteAll()
    {
        isSceneLoaded = true;
        NotifyTerrainComplete();
        NotifyBorderComplete();
        NotifySpawnComplete();
        NotifyGameReadyComplete();
    }

    [ContextMenu("Debug - Skip Loading (Unload Now)")]
    void DebugSkipLoading()
    {
        if (!isUnloadingLoading)
        {
            StopAllCoroutines();
            StartCoroutine(UnloadLoadingScene());
        }
    }

    [ContextMenu("Debug - Show Scene Info")]
    void DebugShowSceneInfo()
    {
        Debug.Log($"=== SCENE INFO ===");
        Debug.Log($"Loading Scene: {loadingScene.name} (isLoaded: {loadingScene.isLoaded})");
        Debug.Log($"Game Scene: {gameScene.name} (isLoaded: {gameScene.isLoaded})");
        Debug.Log($"Active Scene: {SceneManager.GetActiveScene().name}");
        Debug.Log($"Total Loaded Scenes: {SceneManager.sceneCount}");
        
        Debug.Log($"\n=== PROGRESS ===");
        Debug.Log($"Scene Loaded: {isSceneLoaded}");
        Debug.Log($"Terrain: {isTerrainComplete}");
        Debug.Log($"Border: {isBorderComplete}");
        Debug.Log($"Spawn: {isSpawnComplete}");
        Debug.Log($"Progress: {currentProgress * 100:F1}%");
    }

    #endregion
}