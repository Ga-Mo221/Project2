using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Professional Prefab Spawning System
/// Handles spawning of all game objects on procedurally generated terrain
/// </summary>
public class PrefabSpawner : MonoBehaviour
{
    #region Serialized Fields
    [Header("=== Helpers ===")]
    [SerializeField] private TilemapHelper tilemapHelper;
    
    [Header("=== Core References ===")]
    [Tooltip("Tham chiếu đến script tạo địa hình - BẮT BUỘC")]
    [SerializeField] private LayeredTerrainGenerator terrainGenerator;
    [Tooltip("Tham chiếu đến script tạo viền map - BẮT BUỘC")]
    [SerializeField] private ImprovedBorderGenerator borderGenerator;
    
    [Header("=== Terrain Layers ===")]
    [Tooltip("Tilemap nền chính (đất) - BẮT BUỘC | Đây là layer cơ bản nhất của map")]
    [SerializeField] private Tilemap groundTilemap;
    [Tooltip("Tilemap cỏ phụ - TÙY CHỌN | Nếu có thì animals sẽ spawn ưu tiên trên đây")]
    [SerializeField] private Tilemap grassTilemap;

    [Header("=== Global Spawn Rules ===")]
    [Tooltip("Khoảng cách tối thiểu từ rìa map cho các công trình (đơn vị: ô lưới)\n" +
             "- Áp dụng cho: Castles, Lửa trại, Storage, Tower, Gold Mine\n" +
             "- Giá trị đề xuất: 10-20\n" +
             "- Càng lớn = công trình càng xa rìa = an toàn hơn")]
    [SerializeField] private float minDistanceFromMapEdge = 15f;

    [Header("=== Castles ===")]
    [Tooltip("Prefab lâu đài người chơi - BẮT BUỘC")]
    [SerializeField] private GameObject playerCastle;
    [Tooltip("Prefab lâu đài địch - BẮT BUỘC")]
    [SerializeField] private GameObject enemyCastle;
    
    [Range(0.4f, 0.8f)]
    [Tooltip("Khoảng cách tối thiểu giữa 2 lâu đài (% đường chéo map)\n" +
             "- 0.4 = 40% = Gần nhau\n" +
             "- 0.6 = 60% = Vừa phải (đề xuất)\n" +
             "- 0.8 = 80% = Rất xa nhau\n" +
             "VD: Map 100x100 có đường chéo ≈141 → 60% = khoảng cách tối thiểu 85 ô")]
    [SerializeField] private float minCastleDistancePercent = 0.6f;
    
    [Tooltip("Số lần thử tìm vị trí tốt nhất cho Enemy Castle\n" +
             "- Giá trị cao = tìm vị trí xa hơn nhưng lâu hơn\n" +
             "- Đề xuất: 100-300")]
    [SerializeField] private int maxCastleAttempts = 200;

    [Header("=== Enemy Structures ===")]
    [Tooltip("Prefab lửa trại địch")]
    [SerializeField] private GameObject luaTrai;
    [Tooltip("Cài đặt spawn cho Lửa Trại:\n" +
             "- minCount: Số lượng tối thiểu\n" +
             "- maxCount: Số lượng tối đa\n" +
             "- minDistance: Khoảng cách tối thiểu giữa các lửa trại\n" +
             "- maxDistance: Khoảng cách tối đa giữa các lửa trại")]
    [SerializeField] private SpawnSettings luaTraiSettings = new SpawnSettings(3, 6, 20f, 50f);
    [Tooltip("Bán kính bảo vệ quanh Player Castle (đơn vị: ô lưới)\n" +
             "- Lửa trại KHÔNG spawn trong vùng này\n" +
             "- Đề xuất: 25-35\n" +
             "- Giúp người chơi có không gian an toàn ở đầu game")]
    [SerializeField] private float luaTraiMinDistanceFromPlayer = 30f;
    
    [Tooltip("Prefab kho địch")]
    [SerializeField] private GameObject enemyStorage;
    [Tooltip("Cài đặt spawn cho Enemy Storage")]
    [SerializeField] private SpawnSettings storageSettings = new SpawnSettings(3, 5, 15f, 30f);
    [Tooltip("Bán kính bảo vệ quanh Player Castle cho Storage\n" +
             "- NÊN LỚN HƠN luaTraiMinDistanceFromPlayer\n" +
             "- Đề xuất: 30-40\n" +
             "- Storage quan trọng hơn nên cần xa hơn")]
    [SerializeField] private float storageMinDistanceFromPlayer = 35f;
    
    [Tooltip("Prefab tháp canh địch")]
    [SerializeField] private GameObject enemyTower;
    [Range(0f, 1f)]
    [Tooltip("Xác suất spawn tháp quanh mỗi công trình địch\n" +
             "- 0.0 = 0% = Không có tháp\n" +
             "- 0.3 = 30% = Vừa phải (đề xuất)\n" +
             "- 1.0 = 100% = Mọi công trình đều có tháp")]
    [SerializeField] private float towerSpawnChance = 0.3f;
    [Tooltip("Bán kính spawn tháp xung quanh công trình (đơn vị: ô lưới)\n" +
             "- Tháp sẽ spawn trong vòng tròn này\n" +
             "- Đề xuất: 15-30")]
    [SerializeField] private float towerSpawnRadius = 25f;

    [Header("=== Resources ===")]
    [Tooltip("Prefab mỏ vàng")]
    [SerializeField] private GameObject goldMine;
    [Tooltip("Cài đặt spawn cho Gold Mine\n" +
             "- minDistance/maxDistance nên lớn để tránh tập trung")]
    [SerializeField] private SpawnSettings goldMineSettings = new SpawnSettings(2, 4, 30f, 60f);
    [Tooltip("Bán kính spawn mỏ vàng đầu tiên gần Player Castle\n" +
             "- Đảm bảo người chơi có tài nguyên ban đầu\n" +
             "- Đề xuất: 30-50")]
    [SerializeField] private float goldMineNearPlayerRadius = 40f;
    [SerializeField] private float addminDistFromMapEdge = 5f;

    [Header("=== Nature - Trees ===")]
    [Tooltip("Danh sách prefab cây - Chọn ngẫu nhiên từ list này\n" +
             "- Càng nhiều loại = map càng đa dạng")]
    [SerializeField] private List<GameObject> trees = new List<GameObject>();
    [Tooltip("Số cây spawn dọc theo rìa map\n" +
             "- Tạo hiệu ứng rừng bao quanh map\n" +
             "- Đề xuất: 100-200")]
    [SerializeField] private int treeEdgeCount = 150;
    [Tooltip("Số cây spawn ngẫu nhiên khắp map\n" +
             "- Tạo cây rải rác trong map\n" +
             "- Đề xuất: 30-100")]
    [SerializeField] private int treeRandomCount = 50;
    [Tooltip("Độ sâu spawn cây từ rìa vào trong (đơn vị: ô lưới)\n" +
             "- Cây sẽ spawn trong khoảng 0 đến edgeDepth từ rìa\n" +
             "- 0 = Sát rìa\n" +
             "- 10 = Vào trong 10 ô\n" +
             "- Đề xuất: 5-15")]
    [SerializeField] private float edgeDepth = 10f;
    [Tooltip("Khoảng cách tối thiểu giữa các cây (đơn vị: ô lưới)\n" +
             "- Tránh cây chồng lên nhau\n" +
             "- Đề xuất: 1.5-3")]
    [SerializeField] private float treeMinSpacing = 2f;

    [Header("=== Nature - Rocks ===")]
    [Tooltip("Danh sách prefab đá - Chọn ngẫu nhiên")]
    [SerializeField] private List<GameObject> rocks = new List<GameObject>();
    [Tooltip("Tổng số đá spawn trên map\n" +
             "- Spawn ngẫu nhiên khắp map\n" +
             "- Đề xuất: 50-150")]
    [SerializeField] private int rockCount = 80;
    [Range(0.1f, 0.9f)]
    [SerializeField] private float RockGrassSpawnChance = 0.2f;

    [Header("=== Animals ===")]
    [Tooltip("Prefab con gấu")]
    [SerializeField] private GameObject animalBear;
    [Tooltip("Prefab con nhện")]
    [SerializeField] private GameObject animalSpider;
    [Tooltip("Prefab con cừu")]
    [SerializeField] private GameObject animalSheep;
    [Tooltip("Prefab con rắn")]
    [SerializeField] private GameObject animalSnake;
    [Tooltip("Cài đặt spawn chung cho TẤT CẢ loại động vật\n" +
             "- Mỗi loại sẽ spawn số lượng trong khoảng min-max này\n" +
             "- VD: (15, 25) = mỗi loại spawn 15-25 con")]
    [SerializeField] private SpawnSettings animalSettings = new SpawnSettings(15, 25, 15f, 40f);
    [Range(0f, 1f)]
    [Tooltip("Xác suất spawn động vật trên grass layer (vs ground)\n" +
             "- 0.0 = Chỉ spawn trên ground\n" +
             "- 0.6 = 60% grass, 40% ground (đề xuất)\n" +
             "- 1.0 = Chỉ spawn trên grass\n" +
             "CHÚ Ý: Chỉ có tác dụng khi grassTilemap được gán")]
    [SerializeField] private float animalGrassSpawnChance = 0.6f;

    [Header("=== Decorations ===")]
    [Tooltip("Prefab ngọn đuốc")]
    [SerializeField] private GameObject ngonDut;
    [Tooltip("Cài đặt spawn cho Ngọn Đuốc\n" +
             "- Decoration nên có số lượng nhiều\n" +
             "- minDistance nhỏ để phân bố rộng")]
    [SerializeField] private SpawnSettings ngonDutSettings = new SpawnSettings(20, 40, 10f, 20f);
    [Tooltip("List Prefab Trang Trí khác \n" +
             "- Chọn ngẫu nhiên từ list này\n" +
             "- VD: Hoa, bụi cỏ, đá nhỏ, v.v. \n" +
             "- Spawn khắp map để làm đẹp môi trường")]
    [SerializeField] private List<GameObject> decorationPrefabs = new List<GameObject>();
    [SerializeField] private int count = 500;
    [SerializeField] private float decorationGrassSpawnChance = 0.2f;

    [Header("=== Debug Settings ===")]
    [Tooltip("BẬT/TẮT hiển thị tất cả Gizmos debug\n" +
             "- BẬT = Thấy tất cả visualization trong Scene view\n" +
             "- TẮT = Không vẽ gì (giảm lag)")]
    [SerializeField] private bool showDebugGizmos = true;
    [Tooltip("BẬT/TẮT log thông tin spawn ra Console\n" +
             "- BẬT = Thấy chi tiết quá trình spawn\n" +
             "- TẮT = Console sạch sẽ")]
    [SerializeField] private bool showDebugLogs = true;
    [Tooltip("Hiển thị edges của map (rìa)\n" +
             "- ĐỎ = Top, XANH DƯƠNG = Bottom\n" +
             "- VÀNG = Left, TÍM = Right\n" +
             "CHÚ Ý: Nhiều edges có thể gây lag, chỉ bật khi cần debug")]
    [SerializeField] private bool drawEdgeGizmos = false;
    [Tooltip("Hiển thị vị trí Ngọn Đuốc (vòng tròn vàng)\n" +
             "- Giúp kiểm tra phân bố decoration")]
    [SerializeField] private bool drawDecorationGizmos = true;
    [Tooltip("Hiển thị vùng bảo vệ quanh Player Castle\n" +
             "- Vòng tròn xanh = vùng Lửa Trại không spawn\n" +
             "- Vòng tròn xanh nhạt = vùng Storage không spawn")]
    [SerializeField] private bool drawProtectionZones = true;


    [Foldout("Parrent")]
    [SerializeField] private Transform player_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform enemybuilding_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform enemypatrol_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform trees_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform rocks_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform ngondut_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform animal_Perrent;
    [Foldout("Parrent")]
    [SerializeField] private Transform goldmine_Perrent;
    

    #endregion

    #region Private Fields

    // Map info
    private int mapWidth;
    private int mapHeight;
    //private const int TILE_SCALE = 4;
    
    // Terrain layers
    private TerrainLayer groundLayer;
    private TerrainLayer grassLayer;
    
    // Edge collections (organized by direction)
    private EdgeCollections groundEdges;
    private EdgeCollections grassEdges;
    private List<Vector2Int> allEdgePositions;
    
    // Spawned object tracking
    private SpawnedObjectTracker spawnTracker;

    #endregion

    #region Unity Lifecycle

    void Start()
    {
        StartCoroutine(SpawnAllPrefabs());
    }

    private bool canDrawn = false;
    void OnDrawGizmos()
    {
        if (!showDebugGizmos || !Application.isPlaying) return;
        if (!canDrawn) return;
        DrawMapBoundsGizmos();
        DrawSpawnedObjectsGizmos();
        
        if (drawEdgeGizmos)
            DrawEdgeGizmos();
    }

    #endregion

    #region Initialization

    IEnumerator SpawnAllPrefabs()
    {
        yield return new WaitForSeconds(0.6f);

        if (!InitializeSystem())
        {
            Debug.LogError("[PrefabSpawner] Failed to initialize system!");
            yield break;
        }

        LogInfo("=== Starting Prefab Spawning ===");

        // Spawn in priority order
        SpawnCastles();
        yield return null;

        SpawnEnemyStructures();
        yield return null;

        SpawnResources();
        yield return null;

        SpawnNature();
        yield return null;

        SpawnAnimals();
        yield return null;

        SpawnDecorations();
        yield return null;

        LogInfo($"=== Prefab Spawning Complete === Total Objects: {spawnTracker.TotalSpawnedCount}");
        canDrawn = true;
        AstarPath.active.Scan();

        GameManager.Instance.renderMap();

        if (LoadingSceneController.Instance != null)
        {
            LoadingSceneController.Instance.NotifySpawnComplete();
        }
    }

    bool InitializeSystem()
    {
        // Validate references
        if (!ValidateReferences())
            return false;

        // Initialize components
        groundLayer = GetTerrainLayerFromTilemap(groundTilemap);
        if (groundLayer == null)
        {
            Debug.LogError("[PrefabSpawner] Ground layer not found!");
            return false;
        }

        if (grassTilemap != null)
            grassLayer = GetTerrainLayerFromTilemap(grassTilemap);

        // Initialize data structures
        mapWidth = groundLayer.layerMap.GetLength(0);
        mapHeight = groundLayer.layerMap.GetLength(1);
        
        groundEdges = new EdgeCollections();
        grassEdges = new EdgeCollections();
        allEdgePositions = new List<Vector2Int>();
        
        spawnTracker = new SpawnedObjectTracker();

        // Collect edges from border generator
        CollectEdgesFromBorderGenerator();

        LogSystemInfo();
        
        return true;
    }

    bool ValidateReferences()
    {
        if (groundTilemap == null)
        {
            Debug.LogError("[PrefabSpawner] Ground Tilemap not assigned!");
            return false;
        }

        if (terrainGenerator == null)
        {
            Debug.LogError("[PrefabSpawner] Terrain Generator not assigned!");
            return false;
        }

        return true;
    }

    void CollectEdgesFromBorderGenerator()
    {
        if (borderGenerator == null || borderGenerator.borderLayers == null)
        {
            LogWarning("Border Generator not found, calculating edges manually...");
            CalculateEdgesManually();
            return;
        }

        // Extract edges from border generator
        foreach (var borderLayer in borderGenerator.borderLayers)
        {
            if (borderLayer.layer == groundLayer)
            {
                groundEdges.AddEdges(borderLayer);
            }
            else if (grassLayer != null && borderLayer.layer == grassLayer)
            {
                grassEdges.AddEdges(borderLayer);
            }
        }

        // Remove duplicates and combine
        groundEdges.RemoveDuplicates();
        grassEdges.RemoveDuplicates();
        
        allEdgePositions.AddRange(groundEdges.GetAllEdges());
        allEdgePositions.AddRange(grassEdges.GetAllEdges());
        allEdgePositions = new List<Vector2Int>(new HashSet<Vector2Int>(allEdgePositions));
    }

    void CalculateEdgesManually()
    {
        // Calculate ground edges
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (groundLayer.layerMap[x, y])
                {
                    DetectAndAddEdges(groundLayer.layerMap, x, y, groundEdges);
                }
            }
        }

        // Calculate grass edges
        if (grassLayer != null)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (grassLayer.layerMap[x, y])
                    {
                        DetectAndAddEdges(grassLayer.layerMap, x, y, grassEdges);
                    }
                }
            }
        }

        // Combine all edges
        allEdgePositions.AddRange(groundEdges.GetAllEdges());
        allEdgePositions.AddRange(grassEdges.GetAllEdges());
        allEdgePositions = new List<Vector2Int>(new HashSet<Vector2Int>(allEdgePositions));
    }

    void DetectAndAddEdges(bool[,] map, int x, int y, EdgeCollections edges)
    {
        Vector2Int pos = new Vector2Int(x, y);

        if (y == mapHeight - 1 || !map[x, y + 1])
            edges.topEdges.Add(pos);
        if (y == 0 || !map[x, y - 1])
            edges.bottomEdges.Add(pos);
        if (x == 0 || !map[x - 1, y])
            edges.leftEdges.Add(pos);
        if (x == mapWidth - 1 || !map[x + 1, y])
            edges.rightEdges.Add(pos);
    }

    TerrainLayer GetTerrainLayerFromTilemap(Tilemap targetTilemap)
    {
        if (targetTilemap == null) return null;

        var layersField = typeof(LayeredTerrainGenerator).GetField("layers", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (layersField != null)
        {
            var layers = layersField.GetValue(terrainGenerator) as System.Collections.IList;
            if (layers != null)
            {
                foreach (var layer in layers)
                {
                    var terrainLayer = layer as TerrainLayer;
                    if (terrainLayer != null && terrainLayer.tilemap == targetTilemap)
                        return terrainLayer;
                }
            }
        }

        return null;
    }

    #endregion

    #region Castle Spawning

    void SpawnCastles()
    {
        if (playerCastle == null || enemyCastle == null)
        {
            LogWarning("Castle prefabs not assigned!");
            return;
        }

        float mapDiagonal = Mathf.Sqrt(mapWidth * mapWidth + mapHeight * mapHeight);
        float minDistance = mapDiagonal * minCastleDistancePercent;

        // Spawn Player Castle
        Vector2Int playerGridPos = Vector2Int.zero;

        for (int i = 0; i < 20; i++)
        {
            playerGridPos = FindSafeStructurePosition(20f);
            if (playerGridPos != Vector2Int.zero)
                break;
        }
        if (playerGridPos != Vector2Int.zero)
        {
            Vector3 worldPos = GridToWorld(playerGridPos);
            Instantiate(playerCastle, worldPos, Quaternion.identity, player_Perrent);
            spawnTracker.SetPlayerCastle(worldPos);
            
            LogInfo($"Player Castle spawned at grid {playerGridPos}");
        }
        else
        {
            Debug.LogError("[PrefabSpawner] Failed to spawn Player Castle!");
            return;
        }

        // Spawn Enemy Castle (maximize distance)
        Vector2Int enemyGridPos = FindBestEnemyCastlePosition(playerGridPos, minDistance);
        if (enemyGridPos != Vector2Int.zero)
        {
            Vector3 worldPos = GridToWorld(enemyGridPos);
            Instantiate(enemyCastle, worldPos, Quaternion.identity, enemybuilding_Perrent);
            spawnTracker.SetEnemyCastle(worldPos);
            
            float distance = GridDistance(playerGridPos, enemyGridPos);
            LogInfo($"Enemy Castle spawned at grid {enemyGridPos}, distance: {distance:F1} units");
        }
        else
        {
            Debug.LogError("[PrefabSpawner] Failed to spawn Enemy Castle!");
        }
    }

    Vector2Int FindBestEnemyCastlePosition(Vector2Int playerPos, float minDistance)
    {
        float bestDistance = 0f;
        Vector2Int bestPosition = Vector2Int.zero;

        for (int attempt = 0; attempt < maxCastleAttempts; attempt++)
        {
            Vector2Int candidatePos = FindSafeStructurePosition(20f);
            if (candidatePos == Vector2Int.zero) continue;

            float distance = GridDistance(playerPos, candidatePos);
            
            if (distance > bestDistance)
            {
                bestDistance = distance;
                bestPosition = candidatePos;
                
                if (distance >= minDistance)
                    break;
            }
        }

        return bestPosition;
    }

    #endregion

    #region Enemy Structures

    void SpawnEnemyStructures()
    {
        SpawnLuaTrai();
        SpawnEnemyStorages();
        SpawnEnemyTowers();
    }

    void SpawnLuaTrai()
    {
        if (luaTrai == null) return;

        int count = Random.Range(luaTraiSettings.minCount, luaTraiSettings.maxCount + 1);
        int spawned = 0;
        
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = FindPositionWithPlayerProtection(
                luaTraiSettings.minDistance,
                luaTraiSettings.maxDistance,
                spawnTracker.luaTraiPositions,
                luaTraiMinDistanceFromPlayer
            );

            if (pos != Vector3.zero)
            {
                Instantiate(luaTrai, pos, Quaternion.identity, enemypatrol_Perrent);
                spawnTracker.AddLuaTrai(pos);
                spawned++;
            }
        }

        LogInfo($"Spawned {spawned} Lửa Trại");
    }

    void SpawnEnemyStorages()
    {
        if (enemyStorage == null) return;

        int count = Random.Range(storageSettings.minCount, storageSettings.maxCount + 1);
        int spawned = 0;
        
        // Target area: 65% toward enemy castle
        Vector3 targetArea = Vector3.Lerp(
            spawnTracker.playerCastlePos,
            spawnTracker.enemyCastlePos,
            0.65f
        );

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = FindPositionNearWithPlayerProtection(
                targetArea,
                storageSettings.minDistance,
                storageSettings.maxDistance,
                10f,
                storageMinDistanceFromPlayer
            );

            if (pos != Vector3.zero)
            {
                Instantiate(enemyStorage, pos, Quaternion.identity, enemybuilding_Perrent);
                spawnTracker.AddStorage(pos);
                spawned++;
            }
        }

        LogInfo($"Spawned {spawned} Enemy Storages");
    }

    void SpawnEnemyTowers()
    {
        if (enemyTower == null) return;

        List<Vector3> towerBasePositions = new List<Vector3>();
        towerBasePositions.AddRange(spawnTracker.luaTraiPositions);
        towerBasePositions.AddRange(spawnTracker.storagePositions);
        towerBasePositions.Add(spawnTracker.enemyCastlePos);

        int spawned = 0;

        foreach (Vector3 basePos in towerBasePositions)
        {
            if (Random.value <= towerSpawnChance)
            {
                Vector3 towerPos = FindPositionNearStructure(basePos, 10f, towerSpawnRadius, 5f);
                
                if (towerPos != Vector3.zero)
                {
                    Instantiate(enemyTower, towerPos, Quaternion.identity, enemybuilding_Perrent);
                    spawnTracker.AddSpawnedPosition(towerPos);
                    spawned++;
                }
            }
        }

        LogInfo($"Spawned {spawned} Enemy Towers");
    }

    #endregion

    #region Resources

    void SpawnResources()
    {
        SpawnGoldMines();
    }

    void SpawnGoldMines()
    {
        if (goldMine == null) return;

        int count = Random.Range(goldMineSettings.minCount, goldMineSettings.maxCount + 1);
        int spawned = 0;

        // First mine near player
        Vector3 nearPlayerPos = FindPositionNearStructure(
            spawnTracker.playerCastlePos,
            20f,
            goldMineNearPlayerRadius,
            10f
        );
        
        if (nearPlayerPos != Vector3.zero)
        {
            Instantiate(goldMine, nearPlayerPos, Quaternion.identity, goldmine_Perrent);
            spawnTracker.AddGoldMine(nearPlayerPos);
            spawned++;
            count--;
        }

        // Remaining mines
        for (int i = 0; i < count; i++)
        {
            // Vector3 pos = FindPositionWithDistanceConstraint(
            //     goldMineSettings.minDistance,
            //     goldMineSettings.maxDistance,
            //     spawnTracker.goldMinePositions
            // );

            Vector3 pos = FindPositionWithDistanceConstraintAndEdgeCheck(
                goldMineSettings.minDistance,
                goldMineSettings.maxDistance,
                spawnTracker.goldMinePositions,
                addminDistFromMapEdge
            );

            if (pos != Vector3.zero)
            {
                Instantiate(goldMine, pos, Quaternion.identity, goldmine_Perrent);
                spawnTracker.AddGoldMine(pos);
                spawned++;
            }
        }

        LogInfo($"Spawned {spawned} Gold Mines");
    }

    #endregion

    #region Nature

    void SpawnNature()
    {
        SpawnTrees();
        SpawnRocks();
    }

    void SpawnTrees()
    {
        if (trees.Count == 0) return;

        List<Vector3> treePositions = new List<Vector3>();
        
        int edgeSpawned = SpawnTreesAlongEdges(treePositions);
        int randomSpawned = SpawnTreesRandom(treePositions);

        LogInfo($"Spawned {edgeSpawned + randomSpawned} Trees (Edge: {edgeSpawned}, Random: {randomSpawned})");
    }

    int SpawnTreesAlongEdges(List<Vector3> existingTrees)
    {
        if (allEdgePositions.Count == 0)
        {
            LogWarning("No edges found for tree spawning!");
            return 0;
        }

        int spawned = 0;
        int attempts = 0;
        int maxAttempts = treeEdgeCount * 5;

        // Shuffle for randomness
        List<Vector2Int> shuffledEdges = new List<Vector2Int>(allEdgePositions);
        ShuffleList(shuffledEdges);

        while (spawned < treeEdgeCount && attempts < maxAttempts)
        {
            attempts++;

            Vector2Int edgePos = shuffledEdges[Random.Range(0, shuffledEdges.Count)];
            Vector2Int spawnPos = GetInwardPositionFromEdge(edgePos);

            if (IsValidTreePosition(spawnPos, existingTrees) 
                && tilemapHelper != null 
                && !tilemapHelper.IsPointOverlappingTile(GridToWorld(spawnPos), 1f))
            {
                Vector3 worldPos = GridToWorld(spawnPos);
                GameObject treePrefab = trees[Random.Range(0, trees.Count)];
                Instantiate(treePrefab, worldPos, Quaternion.identity, trees_Perrent);
                existingTrees.Add(worldPos);
                spawned++;
            }
        }

        return spawned;
    }

    int SpawnTreesRandom(List<Vector3> existingTrees)
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = treeRandomCount * 5;

        while (spawned < treeRandomCount && attempts < maxAttempts)
        {
            attempts++;

            Vector2Int gridPos = new Vector2Int(
                Random.Range(0, mapWidth),
                Random.Range(0, mapHeight)
            );

            if (IsValidTreePosition(gridPos, existingTrees) 
                && tilemapHelper != null 
                && !tilemapHelper.IsPointOverlappingTile(GridToWorld(gridPos), 1f))
            {
                Vector3 worldPos = GridToWorld(gridPos);
                GameObject treePrefab = trees[Random.Range(0, trees.Count)];
                Instantiate(treePrefab, worldPos, Quaternion.identity, trees_Perrent);
                existingTrees.Add(worldPos);
                spawned++;
            }
        }

        return spawned;
    }

    Vector2Int GetInwardPositionFromEdge(Vector2Int edgePos)
    {
        List<Vector2Int> candidates = new List<Vector2Int>();

        // Check 8 directions
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = edgePos.x + dx;
                int checkY = edgePos.y + dy;

                if (IsInBounds(checkX, checkY) && HasTileAt(checkX, checkY))
                {
                    float offset = Random.Range(0f, edgeDepth);
                    int finalX = Mathf.Clamp(
                        edgePos.x + Mathf.RoundToInt(dx * offset),
                        0, mapWidth - 1
                    );
                    int finalY = Mathf.Clamp(
                        edgePos.y + Mathf.RoundToInt(dy * offset),
                        0, mapHeight - 1
                    );

                    candidates.Add(new Vector2Int(finalX, finalY));
                }
            }
        }

        return candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : edgePos;
    }

    bool IsValidTreePosition(Vector2Int gridPos, List<Vector3> existingTrees)
    {
        if (!IsInBounds(gridPos.x, gridPos.y) || !HasTileAt(gridPos.x, gridPos.y))
            return false;

        Vector3 worldPos = GridToWorld(gridPos);

        // Check distance from structures
        if (spawnTracker.IsTooCloseToStructures(worldPos, GridToWorldDistance(5f)))
            return false;

        // Check spacing between trees
        float minSpacing = GridToWorldDistance(treeMinSpacing);
        foreach (Vector3 treePos in existingTrees)
        {
            if (Vector3.Distance(worldPos, treePos) < minSpacing)
                return false;
        }

        return true;
    }

    void SpawnRocks()
    {
        if (rocks.Count == 0) return;

        int spawned = 0;

        for (int i = 0; i < rockCount; i++)
        {
            TerrainLayer layer = (grassLayer != null && Random.value < RockGrassSpawnChance) ? grassLayer : groundLayer;
            Vector2Int gridPos = FindSafeGridPosition(layer, 3f);
            
            if (gridPos != Vector2Int.zero)
            {
                Vector3 worldPos = GridToWorld(gridPos);
                
                if (!spawnTracker.IsTooCloseToStructures(worldPos, GridToWorldDistance(8f)) 
                    && tilemapHelper != null 
                    && !tilemapHelper.IsPointOverlappingTile(worldPos, 3f))
                {
                    GameObject rockPrefab = rocks[Random.Range(0, rocks.Count)];
                    Instantiate(rockPrefab, worldPos, Quaternion.identity, rocks_Perrent);
                    spawned++;
                }
            }
        }

        LogInfo($"Spawned {spawned} Rocks");
    }

    #endregion

    #region Animals

    void SpawnAnimals()
    {
        SpawnAnimalType(animalBear, "Bear");
        SpawnAnimalType(animalSpider, "Spider");
        SpawnAnimalType(animalSheep, "Sheep");
        SpawnAnimalType(animalSnake, "Snake");
    }

    void SpawnAnimalType(GameObject prefab, string animalName)
    {
        if (prefab == null) return;

        int count = Random.Range(animalSettings.minCount, animalSettings.maxCount + 1);
        int spawned = 0;

        for (int i = 0; i < count; i++)
        {
            TerrainLayer spawnLayer = SelectAnimalSpawnLayer();
            Vector2Int gridPos = FindSafeGridPosition(spawnLayer, 5f);
            
            if (gridPos != Vector2Int.zero)
            {
                Vector3 worldPos = GridToWorld(gridPos);
                
                if (!spawnTracker.IsTooCloseToStructures(worldPos, GridToWorldDistance(15f)))
                {
                    Instantiate(prefab, worldPos, Quaternion.identity, animal_Perrent);
                    spawned++;
                }
            }
        }

        LogInfo($"Spawned {spawned} {animalName}");
    }

    TerrainLayer SelectAnimalSpawnLayer()
    {
        if (grassLayer == null)
            return groundLayer;

        return Random.value < animalGrassSpawnChance ? grassLayer : groundLayer;
    }

    #endregion

    #region Decorations

    void SpawnDecorations()
    {
        int totalSpawned = 0;

        // Spawn Ngon Dut
        if (ngonDut != null)
        {
            int ngonDutCount = Random.Range(ngonDutSettings.minCount, ngonDutSettings.maxCount + 1);
            int spawned = 0;

            for (int i = 0; i < ngonDutCount; i++)
            {
                Vector3 pos = FindPositionWithDistanceConstraint(
                    ngonDutSettings.minDistance,
                    ngonDutSettings.maxDistance,
                    spawnTracker.ngonDutPositions
                );

                if (pos != Vector3.zero
                    && tilemapHelper != null
                    && !tilemapHelper.IsPointOverlappingTile(pos, 2f))
                {
                    Instantiate(ngonDut, pos, Quaternion.identity, ngondut_Perrent);
                    spawnTracker.AddNgonDut(pos);
                    spawned++;
                }
            }

            LogInfo($"Spawned {spawned} Ngón Đuốc");
            totalSpawned += spawned;
        }

        // Spawn Other Decorations
        if (decorationPrefabs.Count > 0 && count > 0)
        {
            int spawned = 0;

            for (int i = 0; i < count; i++)
            {
                // Chọn layer ngẫu nhiên (ưu tiên grass nếu có)
                TerrainLayer spawnLayer = (grassLayer != null && Random.value < decorationGrassSpawnChance)
                    ? grassLayer
                    : groundLayer;

                Vector2Int gridPos = FindSafeGridPosition(spawnLayer, 2f);

                if (gridPos != Vector2Int.zero)
                {
                    Vector3 worldPos = GridToWorld(gridPos);

                    // Check không trùng với structures và tilemap
                    if (!spawnTracker.IsTooCloseToStructures(worldPos, GridToWorldDistance(5f))
                        && tilemapHelper != null
                        && !tilemapHelper.IsPointOverlappingTile(worldPos, 1f))
                    {
                        GameObject decorPrefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];
                        Instantiate(decorPrefab, worldPos, Quaternion.identity, ngondut_Perrent);
                        spawnTracker.AddSpawnedPosition(worldPos);
                        spawned++;
                    }
                }
            }

            LogInfo($"Spawned {spawned} Decorations");
            totalSpawned += spawned;
        }

        LogInfo($"Total Decorations Spawned: {totalSpawned}");
    }

    #endregion

    #region Position Finding Utilities

    Vector2Int FindSafeStructurePosition(float clearRadius)
    {
        int attempts = 0;
        const int MAX_ATTEMPTS = 100;

        while (attempts < MAX_ATTEMPTS)
        {
            Vector2Int gridPos = new Vector2Int(
                Random.Range(10, mapWidth - 10),
                Random.Range(10, mapHeight - 10)
            );

            if (groundLayer.layerMap[gridPos.x, gridPos.y])
            {
                if (IsPositionFarFromEdges(gridPos, minDistanceFromMapEdge))
                {
                    Vector3 worldPos = GridToWorld(gridPos);
                    if (spawnTracker.IsAreaClear(worldPos, GridToWorldDistance(clearRadius)))
                    {
                        return gridPos;
                    }
                }
            }

            attempts++;
        }

        return Vector2Int.zero;
    }

    Vector2Int FindSafeGridPosition(TerrainLayer layer, float clearRadius)
    {
        if (layer == null || layer.layerMap == null)
            return Vector2Int.zero;

        int attempts = 0;
        const int MAX_ATTEMPTS = 100;

        while (attempts < MAX_ATTEMPTS)
        {
            Vector2Int gridPos = new Vector2Int(
                Random.Range(10, mapWidth - 10),
                Random.Range(10, mapHeight - 10)
            );

            if (layer.layerMap[gridPos.x, gridPos.y])
            {
                Vector3 worldPos = GridToWorld(gridPos);
                if (spawnTracker.IsAreaClear(worldPos, GridToWorldDistance(clearRadius)))
                {
                    return gridPos;
                }
            }

            attempts++;
        }

        return Vector2Int.zero;
    }

    Vector3 FindPositionNearStructure(Vector3 center, float minDist, float maxDist, float clearRadius, float addminDistanceFromMapEdge = 0f)
    {
        int attempts = 0;
        const int MAX_ATTEMPTS = 50;

        while (attempts < MAX_ATTEMPTS)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDist, maxDist);
            Vector3 targetPos = center + new Vector3(randomDir.x, randomDir.y, 0) * distance;

            Vector2Int gridPos = WorldToGrid(targetPos);
            
            if (IsPositionFarFromEdges(gridPos, minDistanceFromMapEdge + addminDistanceFromMapEdge) &&
                IsValidGridPosition(gridPos))
            {
                Vector3 worldPos = GridToWorld(gridPos);
                if (spawnTracker.IsAreaClear(worldPos, GridToWorldDistance(clearRadius)))
                {
                    return worldPos;
                }
            }

            attempts++;
        }

        return Vector3.zero;
    }

    Vector3 FindPositionNearWithPlayerProtection(
        Vector3 center,
        float minDist,
        float maxDist,
        float clearRadius,
        float minDistFromPlayer)
    {
        int attempts = 0;
        const int MAX_ATTEMPTS = 100;

        while (attempts < MAX_ATTEMPTS)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDist, maxDist);
            Vector3 targetPos = center + new Vector3(randomDir.x, randomDir.y, 0) * distance;

            Vector2Int gridPos = WorldToGrid(targetPos);
            
            if (!IsPositionFarFromEdges(gridPos, minDistanceFromMapEdge))
            {
                attempts++;
                continue;
            }

            if (!IsValidGridPosition(gridPos))
            {
                attempts++;
                continue;
            }

            // Check distance from player castle
            Vector2Int playerGridPos = WorldToGrid(spawnTracker.playerCastlePos);
            float distFromPlayer = GridDistance(gridPos, playerGridPos);
            
            if (distFromPlayer >= minDistFromPlayer)
            {
                Vector3 worldPos = GridToWorld(gridPos);
                if (spawnTracker.IsAreaClear(worldPos, GridToWorldDistance(clearRadius)))
                {
                    return worldPos;
                }
            }

            attempts++;
        }

        return Vector3.zero;
    }

    Vector3 FindPositionWithPlayerProtection(
        float minDist,
        float maxDist,
        List<Vector3> existingPositions,
        float minDistFromPlayer)
    {
        int attempts = 0;
        const int MAX_ATTEMPTS = 300;

        while (attempts < MAX_ATTEMPTS)
        {
            Vector2Int gridPos = FindSafeStructurePosition(10f);

            if (gridPos == Vector2Int.zero)
            {
                attempts++;
                continue;
            }

            // Check distance from player castle
            Vector2Int playerGridPos = WorldToGrid(spawnTracker.playerCastlePos);
            float distFromPlayer = GridDistance(gridPos, playerGridPos);

            if (distFromPlayer < minDistFromPlayer)
            {
                attempts++;
                continue;
            }

            // Check distance constraints with existing positions
            bool validDistance = true;
            foreach (Vector3 existing in existingPositions)
            {
                Vector2Int existingGrid = WorldToGrid(existing);
                float dist = GridDistance(gridPos, existingGrid);

                if (dist < minDist || dist > maxDist)
                {
                    validDistance = false;
                    break;
                }
            }

            if (validDistance || existingPositions.Count == 0)
            {
                return GridToWorld(gridPos);
            }

            attempts++;
        }

        return Vector3.zero;
    }

    Vector3 FindPositionWithDistanceConstraintAndEdgeCheck(
    float minDist,          // Khoảng cách tối thiểu giữa các gold mine
    float maxDist,          // Khoảng cách tối đa giữa các gold mine
    List<Vector3> existingPositions,  // Danh sách gold mine đã spawn
    float minDistFromEdge)  // Khoảng cách tối thiểu từ rìa map
    {
        int attempts = 0;
        const int MAX_ATTEMPTS = 200;

        while (attempts < MAX_ATTEMPTS)
        {
            attempts++;

            // BƯỚC 1: Tìm vị trí an toàn (clear radius 10 units)
            Vector2Int gridPos = FindSafeStructurePosition(10f);
            if (gridPos == Vector2Int.zero)
                continue;  // Không tìm được → thử lại

            // BƯỚC 2: Kiểm tra XA RÌA MAP
            if (!IsPositionFarFromEdges(gridPos, minDistFromEdge))
                continue;  // Quá gần rìa → thử lại

            // BƯỚC 3: Kiểm tra khoảng cách với gold mine đã có
            bool validDistance = true;

            foreach (Vector3 existing in existingPositions)
            {
                Vector2Int existingGrid = WorldToGrid(existing);
                float dist = GridDistance(gridPos, existingGrid);

                // Phải nằm trong khoảng minDist → maxDist
                if (dist < minDist || dist > maxDist)
                {
                    validDistance = false;
                    break;
                }
            }

            // BƯỚC 4: Return nếu hợp lệ
            if (validDistance || existingPositions.Count == 0)
            {
                return GridToWorld(gridPos);
            }
        }

        // Thử 200 lần mà không tìm được
        return Vector3.zero;
    }

    Vector3 FindPositionWithDistanceConstraint(
        float minDist,
        float maxDist,
        List<Vector3> existingPositions)
    {
        int attempts = 0;
        const int MAX_ATTEMPTS = 100;

        while (attempts < MAX_ATTEMPTS)
        {
            Vector2Int gridPos = FindSafeGridPosition(groundLayer, 10f);
            
            if (gridPos == Vector2Int.zero)
            {
                attempts++;
                continue;
            }

            bool validDistance = true;
            foreach (Vector3 existing in existingPositions)
            {
                Vector2Int existingGrid = WorldToGrid(existing);
                float dist = GridDistance(gridPos, existingGrid);
                
                if (dist < minDist || dist > maxDist)
                {
                    validDistance = false;
                    break;
                }
            }

            if (validDistance || existingPositions.Count == 0)
            {
                return GridToWorld(gridPos);
            }

            attempts++;
        }

        return Vector3.zero;
    }

    bool IsPositionFarFromEdges(Vector2Int pos, float minDistance)
    {
        return IsPositionFarFromEdgeList(pos, groundEdges, minDistance) &&
               IsPositionFarFromEdgeList(pos, grassEdges, minDistance);
    }

    bool IsPositionFarFromEdgeList(Vector2Int pos, EdgeCollections edges, float minDistance)
    {
        foreach (var edge in edges.GetAllEdges())
        {
            if (Vector2Int.Distance(pos, edge) < minDistance)
                return false;
        }
        return true;
    }

    #endregion

    #region Coordinate System Utilities

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        if (groundTilemap == null) return Vector3.zero;
        
        Vector3Int cellPos = new Vector3Int(gridPos.x, gridPos.y, 0);
        Vector3 worldPos = groundTilemap.CellToWorld(cellPos);
        
        worldPos.x += groundTilemap.cellSize.x * 0.5f;
        worldPos.y += groundTilemap.cellSize.y * 0.5f;
        
        return worldPos;
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        if (groundTilemap == null) return Vector2Int.zero;
        
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }

    float GridDistance(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b);
    }

    float GridToWorldDistance(float gridDistance)
    {
        if (groundTilemap == null) return gridDistance;
        return gridDistance * groundTilemap.cellSize.x;
    }

    bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
    }

    bool HasTileAt(int x, int y)
    {
        bool hasGround = groundLayer.layerMap[x, y];
        bool hasGrass = grassLayer != null && grassLayer.layerMap[x, y];
        return hasGround || hasGrass;
    }

    bool IsValidGridPosition(Vector2Int gridPos)
    {
        if (!IsInBounds(gridPos.x, gridPos.y))
            return false;

        return groundLayer.layerMap[gridPos.x, gridPos.y];
    }

    #endregion

    #region Utility Methods

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    #endregion

    #region Logging

    void LogSystemInfo()
    {
        if (!showDebugLogs) return;

        Debug.Log($"[PrefabSpawner] === SYSTEM INITIALIZED ===\n" +
                  $"Map Size: {mapWidth} x {mapHeight}\n" +
                  $"Ground Edges: {groundEdges.TotalCount}\n" +
                  $"Grass Edges: {grassEdges.TotalCount}\n" +
                  $"Total Edge Positions: {allEdgePositions.Count}\n" +
                  $"Min Distance From Edge: {minDistanceFromMapEdge}");
    }

    void LogInfo(string message)
    {
        if (showDebugLogs)
            Debug.Log($"[PrefabSpawner] {message}");
    }

    void LogWarning(string message)
    {
        Debug.LogWarning($"[PrefabSpawner] {message}");
    }

    #endregion

    #region Debug Gizmos

    void DrawMapBoundsGizmos()
    {
        if (groundTilemap == null || allEdgePositions.Count == 0) return;

        float cellSize = groundTilemap.cellSize.x;

        // Calculate bounds from edges
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var edge in allEdgePositions)
        {
            if (edge.x < minX) minX = edge.x;
            if (edge.x > maxX) maxX = edge.x;
            if (edge.y < minY) minY = edge.y;
            if (edge.y > maxY) maxY = edge.y;
        }

        // Draw map boundary
        Vector3 minPos = GridToWorld(new Vector2Int(minX, minY));
        Vector3 maxPos = GridToWorld(new Vector2Int(maxX + 1, maxY + 1));
        
        minPos.x -= cellSize * 0.5f;
        minPos.y -= cellSize * 0.5f;
        maxPos.x += cellSize * 0.5f;
        maxPos.y += cellSize * 0.5f;

        Gizmos.color = Color.cyan;
        DrawRectangle(minPos, maxPos);

        // Draw safe zone indicator
        if (drawProtectionZones)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            float margin = GridToWorldDistance(minDistanceFromMapEdge);
            
            Vector3 safeMin = new Vector3(minPos.x + margin, minPos.y + margin, 0);
            Vector3 safeMax = new Vector3(maxPos.x - margin, maxPos.y - margin, 0);
            DrawRectangle(safeMin, safeMax);
        }

        #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.Label(
            new Vector3(minPos.x, maxPos.y + 10, 0),
            $"Map Bounds | Edge Margin: {minDistanceFromMapEdge} units"
        );
        #endif
    }

    void DrawEdgeGizmos()
    {
        if (groundTilemap == null) return;
        
        float cellSize = groundTilemap.cellSize.x;
        float groundSize = cellSize * 0.3f;
        float grassSize = cellSize * 0.2f;

        // Ground edges with directional colors
        DrawEdgeList(groundEdges.topEdges, new Color(1f, 0f, 0f, 0.5f), groundSize);      // Red - Top
        DrawEdgeList(groundEdges.bottomEdges, new Color(0f, 0f, 1f, 0.5f), groundSize);   // Blue - Bottom
        DrawEdgeList(groundEdges.leftEdges, new Color(1f, 1f, 0f, 0.5f), groundSize);     // Yellow - Left
        DrawEdgeList(groundEdges.rightEdges, new Color(1f, 0f, 1f, 0.5f), groundSize);    // Magenta - Right

        // Grass edges (lighter colors)
        if (grassLayer != null)
        {
            DrawEdgeList(grassEdges.topEdges, new Color(1f, 0.5f, 0.5f, 0.4f), grassSize);
            DrawEdgeList(grassEdges.bottomEdges, new Color(0.5f, 0.5f, 1f, 0.4f), grassSize);
            DrawEdgeList(grassEdges.leftEdges, new Color(1f, 1f, 0.5f, 0.4f), grassSize);
            DrawEdgeList(grassEdges.rightEdges, new Color(1f, 0.5f, 1f, 0.4f), grassSize);
        }

        #if UNITY_EDITOR
        Vector3 infoPos = GridToWorld(Vector2Int.zero) + new Vector3(-35, 25, 0);
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(infoPos,
            $"=== EDGES ===\n" +
            $"Ground: T:{groundEdges.topEdges.Count} B:{groundEdges.bottomEdges.Count} " +
            $"L:{groundEdges.leftEdges.Count} R:{groundEdges.rightEdges.Count}\n" +
            $"Grass: T:{grassEdges.topEdges.Count} B:{grassEdges.bottomEdges.Count} " +
            $"L:{grassEdges.leftEdges.Count} R:{grassEdges.rightEdges.Count}"
        );
        #endif
    }

    void DrawEdgeList(List<Vector2Int> edges, Color color, float size)
    {
        Gizmos.color = color;
        foreach (var edge in edges)
        {
            Vector3 worldPos = GridToWorld(edge);
            Gizmos.DrawWireSphere(worldPos, size);
        }
    }

    void DrawSpawnedObjectsGizmos()
    {
        if (groundTilemap == null) return;
        
        float cellSize = groundTilemap.cellSize.x;

        // Player Castle
        if (spawnTracker != null && spawnTracker.playerCastlePos != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spawnTracker.playerCastlePos, 5f * cellSize);
            
            #if UNITY_EDITOR
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.Label(
                spawnTracker.playerCastlePos + Vector3.up * 12f * cellSize,
                "Player Castle"
            );
            #endif

            // Protection zones
            if (drawProtectionZones)
            {
                Gizmos.color = new Color(0f, 0f, 1f, 0.1f);
                Gizmos.DrawWireSphere(
                    spawnTracker.playerCastlePos,
                    GridToWorldDistance(luaTraiMinDistanceFromPlayer)
                );
                
                Gizmos.color = new Color(0f, 0.5f, 1f, 0.1f);
                Gizmos.DrawWireSphere(
                    spawnTracker.playerCastlePos,
                    GridToWorldDistance(storageMinDistanceFromPlayer)
                );
            }
        }

        // Enemy Castle
        if (spawnTracker.enemyCastlePos != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnTracker.enemyCastlePos, 5f * cellSize);
            
            #if UNITY_EDITOR
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.Label(
                spawnTracker.enemyCastlePos + Vector3.up * 12f * cellSize,
                "Enemy Castle"
            );
            #endif
        }

        // Castle distance line
        if (spawnTracker.playerCastlePos != Vector3.zero && spawnTracker.enemyCastlePos != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(spawnTracker.playerCastlePos, spawnTracker.enemyCastlePos);
            
            #if UNITY_EDITOR
            Vector3 midPoint = (spawnTracker.playerCastlePos + spawnTracker.enemyCastlePos) * 0.5f;
            float distance = GridDistance(
                WorldToGrid(spawnTracker.playerCastlePos),
                WorldToGrid(spawnTracker.enemyCastlePos)
            );
            
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.Label(midPoint, $"Distance: {distance:F1} units");
            #endif
        }

        // Lua Trai
        DrawObjectList(spawnTracker.luaTraiPositions, new Color(1f, 0.5f, 0f), 3f * cellSize);

        // Storages
        DrawObjectList(spawnTracker.storagePositions, Color.magenta, 4f * cellSize);

        // Gold Mines
        DrawObjectList(spawnTracker.goldMinePositions, Color.green, 3f * cellSize);

        // Ngon Dut
        if (drawDecorationGizmos)
        {
            DrawObjectList(spawnTracker.ngonDutPositions, new Color(1f, 1f, 0f, 0.6f), 2f * cellSize);
        }

        // Info panel
        DrawInfoPanel();
    }

    void DrawObjectList(List<Vector3> positions, Color color, float size)
    {
        Gizmos.color = color;
        foreach (Vector3 pos in positions)
        {
            Gizmos.DrawWireSphere(pos, size);
        }
    }

    void DrawRectangle(Vector3 min, Vector3 max)
    {
        Gizmos.DrawLine(new Vector3(min.x, min.y, 0), new Vector3(max.x, min.y, 0));
        Gizmos.DrawLine(new Vector3(max.x, min.y, 0), new Vector3(max.x, max.y, 0));
        Gizmos.DrawLine(new Vector3(max.x, max.y, 0), new Vector3(min.x, max.y, 0));
        Gizmos.DrawLine(new Vector3(min.x, max.y, 0), new Vector3(min.x, min.y, 0));
    }

    void DrawInfoPanel()
    {
        #if UNITY_EDITOR
        if (groundLayer == null) return;

        Vector3 infoPos = GridToWorld(Vector2Int.zero) + new Vector3(-25, 5, 0);
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(infoPos,
            $"=== SPAWNED ===\n" +
            $"Castles: 2\n" +
            $"Lửa Trại: {spawnTracker.luaTraiPositions.Count}\n" +
            $"Storages: {spawnTracker.storagePositions.Count}\n" +
            $"Gold Mines: {spawnTracker.goldMinePositions.Count}\n" +
            $"Ngọn Đuốc: {spawnTracker.ngonDutPositions.Count}\n" +
            $"Total: {spawnTracker.TotalSpawnedCount}"
        );
        #endif
    }

    #endregion
}

#region Helper Classes

/// <summary>
/// Manages collections of edge positions organized by direction
/// </summary>
[System.Serializable]
public class EdgeCollections
{
    public List<Vector2Int> topEdges = new List<Vector2Int>();
    public List<Vector2Int> bottomEdges = new List<Vector2Int>();
    public List<Vector2Int> leftEdges = new List<Vector2Int>();
    public List<Vector2Int> rightEdges = new List<Vector2Int>();

    public int TotalCount => topEdges.Count + bottomEdges.Count + leftEdges.Count + rightEdges.Count;

    public void AddEdges(BorderLayer borderLayer)
    {
        topEdges.AddRange(borderLayer.topEdges);
        bottomEdges.AddRange(borderLayer.bottomEdges);
        leftEdges.AddRange(borderLayer.leftEdges);
        rightEdges.AddRange(borderLayer.rightEdges);
    }

    public void RemoveDuplicates()
    {
        topEdges = new List<Vector2Int>(new HashSet<Vector2Int>(topEdges));
        bottomEdges = new List<Vector2Int>(new HashSet<Vector2Int>(bottomEdges));
        leftEdges = new List<Vector2Int>(new HashSet<Vector2Int>(leftEdges));
        rightEdges = new List<Vector2Int>(new HashSet<Vector2Int>(rightEdges));
    }

    public List<Vector2Int> GetAllEdges()
    {
        List<Vector2Int> all = new List<Vector2Int>();
        all.AddRange(topEdges);
        all.AddRange(bottomEdges);
        all.AddRange(leftEdges);
        all.AddRange(rightEdges);
        return all;
    }
}

/// <summary>
/// Tracks all spawned objects and their positions
/// </summary>
public class SpawnedObjectTracker
{
    // Castle positions
    public Vector3 playerCastlePos = Vector3.zero;
    public Vector3 enemyCastlePos = Vector3.zero;

    // Structure collections
    public List<Vector3> luaTraiPositions = new List<Vector3>();
    public List<Vector3> storagePositions = new List<Vector3>();
    public List<Vector3> goldMinePositions = new List<Vector3>();
    public List<Vector3> ngonDutPositions = new List<Vector3>();
    
    // All spawned positions for collision checking
    private List<Vector3> allSpawnedPositions = new List<Vector3>();

    public int TotalSpawnedCount => allSpawnedPositions.Count;

    public void SetPlayerCastle(Vector3 pos)
    {
        playerCastlePos = pos;
        allSpawnedPositions.Add(pos);
    }

    public void SetEnemyCastle(Vector3 pos)
    {
        enemyCastlePos = pos;
        allSpawnedPositions.Add(pos);
    }

    public void AddLuaTrai(Vector3 pos)
    {
        luaTraiPositions.Add(pos);
        allSpawnedPositions.Add(pos);
    }

    public void AddStorage(Vector3 pos)
    {
        storagePositions.Add(pos);
        allSpawnedPositions.Add(pos);
    }

    public void AddGoldMine(Vector3 pos)
    {
        goldMinePositions.Add(pos);
        allSpawnedPositions.Add(pos);
    }

    public void AddNgonDut(Vector3 pos)
    {
        ngonDutPositions.Add(pos);
        allSpawnedPositions.Add(pos);
    }

    public void AddSpawnedPosition(Vector3 pos)
    {
        allSpawnedPositions.Add(pos);
    }

    public bool IsAreaClear(Vector3 center, float radius)
    {
        foreach (Vector3 pos in allSpawnedPositions)
        {
            if (Vector3.Distance(center, pos) < radius)
                return false;
        }
        return true;
    }

    public bool IsTooCloseToStructures(Vector3 pos, float minDistance)
    {
        if (Vector3.Distance(pos, playerCastlePos) < minDistance ||
            Vector3.Distance(pos, enemyCastlePos) < minDistance)
            return true;

        foreach (Vector3 storage in storagePositions)
        {
            if (Vector3.Distance(pos, storage) < minDistance)
                return true;
        }

        foreach (Vector3 lua in luaTraiPositions)
        {
            if (Vector3.Distance(pos, lua) < minDistance)
                return true;
        }

        return false;
    }
}

/// <summary>
/// Configuration for spawning a type of object
/// </summary>
[System.Serializable]
public class SpawnSettings
{
    [Tooltip("Minimum spawn count")]
    public int minCount;
    
    [Tooltip("Maximum spawn count")]
    public int maxCount;
    
    [Tooltip("Minimum distance between objects (grid units)")]
    public float minDistance;
    
    [Tooltip("Maximum distance between objects (grid units)")]
    public float maxDistance;

    public SpawnSettings(int minCount, int maxCount, float minDistance, float maxDistance)
    {
        this.minCount = minCount;
        this.maxCount = maxCount;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }
}

#endregion