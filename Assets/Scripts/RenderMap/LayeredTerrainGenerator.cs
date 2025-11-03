using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayeredTerrainGenerator : MonoBehaviour
{
    [SerializeField] private ImprovedBorderGenerator Border;

    [Header("Water")]
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private TileBase waterTile;

    [Header("Layer Settings")]
    [SerializeField] private List<TerrainLayer> layers = new List<TerrainLayer>();

    [Header("Map Dimensions")]
    [SerializeField] private int mapWidth = 100;
    [SerializeField] private int mapHeight = 100;

    #region start
    void Start()
    {
        FillArea(waterMap, waterTile, new Vector2Int(-100, -100), new Vector2Int(mapWidth + 100, mapHeight + 100));
        GenerateAllLayers();
    }
    #endregion

    #region Fill Area
    void FillArea(Tilemap map, TileBase tile, Vector2Int start, Vector2Int end)
    {
        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);
        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);

        for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
                map.SetTile(new Vector3Int(x, y, 0), tile);
    }
    #endregion

    #region Generate All Layers
    [ContextMenu("Regenerate All Layers")]
    public void GenerateAllLayers()
    {
        foreach (var layer in layers)
        {
            GenerateLayer(layer);
        }

        if (LoadingSceneController.Instance != null)
        {
            LoadingSceneController.Instance.NotifyTerrainComplete();
        }
    }
    #endregion

    #region Renerate Layer
    void GenerateLayer(TerrainLayer layer)
    {
        if (layer.tilemap == null || layer.tile == null)
        {
            Debug.LogWarning($"Layer {layer.layerName} thiếu tilemap hoặc tile!");
            return;
        }

        layer.layerMap = new bool[mapWidth, mapHeight];
        layer.tilemap.ClearAllTiles();

        if (layer.parentLayer != null)
        {
            GenerateInsideParent(layer);
        }
        else
        {
            GenerateBaseLayer(layer);
        }

        RenderLayer(layer);

        GenerateWallsAndStairs(layer);
    }
    #endregion

    #region Generate Base Layer
    void GenerateBaseLayer(TerrainLayer layer)
    {
        // Tính diện tích mục tiêu dựa trên toàn bộ map
        int totalMapArea = mapWidth * mapHeight;
        int targetArea = Mathf.FloorToInt(totalMapArea * layer.areaPercentage);

        List<Box> boxes = new List<Box>();
        int currentArea = 0;

        // GIAI ĐOẠN 1: Tạo box đầu tiên (lớn)
        Box startBox;
        if (layer.startAtCenter)
        {
            int startSize = Mathf.Max(20, layer.maxBoxSize - 10);
            startBox = new Box(mapWidth / 2 - startSize / 2, mapHeight / 2 - startSize / 2, startSize, startSize);
        }
        else
        {
            int w = Random.Range(layer.maxBoxSize - 10, layer.maxBoxSize);
            int h = Random.Range(layer.maxBoxSize - 10, layer.maxBoxSize);
            int x = Random.Range(0, mapWidth - w);
            int y = Random.Range(0, mapHeight - h);
            startBox = new Box(x, y, w, h);
        }

        boxes.Add(startBox);
        currentArea += FillBoxAndGetArea(layer.layerMap, startBox, mapWidth, mapHeight);

        // GIAI ĐOẠN 2: Tạo box LỚN (0% → 40%)
        Debug.Log($"[{layer.layerName}] Giai đoạn 1: Tạo box LỚN (0% → 40%)");
        int attempts = 0;
        int maxAttempts = 3000;

        while (currentArea < targetArea * 0.4f && attempts < maxAttempts)
        {
            attempts++;
            Box baseBox = boxes[Random.Range(0, boxes.Count)];

            int minSize = Mathf.Max(layer.minBoxSize, (int)(layer.maxBoxSize * 0.8f));
            int maxSize = layer.maxBoxSize;
            Box newBox = CreateConnectedBox(baseBox, minSize, maxSize);

            int addedArea = FillBoxAndGetArea(layer.layerMap, newBox, mapWidth, mapHeight);
            if (addedArea >= 20)
            {
                boxes.Add(newBox);
                currentArea += addedArea;
            }
        }

        // GIAI ĐOẠN 3: Tạo box TRUNG BÌNH (40% → 70%)
        Debug.Log($"[{layer.layerName}] Giai đoạn 2: Tạo box TRUNG BÌNH (40% → 70%)");
        attempts = 0;

        while (currentArea < targetArea * 0.7f && attempts < maxAttempts)
        {
            attempts++;
            Box baseBox = boxes[Random.Range(0, boxes.Count)];

            int minSize = (int)(layer.maxBoxSize * 0.5f);
            int maxSize = (int)(layer.maxBoxSize * 0.8f);
            Box newBox = CreateConnectedBox(baseBox, minSize, maxSize);

            int addedArea = FillBoxAndGetArea(layer.layerMap, newBox, mapWidth, mapHeight);
            if (addedArea >= 10)
            {
                boxes.Add(newBox);
                currentArea += addedArea;
            }
        }

        // GIAI ĐOẠN 4: Tạo box NHỎ (70% → 100%)
        Debug.Log($"[{layer.layerName}] Giai đoạn 3: Tạo box NHỎ (70% → 100%)");
        attempts = 0;

        while (currentArea < targetArea && attempts < maxAttempts)
        {
            attempts++;
            Box baseBox = boxes[Random.Range(0, boxes.Count)];

            int minSize = layer.minBoxSize;
            int maxSize = Mathf.Max(layer.minBoxSize + 5, (int)(layer.maxBoxSize * 0.5f));
            Box newBox = CreateConnectedBox(baseBox, minSize, maxSize);

            int addedArea = FillBoxAndGetArea(layer.layerMap, newBox, mapWidth, mapHeight);
            if (addedArea > 0 && currentArea + addedArea <= targetArea * 1.1f)
            {
                boxes.Add(newBox);
                currentArea += addedArea;
            }
        }

        float coveragePercent = (float)currentArea / totalMapArea * 100f;
        Debug.Log($"<color=green>Layer {layer.layerName} (Base): Hoàn thành! {boxes.Count} boxes | Diện tích: {currentArea}/{totalMapArea} ({coveragePercent:F1}%) | Mục tiêu: {layer.areaPercentage * 100}%</color>");
    }
    #endregion

    #region Generate inside Parent
    void GenerateInsideParent(TerrainLayer layer)
    {
        TerrainLayer parentLayer = GetLayerByTilemap(layer.parentLayer);
        if (parentLayer == null || parentLayer.layerMap == null)
        {
            Debug.LogWarning($"Parent layer chưa được generate! Generate parent trước.");
            return;
        }

        List<Vector2Int> validPositions = GetValidPositionsInParent(parentLayer, layer.marginFromParent);

        if (validPositions.Count == 0)
        {
            Debug.LogWarning($"Layer {layer.layerName}: Không có vị trí hợp lệ trong parent layer! Thử giảm marginFromParent.");
            return;
        }

        // Tính diện tích parent
        int parentArea = 0;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (parentLayer.layerMap[x, y])
                {
                    parentArea++;
                }
            }
        }

        // Mục tiêu: areaPercentage % diện tích parent
        int targetArea = Mathf.FloorToInt(parentArea * layer.areaPercentage);

        List<Box> boxes = new List<Box>();
        int currentArea = 0;

        // GIAI ĐOẠN 1: Tạo box đầu tiên (kích thước lớn)
        int attempts = 0;
        int maxInitAttempts = 100;

        while (boxes.Count == 0 && attempts < maxInitAttempts)
        {
            attempts++;
            Vector2Int startPos = validPositions[Random.Range(0, validPositions.Count)];
            // Box đầu tiên to để bắt đầu nhanh
            int startW = Random.Range(layer.maxBoxSize - 10, layer.maxBoxSize);
            int startH = Random.Range(layer.maxBoxSize - 10, layer.maxBoxSize);
            Box startBox = new Box(startPos.x, startPos.y, startW, startH);

            if (IsBoxInsideParent(startBox, parentLayer.layerMap, layer.marginFromParent))
            {
                boxes.Add(startBox);
                currentArea += FillBoxAndGetArea(layer.layerMap, startBox, mapWidth, mapHeight);
                break;
            }
        }

        if (boxes.Count == 0)
        {
            Debug.LogWarning($"Layer {layer.layerName}: Không thể tạo box đầu tiên!");
            return;
        }

        // GIAI ĐOẠN 2: Tạo box lớn cho đến khi đạt 40% mục tiêu
        attempts = 0;
        int maxAttempts = 3000;
        int consecutiveFails = 0;
        int maxConsecutiveFails = 100;

        Debug.Log($"[{layer.layerName}] Giai đoạn 1: Tạo box LỚN (0% → 40%)");

        while (currentArea < targetArea * 0.4f && attempts < maxAttempts && consecutiveFails < maxConsecutiveFails)
        {
            attempts++;

            Box baseBox = boxes[Random.Range(0, boxes.Count)];
            // Dùng box LỚN (80-100% maxSize)
            int minSize = Mathf.Max(layer.minBoxSize, (int)(layer.maxBoxSize * 0.8f));
            int maxSize = layer.maxBoxSize;

            Box newBox = CreateConnectedBox(baseBox, minSize, maxSize);

            if (IsBoxInsideParent(newBox, parentLayer.layerMap, layer.marginFromParent))
            {
                int addedArea = FillBoxAndGetArea(layer.layerMap, newBox, mapWidth, mapHeight);

                if (addedArea >= 20) // Chỉ chấp nhận box thêm ít nhất 20 ô
                {
                    boxes.Add(newBox);
                    currentArea += addedArea;
                    consecutiveFails = 0;
                }
                else
                {
                    consecutiveFails++;
                }
            }
            else
            {
                consecutiveFails++;
            }
        }

        // GIAI ĐOẠN 3: Tạo box trung bình (40% → 70%)
        Debug.Log($"[{layer.layerName}] Giai đoạn 2: Tạo box TRUNG BÌNH (40% → 70%)");
        attempts = 0;
        consecutiveFails = 0;

        while (currentArea < targetArea * 0.7f && attempts < maxAttempts && consecutiveFails < maxConsecutiveFails)
        {
            attempts++;

            Box baseBox = boxes[Random.Range(0, boxes.Count)];
            // Dùng box TRUNG BÌNH (50-80% maxSize)
            int minSize = (int)(layer.maxBoxSize * 0.5f);
            int maxSize = (int)(layer.maxBoxSize * 0.8f);

            Box newBox = CreateConnectedBox(baseBox, minSize, maxSize);

            if (IsBoxInsideParent(newBox, parentLayer.layerMap, layer.marginFromParent))
            {
                int addedArea = FillBoxAndGetArea(layer.layerMap, newBox, mapWidth, mapHeight);

                if (addedArea >= 10)
                {
                    boxes.Add(newBox);
                    currentArea += addedArea;
                    consecutiveFails = 0;
                }
                else
                {
                    consecutiveFails++;
                }
            }
            else
            {
                consecutiveFails++;
            }
        }

        // GIAI ĐOẠN 4: Tạo box nhỏ để lấp khe hở (70% → 100%)
        Debug.Log($"[{layer.layerName}] Giai đoạn 3: Tạo box NHỎ để lấp khe (70% → 100%)");
        attempts = 0;
        consecutiveFails = 0;

        while (currentArea < targetArea && attempts < maxAttempts && consecutiveFails < maxConsecutiveFails)
        {
            attempts++;

            Box baseBox = boxes[Random.Range(0, boxes.Count)];
            // Dùng box NHỎ (minSize → 50% maxSize)
            int minSize = layer.minBoxSize;
            int maxSize = Mathf.Max(layer.minBoxSize + 5, (int)(layer.maxBoxSize * 0.5f));

            Box newBox = CreateConnectedBox(baseBox, minSize, maxSize);

            if (IsBoxInsideParent(newBox, parentLayer.layerMap, layer.marginFromParent))
            {
                int addedArea = FillBoxAndGetArea(layer.layerMap, newBox, mapWidth, mapHeight);

                if (addedArea > 0 && currentArea + addedArea <= targetArea * 1.1f)
                {
                    boxes.Add(newBox);
                    currentArea += addedArea;
                    consecutiveFails = 0;
                }
                else
                {
                    consecutiveFails++;
                }
            }
            else
            {
                consecutiveFails++;
            }
        }

        float coveragePercent = (float)currentArea / parentArea * 100f;

        // GIAI ĐOẠN 5: Fill gaps nếu chưa đạt mục tiêu
        if (currentArea < targetArea * 0.95f) // Nếu chưa đạt 95% mục tiêu
        {
            Debug.Log($"[{layer.layerName}] Giai đoạn 4: FILL GAPS - Lấp đầy còn thiếu");
            int addedFromGaps = FillRemainingGaps(layer, parentLayer, targetArea - currentArea);
            currentArea += addedFromGaps;
            coveragePercent = (float)currentArea / parentArea * 100f;
        }

        Debug.Log($"<color=green>Layer {layer.layerName}: Hoàn thành! {boxes.Count} boxes | Diện tích: {currentArea}/{parentArea} ({coveragePercent:F1}%) | Mục tiêu: {layer.areaPercentage * 100}%</color>");

        if (coveragePercent < layer.areaPercentage * 90) // Dưới 90% mục tiêu
        {
            Debug.LogWarning($"Layer {layer.layerName}: Không đạt mục tiêu! Diện tích thiếu: {targetArea - currentArea} ô");
        }
    }
    #endregion

    #region Generate Walls And Stairs
    void GenerateWallsAndStairs(TerrainLayer layer)
    {
        List<Vector2Int> topEdges = new List<Vector2Int>();
        List<Vector2Int> bottomEdges = new List<Vector2Int>();
        List<Vector2Int> leftEdges = new List<Vector2Int>();
        List<Vector2Int> rightEdges = new List<Vector2Int>();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (layer.layerMap[x, y])
                {
                    if (y == 0 || !layer.layerMap[x, y - 1])
                    {
                        bottomEdges.Add(new Vector2Int(x, y));
                    }
                    if (y == mapHeight - 1 || !layer.layerMap[x, y + 1])
                    {
                        topEdges.Add(new Vector2Int(x, y));
                    }
                    if (x == 0 || !layer.layerMap[x - 1, y])
                    {
                        leftEdges.Add(new Vector2Int(x, y));
                    }
                    if (x == mapWidth - 1 || !layer.layerMap[x + 1, y])
                    {
                        rightEdges.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        BorderLayer borderlayer = new BorderLayer(layer, topEdges, bottomEdges, leftEdges, rightEdges);
        Border.borderLayers.Add(borderlayer);

        if (layer.tileWall != null)
        {
            layer.tileWall.ClearAllTiles();
        }
        if (layer.tileStair != null)
        {
            layer.tileStair.ClearAllTiles();
        }

        if (layer.wall && bottomEdges.Count > 0)
        {
            GenerateBottomWallsAndStairs(layer, bottomEdges);
        }

        if (!layer.stair) return;
        if (leftEdges.Count > 0 && layer.tileStair != null)
        {
            GenerateSideStairs(layer, leftEdges, true);
        }

        if (rightEdges.Count > 0 && layer.tileStair != null)
        {
            GenerateSideStairs(layer, rightEdges, false);
        }
    }
    #endregion

    #region Generate Bottom Walls And Stairs
    void GenerateBottomWallsAndStairs(TerrainLayer layer, List<Vector2Int> bottomEdges)
    {
        bottomEdges.Sort((a, b) =>
        {
            int yCompare = a.y.CompareTo(b.y);
            if (yCompare != 0) return yCompare;
            return a.x.CompareTo(b.x);
        });

        List<List<Vector2Int>> segments = new List<List<Vector2Int>>();
        List<Vector2Int> currentSegment = new List<Vector2Int> { bottomEdges[0] };

        for (int i = 1; i < bottomEdges.Count; i++)
        {
            Vector2Int current = bottomEdges[i];
            Vector2Int previous = bottomEdges[i - 1];

            if (current.y == previous.y && current.x == previous.x + 1)
            {
                currentSegment.Add(current);
            }
            else
            {
                segments.Add(currentSegment);
                currentSegment = new List<Vector2Int> { current };
            }
        }
        segments.Add(currentSegment);

        foreach (var segment in segments)
        {
            if (segment.Count == 1)
            {
                Vector3Int pos = new Vector3Int(segment[0].x, segment[0].y - 1, 0);
                if (CheckStartOrEndStair(layer, pos, -1) && CheckStartOrEndStair(layer, pos, 1))
                    layer.tileWall.SetTile(pos, layer.wallTile_center);
                else if (CheckStartOrEndStair(layer, pos, -1))
                    layer.tileWall.SetTile(pos, layer.wallTile_right);
                else if (CheckStartOrEndStair(layer, pos, 1))
                    layer.tileWall.SetTile(pos, layer.wallTile_left);
                continue;
            }

            int stairStartIdx = 0;
            int stairWidth = 0;
            bool canCreateStair = false;
            if (segment.Count > 3)
            {
                stairStartIdx = Random.Range(1, segment.Count - 3);
                stairWidth = Random.Range(3, Mathf.Min(6, segment.Count - 2));
                canCreateStair = true;
            }

            List<Vector3Int> botomStair = new List<Vector3Int>();
            List<Vector3Int> wall = new List<Vector3Int>();

            for (int i = 0; i < segment.Count; i++)
            {
                Vector3Int pos = new Vector3Int(segment[i].x, segment[i].y - 1, 0);

                if (layer.stair && canCreateStair && i >= stairStartIdx && i < stairStartIdx + stairWidth)
                {
                    int stairIdx = i - stairStartIdx;
                    if (stairIdx == 0)
                    {
                        layer.tileStair.SetTile(pos, layer.stairDown_left);
                        botomStair.Add(pos);
                    }
                    else if (stairIdx == stairWidth - 1)
                    {
                        layer.tileStair.SetTile(pos, layer.stairDown_right);
                        botomStair.Add(pos);
                    }
                    else
                    {
                        layer.tileStair.SetTile(pos, layer.stairDown_center);
                        botomStair.Add(pos);
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        if (!CheckStartOrEndStair(layer, pos, -1))
                        {
                            layer.tileWall.SetTile(pos, layer.wallTile_left);
                            wall.Add(pos);
                        }
                        else
                        {
                            layer.tileWall.SetTile(pos, layer.wallTile_center);
                            wall.Add(pos);
                        }
                    }
                    else if (i == segment.Count - 1)
                    {
                        if (!CheckStartOrEndStair(layer, pos, 1))
                        {
                            layer.tileWall.SetTile(pos, layer.wallTile_right);
                            wall.Add(pos);
                        }
                        else
                        {
                            layer.tileWall.SetTile(pos, layer.wallTile_center);
                            wall.Add(pos);
                        }
                    }
                    else
                    {
                        layer.tileWall.SetTile(pos, layer.wallTile_center);
                        wall.Add(pos);
                    }
                }
            }

            if (botomStair.Count > 0)
            {
                Stairs s = new Stairs(botomStair);
                Border.botomStair.Add(s);
            }

            if (wall.Count > 0)
            {
                Stairs s = new Stairs(wall);
                Border.wall.Add(s);
            }
        }

        if (layer.tileWall != null) layer.tileWall.RefreshAllTiles();
        if (layer.tileStair != null) layer.tileStair.RefreshAllTiles();
    }
    #endregion

    #region Check start or end stair
    private bool CheckStartOrEndStair(TerrainLayer layer, Vector3Int pos, int direction)
    {
        int checkX = pos.x + direction;
        if (checkX < 0 || checkX >= mapWidth) return false;
        return layer.layerMap[checkX, pos.y];
    }
    #endregion

    #region Generate Side Stairs
    void GenerateSideStairs(TerrainLayer layer, List<Vector2Int> edges, bool isLeft)
    {
        edges.Sort((a, b) =>
        {
            int xCompare = a.x.CompareTo(b.x);
            if (xCompare != 0) return xCompare;
            return a.y.CompareTo(b.y);
        });

        List<List<Vector2Int>> segments = new List<List<Vector2Int>>();
        List<Vector2Int> currentSegment = new List<Vector2Int> { edges[0] };

        for (int i = 1; i < edges.Count; i++)
        {
            Vector2Int current = edges[i];
            Vector2Int previous = edges[i - 1];

            if (current.x == previous.x && current.y == previous.y + 1)
            {
                currentSegment.Add(current);
            }
            else
            {
                segments.Add(currentSegment);
                currentSegment = new List<Vector2Int> { current };
            }
        }
        segments.Add(currentSegment);

        int stairCount = Random.Range(
            Mathf.CeilToInt(segments.Count * 0.5f),
            Mathf.FloorToInt(segments.Count * 0.8f) + 1
        );
        List<int> selectedSegments = new List<int>();

        for (int i = 0; i < stairCount && segments.Count > 0; i++)
        {
            int idx = Random.Range(0, segments.Count);
            if (!selectedSegments.Contains(idx) && segments[idx].Count >= 3)
            {
                selectedSegments.Add(idx);
            }
        }

        foreach (int segIdx in selectedSegments)
        {
            var segment = segments[segIdx];
            if (segment.Count < 4) continue;

            int stairHeight = segment.Count < 7 ? 3 : segment.Count < 10 ? 4 : 5;
            int startIdx = Random.Range(1, segment.Count - stairHeight);

            List<Vector3Int> leftStair = new List<Vector3Int>();
            List<Vector3Int> rightStair = new List<Vector3Int>();

            for (int i = 0; i < stairHeight; i++)
            {
                Vector3Int pos = new Vector3Int(
                    segment[startIdx + i].x + (isLeft ? -1 : 1),
                    segment[startIdx + i].y,
                    0
                );

                if (isLeft)
                {
                    if (i == 0)
                    {
                        layer.tileStair.SetTile(pos, layer.stairLeft_Down);
                        leftStair.Add(pos);
                    }
                    else if (i == stairHeight - 1)
                    {
                        layer.tileStair.SetTile(pos, layer.stairLeft_Up);
                        leftStair.Add(pos);
                    }
                    else
                    {
                        layer.tileStair.SetTile(pos, layer.stairLeft_center);
                        leftStair.Add(pos);
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        layer.tileStair.SetTile(pos, layer.stairRight_Down);
                        rightStair.Add(pos);
                    }
                    else if (i == stairHeight - 1)
                    {
                        layer.tileStair.SetTile(pos, layer.stairRight_Up);
                        rightStair.Add(pos);
                    }
                    else
                    {
                        layer.tileStair.SetTile(pos, layer.stairRight_center);
                        rightStair.Add(pos);
                    }
                }
            }

            if (leftStair.Count > 0)
            {
                Stairs s = new Stairs(leftStair);
                Border.leftStair.Add(s);
            }

            if (rightStair.Count > 0)
            {
                Stairs s = new Stairs(rightStair);
                Border.rightStair.Add(s);
            }
        }

        layer.tileStair.RefreshAllTiles();
    }
    #endregion

    #region Fill Remaining Gaps
    /*
    Hàm lấp đầy các khoảng trống còn lại để đạt mục tiêu
    */
    int FillRemainingGaps(TerrainLayer layer, TerrainLayer parentLayer, int remainingArea)
    {
        int filled = 0;
        int attempts = 0;
        int maxAttempts = 2000;

        // Tìm tất cả các ô trống trong parent mà chưa được fill
        List<Vector2Int> emptySpots = new List<Vector2Int>();

        for (int x = layer.marginFromParent; x < mapWidth - layer.marginFromParent; x++)
        {
            for (int y = layer.marginFromParent; y < mapHeight - layer.marginFromParent; y++)
            {
                // Ô này trong parent nhưng chưa được fill trong layer hiện tại
                if (parentLayer.layerMap[x, y] && !layer.layerMap[x, y])
                {
                    emptySpots.Add(new Vector2Int(x, y));
                }
            }
        }

        if (emptySpots.Count == 0)
        {
            Debug.Log($"[{layer.layerName}] Không còn khoảng trống để fill!");
            return 0;
        }

        Debug.Log($"[{layer.layerName}] Tìm thấy {emptySpots.Count} ô trống. Cần fill thêm {remainingArea} ô");

        // Shuffle để random
        for (int i = 0; i < emptySpots.Count; i++)
        {
            int randomIndex = Random.Range(i, emptySpots.Count);
            var temp = emptySpots[i];
            emptySpots[i] = emptySpots[randomIndex];
            emptySpots[randomIndex] = temp;
        }

        // Fill từng cụm nhỏ
        while (filled < remainingArea && attempts < maxAttempts && emptySpots.Count > 0)
        {
            attempts++;

            // Lấy vị trí trống ngẫu nhiên
            if (emptySpots.Count == 0) break;

            Vector2Int spot = emptySpots[0];
            emptySpots.RemoveAt(0);

            // Tạo box nhỏ xung quanh vị trí này
            int smallSize = Random.Range(layer.minBoxSize / 2, layer.minBoxSize + 3);
            Box smallBox = new Box(
                spot.x - smallSize / 2,
                spot.y - smallSize / 2,
                smallSize,
                smallSize
            );

            // Kiểm tra box có hợp lệ không
            if (IsBoxInsideParent(smallBox, parentLayer.layerMap, layer.marginFromParent))
            {
                int addedArea = FillBoxAndGetArea(layer.layerMap, smallBox, mapWidth, mapHeight);
                filled += addedArea;

                // Xóa các ô đã được fill khỏi emptySpots
                emptySpots.RemoveAll(pos => layer.layerMap[pos.x, pos.y]);
            }
        }

        Debug.Log($"[{layer.layerName}] Fill gaps: Đã lấp thêm {filled} ô");
        return filled;
    }
    #endregion

    #region Get Valid Positions In Parent
    List<Vector2Int> GetValidPositionsInParent(TerrainLayer parent, int margin)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int x = margin; x < mapWidth - margin; x++)
        {
            for (int y = margin; y < mapHeight - margin; y++)
            {
                if (parent.layerMap[x, y])
                {
                    bool validMargin = true;
                    for (int mx = -margin; mx <= margin; mx++)
                    {
                        for (int my = -margin; my <= margin; my++)
                        {
                            int checkX = x + mx;
                            int checkY = y + my;
                            if (checkX < 0 || checkX >= mapWidth || checkY < 0 || checkY >= mapHeight ||
                                !parent.layerMap[checkX, checkY])
                            {
                                validMargin = false;
                                break;
                            }
                        }
                        if (!validMargin) break;
                    }

                    if (validMargin)
                    {
                        positions.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        return positions;
    }
    #endregion

    #region Is Box Inside Parent
    bool IsBoxInsideParent(Box box, bool[,] parentMap, int margin)
    {
        for (int x = box.x - margin; x < box.x + box.w + margin; x++)
        {
            for (int y = box.y - margin; y < box.y + box.h + margin; y++)
            {
                if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight || !parentMap[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

    #region Create Connected Box
    Box CreateConnectedBox(Box baseBox, int minSize, int maxSize)
    {
        int w = Random.Range(minSize, maxSize);
        int h = Random.Range(minSize, maxSize);
        int dir = Random.Range(0, 4);
        int x = baseBox.x;
        int y = baseBox.y;

        switch (dir)
        {
            case 0: x = baseBox.x + Random.Range(-w / 2, baseBox.w); y = baseBox.y + baseBox.h; break;
            case 1: x = baseBox.x + Random.Range(-w / 2, baseBox.w); y = baseBox.y - h; break;
            case 2: x = baseBox.x - w; y = baseBox.y + Random.Range(-h / 2, baseBox.h); break;
            case 3: x = baseBox.x + baseBox.w; y = baseBox.y + Random.Range(-h / 2, baseBox.h); break;
        }

        return new Box(x, y, w, h);
    }
    #endregion

    #region Fill Box
    void FillBox(bool[,] map, Box box, int width, int height)
    {
        for (int i = Mathf.Max(0, box.x); i < Mathf.Min(width, box.x + box.w); i++)
        {
            for (int j = Mathf.Max(0, box.y); j < Mathf.Min(height, box.y + box.h); j++)
            {
                map[i, j] = true;
            }
        }
    }

    int FillBoxAndGetArea(bool[,] map, Box box, int width, int height)
    {
        int area = 0;
        for (int i = Mathf.Max(0, box.x); i < Mathf.Min(width, box.x + box.w); i++)
        {
            for (int j = Mathf.Max(0, box.y); j < Mathf.Min(height, box.y + box.h); j++)
            {
                if (!map[i, j])
                {
                    map[i, j] = true;
                    area++;
                }
            }
        }
        return area;
    }
    #endregion

    #region Render Layer
    void RenderLayer(TerrainLayer layer)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (layer.layerMap[x, y])
                {
                    layer.tilemap.SetTile(new Vector3Int(x, y, 0), layer.tile);
                }
            }
        }

        layer.tilemap.RefreshAllTiles();
    }
    #endregion

    #region Get Layer By Tilemap
    TerrainLayer GetLayerByTilemap(Tilemap tilemap)
    {
        return layers.Find(l => l.tilemap == tilemap);
    }
    #endregion

    #region Get Layer By Name
    public TerrainLayer GetLayerByName(string name)
    {
        return layers.Find(l => l.layerName == name);
    }
    #endregion

    #region Class Box
    private class Box
    {
        public int x, y, w, h;
        public Box(int x, int y, int w, int h)
        {
            this.x = x; this.y = y; this.w = w; this.h = h;
        }
    }
    #endregion
}

#region Terrain Layer
[System.Serializable]
public class TerrainLayer
{
    public string layerName = "Layer";
    public Tilemap tilemap;
    public TileBase tile;

    [Header("Wall layer")]
    [Tooltip("layer vẽ wall")]
    public Tilemap tileWall;
    public TileBase wallTile_left;
    public TileBase wallTile_center;
    public TileBase wallTile_right;

    [Header("Stair layer")]
    [Tooltip("layer để tạo cầu thang")]
    public Tilemap tileStair;
    public TileBase stairDown_left;
    public TileBase stairDown_center;
    public TileBase stairDown_right;
    public TileBase stairLeft_Up;
    public TileBase stairLeft_center;
    public TileBase stairLeft_Down;
    public TileBase stairRight_Up;
    public TileBase stairRight_center;
    public TileBase stairRight_Down;

    [Header("Generation Settings")]
    [Tooltip("Layer cha - layer này sẽ được tạo bên trong layer cha")]
    public Tilemap parentLayer;

    [Tooltip("Số lượng vùng (box) muốn tạo - CHỈ dùng cho base layer")]
    public int boxCount = 12;

    [Tooltip("% diện tích so với parent (0.5 = 50%, 0.7 = 70%) - CHỈ dùng cho child layer")]
    [Range(0.1f, 0.9f)]
    public float areaPercentage = 0.7f;

    [Tooltip("Kích thước tối thiểu của mỗi box")]
    public int minBoxSize = 15;

    [Tooltip("Kích thước tối đa của mỗi box")]
    public int maxBoxSize = 35;

    [Tooltip("Lề thu hẹp so với layer cha (tính theo số ô)")]
    [Range(0, 20)]
    public int marginFromParent = 5;

    [Tooltip("Có tạo ở giữa không (layer đầu tiên)")]
    public bool startAtCenter = true;

    [Tooltip("Có tạo tường hay không")]
    public bool wall = false;

    [Tooltip("Có tạo cầu thang hay không")]
    public bool stair = false;

    [HideInInspector]
    public bool[,] layerMap;
}
#endregion