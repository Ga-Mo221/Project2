using System.Linq;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    #region Singleton
    public static DetectionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[DetectionManager] Instance created in Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Performance Settings")]
    [Tooltip("Số lần update mỗi giây (10-30 tối ưu)")]
    [Range(5f, 60f)]
    [SerializeField] private float updatesPerSecond = 15f;

    [Header("Grid Settings")]
    [Tooltip("Tự động tính toán grid size tối ưu dựa trên vị trí objects")]
    [SerializeField] private bool autoCalculateGridSize = true;
    
    [Tooltip("Số lượng cells mục tiêu trên mỗi chiều (chỉ dùng khi auto = true)")]
    [Range(10, 50)]
    [SerializeField] private int targetCellsPerAxis = 20;
    
    [Tooltip("Kích thước mỗi ô grid thủ công (chỉ dùng khi auto = false)")]
    [SerializeField] private float manualGridCellSize = 10f;
    
    private float gridCellSize = 10f; // Giá trị thực tế được sử dụng

    [Tooltip("Tự động điều chỉnh update rate dựa trên FPS")]
    [SerializeField] private bool adaptiveUpdateRate = true;

    [Header("Debug Info (Read Only)")]
    [SerializeField] private int activeRadaCount = 0;
    [SerializeField] private int activeDetectableCount = 0;
    [SerializeField] private int currentBatchIndex = 0;
    [SerializeField] private float currentUpdateInterval = 0f;
    [SerializeField] private float calculatedGridCellSize = 0f;
    [SerializeField] private int totalOccupiedCells = 0;

    [Foldout("Map Size (Auto-detected)")]
    [SerializeField] private float detectedMapWidth = 0f;
    [Foldout("Map Size (Auto-detected)")]
    [SerializeField] private float detectedMapHeight = 0f;
    [Foldout("Map Size (Auto-detected)")]
    [SerializeField] private Vector2 mapCenter = Vector2.zero;

    public bool canActiveOBJ = true;

    // Collections
    private List<Rada> allRadars = new List<Rada>(50);
    private List<IDetectable> allDetectables = new List<IDetectable>(1000);

    // Spatial Grid
    private Dictionary<Vector2Int, List<IDetectable>> spatialGrid = new Dictionary<Vector2Int, List<IDetectable>>();
    private HashSet<Vector2Int> dirtyGridCells = new HashSet<Vector2Int>();

    // Batch processing
    private int batchSize = 50;
    private float updateTimer = 0f;
    
    // Initialization flag
    private bool isInitialized = false;

    #region Lifecycle

    void Start()
    {
        InitializeSystem();
    }

    private void InitializeSystem()
    {
        if (isInitialized) return;
        
        currentUpdateInterval = 1f / updatesPerSecond;
        
        Debug.Log($"[DetectionManager] Initializing system...");
        
        isInitialized = true;
        
        // Tự động đăng ký các object đã tồn tại trong scene
        RegisterExistingObjects();
        
        // Tính toán grid size sau khi đã đăng ký objects
        CalculateOptimalGridSize();
    }

    private void CalculateOptimalGridSize()
    {
        if (autoCalculateGridSize && allDetectables.Count > 0)
        {
            // Tìm bounding box của tất cả objects
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            
            foreach (var det in allDetectables)
            {
                if (det == null) continue;
                Vector2 pos = det.Position;
                minX = Mathf.Min(minX, pos.x);
                maxX = Mathf.Max(maxX, pos.x);
                minY = Mathf.Min(minY, pos.y);
                maxY = Mathf.Max(maxY, pos.y);
            }
            
            // Thêm buffer 10% để objects ở rìa không bị thiếu
            float bufferX = (maxX - minX) * 0.1f;
            float bufferY = (maxY - minY) * 0.1f;
            
            detectedMapWidth = (maxX - minX) + bufferX * 2;
            detectedMapHeight = (maxY - minY) + bufferY * 2;
            mapCenter = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
            
            // Tính grid size tối ưu: chia map thành targetCellsPerAxis cells
            float optimalSizeX = detectedMapWidth / targetCellsPerAxis;
            float optimalSizeY = detectedMapHeight / targetCellsPerAxis;
            gridCellSize = Mathf.Max(optimalSizeX, optimalSizeY);
            
            // Làm tròn để dễ debug
            gridCellSize = Mathf.Ceil(gridCellSize);
            
            // Đảm bảo grid size không quá nhỏ (tối thiểu 5) hoặc quá lớn (tối đa 50)
            gridCellSize = Mathf.Clamp(gridCellSize, 5f, 50f);
            
            calculatedGridCellSize = gridCellSize;
            
            Debug.Log($"[DetectionManager] AUTO Grid Size Calculated:");
            Debug.Log($"  Map bounds: X({minX:F1} to {maxX:F1}), Y({minY:F1} to {maxY:F1})");
            Debug.Log($"  Map size: {detectedMapWidth:F1} x {detectedMapHeight:F1}");
            Debug.Log($"  Map center: {mapCenter}");
            Debug.Log($"  Optimal grid cell size: {gridCellSize}");
            Debug.Log($"  Estimated cells: ~{Mathf.CeilToInt(detectedMapWidth / gridCellSize) * Mathf.CeilToInt(detectedMapHeight / gridCellSize)}");
        }
        else
        {
            // Dùng giá trị thủ công
            gridCellSize = manualGridCellSize;
            calculatedGridCellSize = gridCellSize;
            
            Debug.Log($"[DetectionManager] MANUAL Grid Size: {gridCellSize}");
        }
        
        // Re-organize tất cả objects vào grid với size mới
        ReorganizeGrid();
    }

    private void ReorganizeGrid()
    {
        // Clear grid cũ
        spatialGrid.Clear();
        
        // Thêm lại tất cả objects vào grid với size mới
        foreach (var det in allDetectables)
        {
            if (det == null) continue;
            
            Vector2Int gridPos = WorldToGrid(det.Position);
            
            if (!spatialGrid.ContainsKey(gridPos))
            {
                spatialGrid[gridPos] = new List<IDetectable>();
            }
            
            spatialGrid[gridPos].Add(det);
        }
        
        // Đếm số cell có object
        totalOccupiedCells = 0;
        foreach (var cell in spatialGrid.Values)
        {
            if (cell.Count > 0) totalOccupiedCells++;
        }
        
        Debug.Log($"[DetectionManager] Grid reorganized | Total cells: {spatialGrid.Count} | Occupied: {totalOccupiedCells} | Objects: {allDetectables.Count}");
    }

    private void RegisterExistingObjects()
    {
        // Tìm và đăng ký tất cả Rada trong scene
        Rada[] existingRadars = FindObjectsByType<Rada>(FindObjectsSortMode.None);
        foreach (var rada in existingRadars)
        {
            if (rada.gameObject.activeInHierarchy)
            {
                RegisterRadar(rada);
            }
        }

        // Tìm và đăng ký tất cả Display trong scene
        Display[] existingDisplays = FindObjectsByType<Display>(FindObjectsSortMode.None);
        foreach (var display in existingDisplays)
        {
            if (display.gameObject.activeInHierarchy)
            {
                RegisterDetectable(display);
            }
        }

        Debug.Log($"[DetectionManager] Auto-registered {existingRadars.Length} radars and {existingDisplays.Length} displays from scene");
    }

    void Update()
    {
        if (!isInitialized) return;

        // Adaptive update rate based on FPS
        if (adaptiveUpdateRate)
        {
            float fps = 1f / Time.deltaTime;
            if (fps < 30f)
            {
                currentUpdateInterval = Mathf.Min(currentUpdateInterval * 1.1f, 0.2f);
            }
            else if (fps > 50f)
            {
                currentUpdateInterval = Mathf.Max(currentUpdateInterval * 0.95f, 1f / updatesPerSecond);
            }
        }

        // Update timer
        updateTimer += Time.deltaTime;

        if (updateTimer >= currentUpdateInterval)
        {
            updateTimer = 0f;
            ProcessDetection();
        }
    }

    #endregion

    #region Registration

    public void RegisterRadar(Rada rada)
    {
        if (rada == null) return;
        
        if (!allRadars.Contains(rada))
        {
            allRadars.Add(rada);
            activeRadaCount = allRadars.Count;
            Debug.Log($"[DetectionManager] Radar registered: {rada.gameObject.name} | Total: {activeRadaCount}");
        }
    }

    public void UnregisterRadar(Rada rada)
    {
        if (rada == null) return;
        
        if (allRadars.Remove(rada))
        {
            activeRadaCount = allRadars.Count;
            Debug.Log($"[DetectionManager] Radar unregistered: {rada.gameObject.name} | Total: {activeRadaCount}");

            // Duyệt qua snapshot để tránh lỗi Collection modified
            var snapshot = new List<IDetectable>(allDetectables);
            foreach (var detectable in snapshot)
            {
                if (detectable != null)
                    detectable.RemoveSeemer(rada);
            }
        }
    }

    public void RegisterDetectable(IDetectable detectable)
    {
        if (detectable == null) return;
        
        if (!allDetectables.Contains(detectable))
        {
            allDetectables.Add(detectable);
            activeDetectableCount = allDetectables.Count;

            // Nếu chưa init hoặc đang ở giai đoạn đầu, chỉ add vào list
            // Grid sẽ được tính toán sau trong CalculateOptimalGridSize()
            if (!isInitialized)
            {
                return;
            }

            // Add to spatial grid (tạo cell mới nếu cần)
            Vector2Int gridPos = WorldToGrid(detectable.Position);
            
            if (!spatialGrid.ContainsKey(gridPos))
            {
                spatialGrid[gridPos] = new List<IDetectable>();
            }
            
            spatialGrid[gridPos].Add(detectable);
            
            // Update occupied cell count
            if (spatialGrid[gridPos].Count == 1)
            {
                totalOccupiedCells++;
            }
            
            // Debug (chỉ log 5 object đầu)
            if (activeDetectableCount <= 5)
            {
                Display display = detectable as Display;
                string objName = display != null ? display.gameObject.name : "Unknown";
                Debug.Log($"[Grid] '{objName}' registered at cell {gridPos} | World pos: {detectable.Position}");
            }
        }
    }

    public void UnregisterDetectable(IDetectable detectable)
    {
        if (detectable == null) return;
        
        if (allDetectables.Remove(detectable))
        {
            activeDetectableCount = allDetectables.Count;

            // Remove from spatial grid
            Vector2Int gridPos = WorldToGrid(detectable.Position);
            if (spatialGrid.ContainsKey(gridPos))
            {
                spatialGrid[gridPos].Remove(detectable);
            }
        }
    }

    public void UpdateDetectablePosition(IDetectable detectable, Vector2 oldPos, Vector2 newPos)
    {
        Vector2Int oldGrid = WorldToGrid(oldPos);
        Vector2Int newGrid = WorldToGrid(newPos);

        if (oldGrid != newGrid)
        {
            // Remove from old cell
            if (spatialGrid.ContainsKey(oldGrid))
            {
                spatialGrid[oldGrid].Remove(detectable);
            }

            // Add to new cell (tạo mới nếu cần)
            if (!spatialGrid.ContainsKey(newGrid))
            {
                spatialGrid[newGrid] = new List<IDetectable>();
            }
            spatialGrid[newGrid].Add(detectable);

            // Đánh dấu cả 2 ô là dirty
            dirtyGridCells.Add(oldGrid);
            dirtyGridCells.Add(newGrid);
        }
    }

    #endregion

    #region Detection Processing

    private void ProcessDetection()
    {
        // Early exit nếu không có rada hoặc detectables
        if (allRadars.Count == 0 || allDetectables.Count == 0)
            return;

        // Batch processing - mỗi frame chỉ xử lý 1 phần
        int startIdx = currentBatchIndex;
        batchSize = Mathf.Max(1, Mathf.CeilToInt(allRadars.Count / 2f));
        int endIdx = Mathf.Min(startIdx + batchSize, allRadars.Count);

        for (int i = startIdx; i < endIdx; i++)
        {
            Rada rada = allRadars[i];

            if (rada == null || rada.IsDead || !rada.IsActive)
                continue;

            ProcessRadarDetection(rada);
        }

        // Update batch index
        currentBatchIndex = endIdx;
        if (currentBatchIndex >= allRadars.Count)
        {
            currentBatchIndex = 0;
        }
    }

    private void ProcessRadarDetection(Rada rada)
    {
        Vector2 radarPos = rada.Position;
        float detectionRadius = rada.DetectionRadius;
        float displayRadius = rada.DisplayRadius;

        HashSet<Vector2Int> gridCellsToCheck = GetGridCellsInRadius(radarPos, detectionRadius);

        foreach (Vector2Int gridCell in gridCellsToCheck)
        {
            if (!spatialGrid.TryGetValue(gridCell, out var objectsInCell))
                continue;
            
            // Snapshot danh sách để tránh lỗi khi RemoveSeemer() thay đổi grid
            var snapshot = new List<IDetectable>(objectsInCell);

            foreach (var detectable in snapshot)
            {
                if (detectable == null || !detectable.IsValid)
                    continue;
                
                float distance = Vector2.Distance(radarPos, detectable.Position);
                bool wasSeeing = detectable.HasSeemer(rada);

                if (distance <= displayRadius)
                {
                    if (!wasSeeing)
                        detectable.AddSeemer(rada);
                }
                else
                {
                    if (wasSeeing)
                        detectable.RemoveSeemer(rada);
                }
            }
        }
    }

    #endregion

    #region Spatial Grid Helpers

    private Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / gridCellSize),
            Mathf.FloorToInt(worldPos.y / gridCellSize)
        );
    }

    private HashSet<Vector2Int> GetGridCellsInRadius(Vector2 center, float radius)
    {
        HashSet<Vector2Int> cells = new HashSet<Vector2Int>();

        Vector2Int centerGrid = WorldToGrid(center);
        int cellRadius = Mathf.CeilToInt(radius / gridCellSize);

        for (int x = -cellRadius; x <= cellRadius; x++)
        {
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                cells.Add(new Vector2Int(centerGrid.x + x, centerGrid.y + y));
            }
        }

        return cells;
    }

    #endregion

    #region Debug

    [Header("Debug Visualization")]
    [SerializeField] private bool showGridGizmos = false;
    [SerializeField] private bool showOnlyOccupiedCells = true;

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !showGridGizmos || spatialGrid == null) return;

        // Vẽ spatial grid
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange with transparency

        if (showOnlyOccupiedCells)
        {
            // Chỉ vẽ các ô có object
            foreach (var kvp in spatialGrid)
            {
                if (kvp.Value.Count > 0)
                {
                    Vector2 worldPos = new Vector2(
                        kvp.Key.x * gridCellSize,
                        kvp.Key.y * gridCellSize
                    );

                    Gizmos.DrawWireCube(
                        new Vector3(worldPos.x + gridCellSize * 0.5f, worldPos.y + gridCellSize * 0.5f, 0),
                        new Vector3(gridCellSize, gridCellSize, 0.1f)
                    );

                    // Hiển thị số lượng object trong cell
                    #if UNITY_EDITOR
                    UnityEditor.Handles.Label(
                        new Vector3(worldPos.x + gridCellSize * 0.5f, worldPos.y + gridCellSize * 0.5f, 0),
                        kvp.Value.Count.ToString()
                    );
                    #endif
                }
            }
        }
        else
        {
            // Vẽ tất cả các ô (có thể lag nếu grid lớn)
            foreach (var kvp in spatialGrid)
            {
                Vector2 worldPos = new Vector2(
                    kvp.Key.x * gridCellSize,
                    kvp.Key.y * gridCellSize
                );

                Gizmos.color = kvp.Value.Count > 0 ? 
                    new Color(0f, 1f, 0f, 0.3f) : // Green if occupied
                    new Color(0.5f, 0.5f, 0.5f, 0.1f); // Gray if empty

                Gizmos.DrawWireCube(
                    new Vector3(worldPos.x + gridCellSize * 0.5f, worldPos.y + gridCellSize * 0.5f, 0),
                    new Vector3(gridCellSize, gridCellSize, 0.1f)
                );
            }
        }

        // Vẽ radar ranges
        if (allRadars != null)
        {
            foreach (var radar in allRadars)
            {
                if (radar != null && !radar.IsDead)
                {
                    // Display radius (đỏ)
                    Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                    Gizmos.DrawWireSphere(radar.Position, radar.DisplayRadius);

                    // Detection radius (xanh)
                    Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
                    Gizmos.DrawWireSphere(radar.Position, radar.DetectionRadius);
                }
            }
        }
    }

    [ContextMenu("Debug - Show Stats")]
    void DebugShowStats()
    {
        Debug.Log("=== DETECTION MANAGER STATS ===");
        Debug.Log($"Active Radars: {activeRadaCount}");
        Debug.Log($"Active Detectables: {activeDetectableCount}");
        Debug.Log($"Grid Cells Total: {spatialGrid.Count}");
        Debug.Log($"Grid Cells Occupied: {GetOccupiedCellCount()}");
        Debug.Log($"Update Rate: {1f / currentUpdateInterval:F1}/s");
        Debug.Log($"Batch Size: {batchSize}");
        Debug.Log($"Batch Progress: {currentBatchIndex}/{allRadars.Count}");
        
        // Chi tiết radar
        Debug.Log("\n--- RADARS ---");
        for (int i = 0; i < allRadars.Count; i++)
        {
            var radar = allRadars[i];
            if (radar != null)
            {
                Debug.Log($"  [{i}] {radar.gameObject.name} - Pos: {radar.Position} | Display: {radar.DisplayRadius} | Detection: {radar.DetectionRadius} | Active: {radar.IsActive}");
            }
        }
        
        // Chi tiết detectable
        Debug.Log("\n--- DETECTABLES (first 10) ---");
        for (int i = 0; i < Mathf.Min(10, allDetectables.Count); i++)
        {
            var det = allDetectables[i];
            if (det != null)
            {
                Display display = det as Display;
                if (display != null)
                {
                    Debug.Log($"  [{i}] {display.gameObject.name} - Pos: {det.Position} | Valid: {det.IsValid} | Detected: {display.checkDetec()}");
                }
            }
        }
    }

    [ContextMenu("Debug - Force Re-register All")]
    void DebugForceReregister()
    {
        RegisterExistingObjects();
    }

    [ContextMenu("Debug - Clear All")]
    void DebugClearAll()
    {
        allRadars.Clear();
        allDetectables.Clear();
        foreach (var cell in spatialGrid.Values)
        {
            cell.Clear();
        }
        activeRadaCount = 0;
        activeDetectableCount = 0;
        Debug.Log("[DetectionManager] All data cleared");
    }

    [ContextMenu("Debug - Recalculate Grid Size")]
    void DebugRecalculateGrid()
    {
        Debug.Log("[DetectionManager] Manually recalculating grid...");
        CalculateOptimalGridSize();
    }

    [ContextMenu("Debug - Analyze Grid Distribution")]
    void DebugAnalyzeGrid()
    {
        Debug.Log("=== GRID DISTRIBUTION ANALYSIS ===");
        Debug.Log($"Grid Mode: {(autoCalculateGridSize ? "AUTO" : "MANUAL")}");
        Debug.Log($"Grid Cell Size: {gridCellSize}");
        Debug.Log($"Total Detectables: {allDetectables.Count}");
        Debug.Log($"Total Grid Cells: {spatialGrid.Count}");
        Debug.Log($"Occupied Cells: {totalOccupiedCells}");
        
        // Tìm cell có nhiều object nhất
        int maxObjectsInCell = 0;
        Vector2Int maxCell = Vector2Int.zero;
        
        foreach (var kvp in spatialGrid)
        {
            if (kvp.Value.Count > maxObjectsInCell)
            {
                maxObjectsInCell = kvp.Value.Count;
                maxCell = kvp.Key;
            }
        }
        
        Debug.Log($"Max objects in single cell: {maxObjectsInCell} at {maxCell}");
        
        // Phân bố objects per cell
        Dictionary<int, int> distribution = new Dictionary<int, int>();
        foreach (var kvp in spatialGrid)
        {
            int count = kvp.Value.Count;
            if (count > 0)
            {
                if (!distribution.ContainsKey(count))
                    distribution[count] = 0;
                distribution[count]++;
            }
        }
        
        Debug.Log("\n--- OBJECTS PER CELL DISTRIBUTION ---");
        foreach (var kvp in distribution.OrderBy(x => x.Key))
        {
            Debug.Log($"  {kvp.Value} cells have {kvp.Key} object(s)");
        }
        
        // Top 5 most occupied cells
        Debug.Log("\n--- TOP 5 MOST OCCUPIED CELLS ---");
        var sorted = spatialGrid
            .Where(x => x.Value.Count > 0)
            .OrderByDescending(x => x.Value.Count)
            .Take(5);
            
        foreach (var cell in sorted)
        {
            Vector2 worldPos = new Vector2(cell.Key.x * gridCellSize, cell.Key.y * gridCellSize);
            Debug.Log($"  Cell {cell.Key} (World: {worldPos}) has {cell.Value.Count} objects");
        }
    }

    [ContextMenu("Debug - Test Detection")]
    void DebugTestDetection()
    {
        if (allRadars.Count == 0)
        {
            Debug.LogWarning("[DetectionManager] No radars to test!");
            return;
        }

        if (allDetectables.Count == 0)
        {
            Debug.LogWarning("[DetectionManager] No detectables to test!");
            return;
        }

        Debug.Log($"[DetectionManager] Testing detection with {allRadars.Count} radars and {allDetectables.Count} detectables...");
        
        foreach (var radar in allRadars)
        {
            if (radar == null) continue;
            
            int detectedCount = 0;
            foreach (var detectable in allDetectables)
            {
                if (detectable == null) continue;
                
                float dist = Vector2.Distance(radar.Position, detectable.Position);
                if (dist <= radar.DisplayRadius)
                {
                    detectedCount++;
                }
            }
            
            Debug.Log($"  Radar '{radar.gameObject.name}' should detect {detectedCount} objects");
        }
    }

    private int GetOccupiedCellCount()
    {
        return totalOccupiedCells;
    }

    void OnValidate()
    {
        // Khi user thay đổi settings trong Inspector
        if (Application.isPlaying && isInitialized)
        {
            if (!autoCalculateGridSize)
            {
                // Nếu chuyển sang manual mode, update grid size
                if (Mathf.Abs(gridCellSize - manualGridCellSize) > 0.01f)
                {
                    Debug.Log($"[DetectionManager] Grid size changed to {manualGridCellSize} (manual mode)");
                    gridCellSize = manualGridCellSize;
                    calculatedGridCellSize = gridCellSize;
                    ReorganizeGrid();
                }
            }
        }
    }

    #endregion
}

public interface IDetectable
{
    Vector2 Position { get; }
    bool IsValid { get; }
    bool HasSeemer(Rada seemer);
    void AddSeemer(Rada seemer);
    void RemoveSeemer(Rada seemer);
}


// using System.Collections.Generic;
// using NaughtyAttributes;
// using UnityEngine;

// public class DetectionManager : MonoBehaviour
// {
//     #region Singleton
//     public static DetectionManager Instance { get; private set; }

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//     #endregion

//     [Header("Performance Settings")]
//     [Tooltip("Số lần update mỗi giây (10-30 tối ưu)")]
//     [Range(5f, 60f)]
//     [SerializeField] private float updatesPerSecond = 15f;

//     [Tooltip("Kích thước mỗi ô grid (càng nhỏ = chính xác hơn nhưng nhiều ô hơn)")]
//     [SerializeField] private float gridCellSize = 20f;

//     [Tooltip("Tự động điều chỉnh update rate dựa trên FPS")]
//     [SerializeField] private bool adaptiveUpdateRate = true;

//     [Header("Debug Info (Read Only)")]
//     [SerializeField] private int activeRadaCount = 0;
//     [SerializeField] private int activeDetectableCount = 0;
//     [SerializeField] private int currentBatchIndex = 0;
//     [SerializeField] private float currentUpdateInterval = 0f;

//     [Foldout("Map Size")]
//     [SerializeField] private float mapWidth = 200f;
//     [Foldout("Map Size")]
//     [SerializeField] private float mapHeight = 200f;

//     public bool canActiveOBJ = true;

//     // Collections
//     [SerializeField] private List<Rada> allRadars = new List<Rada>(50);
//     [SerializeField] private List<IDetectable> allDetectables = new List<IDetectable>(1000);

//     // Spatial Grid
//     private Dictionary<Vector2Int, List<IDetectable>> spatialGrid = new Dictionary<Vector2Int, List<IDetectable>>();
//     private HashSet<Vector2Int> dirtyGridCells = new HashSet<Vector2Int>();

//     // Batch processing
//     private int batchSize = 50; // Xử lý 50 objects mỗi frame
//     private float updateTimer = 0f;
//     //private bool isDirty = false;

//     #region Lifecycle

//     void Start()
//     {
//         currentUpdateInterval = 1f / updatesPerSecond;

//         // Pre-allocate spatial grid (giả sử map 200x200)
//         int gridWidth = Mathf.CeilToInt(mapWidth / gridCellSize);
//         int gridHeight = Mathf.CeilToInt(mapHeight / gridCellSize);

//         for (int x = -gridWidth; x <= gridWidth; x++)
//         {
//             for (int y = -gridHeight; y <= gridHeight; y++)
//             {
//                 spatialGrid[new Vector2Int(x, y)] = new List<IDetectable>(20);
//             }
//         }

//         Debug.Log($"[DetectionManager] Initialized | Grid cells: {spatialGrid.Count} | Update rate: {updatesPerSecond}/s");
//     }

//     void Update()
//     {
//         // Adaptive update rate based on FPS
//         if (adaptiveUpdateRate)
//         {
//             float fps = 1f / Time.deltaTime;
//             if (fps < 30f)
//             {
//                 currentUpdateInterval = Mathf.Min(currentUpdateInterval * 1.1f, 0.2f); // Giảm tốc độ
//             }
//             else if (fps > 50f)
//             {
//                 currentUpdateInterval = Mathf.Max(currentUpdateInterval * 0.95f, 1f / updatesPerSecond);
//             }
//         }

//         // Update timer
//         updateTimer += Time.deltaTime;

//         if (updateTimer >= currentUpdateInterval)
//         {
//             updateTimer = 0f;
//             ProcessDetection();
//         }
//     }

//     #endregion

//     #region Registration

//     /// <summary>
//     /// Đăng ký Rada (detector)
//     /// </summary>
//     public void RegisterRadar(Rada rada)
//     {
//         if (!allRadars.Contains(rada))
//         {
//             allRadars.Add(rada);
//             activeRadaCount = allRadars.Count;
//             //isDirty = true;
//         }
//     }

//     /// <summary>
//     /// Hủy đăng ký Rada
//     /// </summary>
//     public void UnregisterRadar(Rada rada)
//     {
//         if (allRadars.Remove(rada))
//         {
//             activeRadaCount = allRadars.Count;

//             // Duyệt qua snapshot để tránh lỗi Collection modified
//             var snapshot = new List<IDetectable>(allDetectables);
//             foreach (var detectable in snapshot)
//             {
//                 if (detectable != null)
//                     detectable.RemoveSeemer(rada);
//             }
//         }
//     }

//     /// <summary>
//     /// Đăng ký detectable object
//     /// </summary>
//     public void RegisterDetectable(IDetectable detectable)
//     {
//         if (!allDetectables.Contains(detectable))
//         {
//             allDetectables.Add(detectable);
//             activeDetectableCount = allDetectables.Count;

//             // Add to spatial grid
//             Vector2Int gridPos = WorldToGrid(detectable.Position);
//             if (spatialGrid.ContainsKey(gridPos))
//             {
//                 spatialGrid[gridPos].Add(detectable);
//             }

//             //isDirty = true;
//         }
//     }

//     /// <summary>
//     /// Hủy đăng ký detectable object
//     /// </summary>
//     public void UnregisterDetectable(IDetectable detectable)
//     {
//         if (allDetectables.Remove(detectable))
//         {
//             activeDetectableCount = allDetectables.Count;

//             // Remove from spatial grid
//             Vector2Int gridPos = WorldToGrid(detectable.Position);
//             if (spatialGrid.ContainsKey(gridPos))
//             {
//                 spatialGrid[gridPos].Remove(detectable);
//             }
//         }
//     }

//     /// <summary>
//     /// Cập nhật vị trí của detectable trong grid
//     /// </summary>
//     public void UpdateDetectablePosition(IDetectable detectable, Vector2 oldPos, Vector2 newPos)
//     {
//         Vector2Int oldGrid = WorldToGrid(oldPos);
//         Vector2Int newGrid = WorldToGrid(newPos);

//         if (oldGrid != newGrid)
//         {
//             // Di chuyển sang ô mới
//             if (spatialGrid.ContainsKey(oldGrid))
//                 spatialGrid[oldGrid].Remove(detectable);

//             if (spatialGrid.ContainsKey(newGrid))
//                 spatialGrid[newGrid].Add(detectable);

//             // Đánh dấu cả 2 ô là dirty
//             dirtyGridCells.Add(oldGrid);
//             dirtyGridCells.Add(newGrid);
//         }
//     }

//     #endregion

//     #region Detection Processing

//     private void ProcessDetection()
//     {
//         // Early exit nếu không có rada hoặc detectables
//         if (allRadars.Count == 0 || allDetectables.Count == 0)
//             return;

//         // Batch processing - mỗi frame chỉ xử lý 1 phần
//         int startIdx = currentBatchIndex;
//         batchSize = Mathf.CeilToInt(allRadars.Count / 2f); // mỗi frame xử lý nửa số radar
//         int endIdx = Mathf.Min(startIdx + batchSize, allRadars.Count);

//         for (int i = startIdx; i < endIdx; i++)
//         {
//             Rada rada = allRadars[i];

//             if (rada == null || rada.IsDead)
//                 continue;

//             ProcessRadarDetection(rada);
//         }

//         // Update batch index
//         currentBatchIndex = endIdx;
//         if (currentBatchIndex >= allRadars.Count)
//         {
//             currentBatchIndex = 0; // Reset về đầu
//         }
//     }

//     private void ProcessRadarDetection(Rada rada)
//     {
//         Vector2 radarPos = rada.Position;
//         float detectionRadius = rada.DetectionRadius;
//         float displayRadius = rada.DisplayRadius;

//         HashSet<Vector2Int> gridCellsToCheck = GetGridCellsInRadius(radarPos, detectionRadius);

//         foreach (Vector2Int gridCell in gridCellsToCheck)
//         {
//             if (!spatialGrid.TryGetValue(gridCell, out var objectsInCell))
//                 continue;
//             // Snapshot danh sách để tránh lỗi khi RemoveSeemer() thay đổi grid
//             var snapshot = new List<IDetectable>(objectsInCell);

//             foreach (var detectable in snapshot)
//             {
//                 if (detectable == null || !detectable.IsValid)
//                     continue;
//                 float distance = Vector2.Distance(radarPos, detectable.Position);
//                 bool wasSeeing = detectable.HasSeemer(rada);

//                 if (distance < displayRadius)
//                 {
//                     if (!wasSeeing)
//                         detectable.AddSeemer(rada);
//                 }
//                 else
//                 {
//                     if (wasSeeing)
//                         detectable.RemoveSeemer(rada);
//                 }
//             }
//         }
//     }

//     #endregion

//     #region Spatial Grid Helpers

//     private Vector2Int WorldToGrid(Vector2 worldPos)
//     {
//         return new Vector2Int(
//             Mathf.FloorToInt(worldPos.x / gridCellSize),
//             Mathf.FloorToInt(worldPos.y / gridCellSize)
//         );
//     }

//     private HashSet<Vector2Int> GetGridCellsInRadius(Vector2 center, float radius)
//     {
//         HashSet<Vector2Int> cells = new HashSet<Vector2Int>();

//         Vector2Int centerGrid = WorldToGrid(center);
//         int cellRadius = Mathf.CeilToInt(radius / gridCellSize);

//         for (int x = -cellRadius; x <= cellRadius; x++)
//         {
//             for (int y = -cellRadius; y <= cellRadius; y++)
//             {
//                 cells.Add(new Vector2Int(centerGrid.x + x, centerGrid.y + y));
//             }
//         }

//         return cells;
//     }

//     #endregion

//     #region Debug

//     void OnDrawGizmos()
//     {
//         if (!Application.isPlaying) return;

//         // Vẽ spatial grid
//         Gizmos.color = Color.orange;
//         foreach (var kvp in spatialGrid)
//         {
//             if (kvp.Value.Count > 0)
//             {
//                 Vector2 worldPos = new Vector2(
//                     kvp.Key.x * gridCellSize,
//                     kvp.Key.y * gridCellSize
//                 );

//                 Gizmos.DrawWireCube(
//                     worldPos + Vector2.one * gridCellSize * 0.5f,
//                     Vector2.one * gridCellSize
//                 );
//             }
//         }
//     }

//     [ContextMenu("Debug - Show Stats")]
//     void DebugShowStats()
//     {
//         Debug.Log("=== DETECTION MANAGER STATS ===");
//         Debug.Log($"Active Radars: {activeRadaCount}");
//         Debug.Log($"Active Detectables: {activeDetectableCount}");
//         Debug.Log($"Grid Cells: {spatialGrid.Count}");
//         Debug.Log($"Update Rate: {1f / currentUpdateInterval:F1}/s");
//         Debug.Log($"Batch Size: {batchSize}");
//         Debug.Log($"Batch Progress: {currentBatchIndex}/{allRadars.Count}");
//     }

//     #endregion
// }

// public interface IDetectable
// {
//     Vector2 Position { get; }
//     bool IsValid { get; }
//     bool HasSeemer(Rada seemer);
//     void AddSeemer(Rada seemer);
//     void RemoveSeemer(Rada seemer);
// }