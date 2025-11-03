using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImprovedBorderGenerator : MonoBehaviour
{
    [SerializeField] private TileBase tilebase;
    [SerializeField] private int mapWidth = 100;  // Thêm để check bounds
    [SerializeField] private int mapHeight = 100;

    [SerializeField] private TilemapHelper tilemapHelper;
    public List<BorderLayer> borderLayers = new List<BorderLayer>();
    public List<Stairs> wall = new List<Stairs>();
    public List<Stairs> botomStair = new List<Stairs>();
    public List<Stairs> leftStair = new List<Stairs>();
    public List<Stairs> rightStair = new List<Stairs>();

    void Start()
    {
        // Đợi map tạo xong sẽ generate border
        StartCoroutine(LoadLayerMap());
    }

    IEnumerator LoadLayerMap()
    {
        yield return new WaitForSeconds(0.4f);

        foreach (var borderlayer in borderLayers)
        {
            GameObject tileObj = new GameObject($"Border_{borderlayer.layer.layerName}");
            tileObj.layer = gameObject.layer;
            tileObj.transform.parent = transform;

            Tilemap tilemap = tileObj.AddComponent<Tilemap>();

            if (tilemapHelper != null) tilemapHelper.tilemaps.Add(tilemap);

            tileObj.AddComponent<TilemapRenderer>();
            tileObj.AddComponent<TilemapCollider2D>();

            borderlayer.tilemap = tilemap;
        }

        GenerateAllBorder();
    }

    private void GenerateAllBorder()
    {
        foreach (var borderlayer in borderLayers)
        {
            GenerateBorderForLayer(borderlayer);
        }

        if (LoadingSceneController.Instance != null)
        {
            LoadingSceneController.Instance.NotifyBorderComplete();
        }
    }

    private void GenerateBorderForLayer(BorderLayer border)
    {
        if (border.layer == null || border.tilemap == null) return;

        // Biên cho vùng bình thường
        foreach (var pos in border.bottomEdges)
        {
            border.tilemap.SetTile(ScalePos(pos, 0, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 1, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 2, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 3, -1), tilebase);
        }
        
        foreach (var pos in border.topEdges)
        {
            border.tilemap.SetTile(ScalePos(pos, 0, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 1, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 2, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 3, 4), tilebase);
        }
        
        foreach (var pos in border.leftEdges)
        {
            border.tilemap.SetTile(ScalePos(pos, -1, 0), tilebase); 
            border.tilemap.SetTile(ScalePos(pos, -1, 1), tilebase); 
            border.tilemap.SetTile(ScalePos(pos, -1, 2), tilebase); 
            border.tilemap.SetTile(ScalePos(pos, -1, 3), tilebase); 
        }
        
        foreach (var pos in border.rightEdges)
        {
            border.tilemap.SetTile(ScalePos(pos, 4, 0), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 4, 1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 4, 2), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 4, 3), tilebase);
        }

        // Nếu layer có cầu thang thì thêm border theo quy tắc riêng
        if (botomStair.Count > 0
            || leftStair.Count > 0
            || rightStair.Count > 0)
        {
            BorderAroundStairs(border);
        }

        if (wall.Count > 0)
        {
            BorderWall(border);
        }

        border.tilemap.RefreshAllTiles();
    }

    private void BorderAroundStairs(BorderLayer border)
    {
        // Cầu thang xuống → border 2 bên (trái và phải)
        foreach (var stair in botomStair)
        {
            Vector2Int pos = stair.stairs[0];
            // Border bên TRÁI
            border.tilemap.SetTile(ScalePos(pos, -1, 0), tilebase);
            border.tilemap.SetTile(ScalePos(pos, -1, 1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, -1, 2), tilebase);
            border.tilemap.SetTile(ScalePos(pos, -1, 3), tilebase);

            pos = stair.stairs[stair.stairs.Count - 1];
            // Border bên PHẢI (đã sửa lỗi: trước đây set cùng 1 vị trí 4 lần)
            border.tilemap.SetTile(ScalePos(pos, 4, 0), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 4, 1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 4, 2), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 4, 3), tilebase);

            foreach (var i in stair.stairs)
            {
                border.tilemap.SetTile(ScalePos(i, 0, 3), null);
                border.tilemap.SetTile(ScalePos(i, 1, 3), null);
                border.tilemap.SetTile(ScalePos(i, 2, 3), null);
                border.tilemap.SetTile(ScalePos(i, 3, 3), null);
            }
        }

        // Cầu thang trái → border trên dưới
        foreach (var stair in leftStair)
        {
            Vector2Int pos = stair.stairs[0];
            // Border phía DƯỚI
            border.tilemap.SetTile(ScalePos(pos, 0, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 1, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 2, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 3, -1), tilebase);

            pos = stair.stairs[stair.stairs.Count - 1];
            // Border phía TRÊN
            border.tilemap.SetTile(ScalePos(pos, 0, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 1, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 2, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 3, 4), tilebase);

            foreach (var i in stair.stairs)
            {
                border.tilemap.SetTile(ScalePos(i, 3, 0), null);
                border.tilemap.SetTile(ScalePos(i, 3, 1), null);
                border.tilemap.SetTile(ScalePos(i, 3, 2), null);
                border.tilemap.SetTile(ScalePos(i, 3, 3), null);
            }
        }

        // Cầu thang phải → border trên dưới
        foreach (var stair in rightStair)
        {
            Vector2Int pos = stair.stairs[0];
            // Border phía DƯỚI
            border.tilemap.SetTile(ScalePos(pos, 0, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 1, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 2, -1), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 3, -1), tilebase);

            pos = stair.stairs[stair.stairs.Count - 1];
            // Border phía TRÊN
            border.tilemap.SetTile(ScalePos(pos, 0, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 1, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 2, 4), tilebase);
            border.tilemap.SetTile(ScalePos(pos, 3, 4), tilebase);

            foreach (var i in stair.stairs)
            {
                border.tilemap.SetTile(ScalePos(i, 0, 0), null);
                border.tilemap.SetTile(ScalePos(i, 0, 1), null);
                border.tilemap.SetTile(ScalePos(i, 0, 2), null);
                border.tilemap.SetTile(ScalePos(i, 0, 3), null);
            }
        }
    }
    
    private void BorderWall(BorderLayer border)
    {
        foreach( var wal in wall)
        {
            foreach (var i in wal.stairs)
            {
                border.tilemap.SetTile(ScalePos(i, 0, +1), tilebase);
                border.tilemap.SetTile(ScalePos(i, 1, +1), tilebase);
                border.tilemap.SetTile(ScalePos(i, 2, +1), tilebase);
                border.tilemap.SetTile(ScalePos(i, 3, +1), tilebase);

                border.tilemap.SetTile(ScalePos(i, 0, +2), tilebase);
                border.tilemap.SetTile(ScalePos(i, 1, +2), tilebase);
                border.tilemap.SetTile(ScalePos(i, 2, +2), tilebase);
                border.tilemap.SetTile(ScalePos(i, 3, +2), tilebase);
            }
        }
    }

    /// <summary>
    /// Kiểm tra vị trí có phải cầu thang không bằng cách check Tilemap
    /// Đơn giản và hiệu quả hơn việc so sánh từng tile cụ thể
    /// </summary>
    private bool IsStairPosition(TerrainLayer layer, int x, int y)
    {
        // Kiểm tra bounds
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
            return false;
        
        // Kiểm tra xem vị trí này có tile trong stairMap không
        if (layer.tileStair == null)
            return false;
            
        TileBase tile = layer.tileStair.GetTile(new Vector3Int(x, y, 0));
        return tile != null;
    }

    /// <summary>
    /// Chuyển đổi tọa độ từ grid 2x2 sang grid 2/4x2/4
    /// Scale factor = 2 / (2/4) = 4
    /// </summary>
    private Vector3Int ScalePos(Vector2Int pos, int offsetX = 0, int offsetY = 0)
    {
        int scale = 4; // Sửa lại: 2 / (2/4) = 4
        return new Vector3Int(pos.x * scale + offsetX, pos.y * scale + offsetY, 0);
    }
}

[System.Serializable]
public class BorderLayer
{
    public Tilemap tilemap;
    public TerrainLayer layer;
    public bool stair = false;
    public List<Vector2Int> topEdges = new List<Vector2Int>();
    public List<Vector2Int> bottomEdges = new List<Vector2Int>();
    public List<Vector2Int> leftEdges = new List<Vector2Int>();
    public List<Vector2Int> rightEdges = new List<Vector2Int>();

    public BorderLayer(TerrainLayer layer,
                       List<Vector2Int> topEdges,
                       List<Vector2Int> bottomEdges,
                       List<Vector2Int> leftEdges,
                       List<Vector2Int> rightEdges)
    {
        this.layer = layer;
        this.stair = layer.stair;
        this.topEdges = topEdges;
        this.bottomEdges = bottomEdges;
        this.leftEdges = leftEdges;
        this.rightEdges = rightEdges;
    }
}

[System.Serializable]
public class Stairs
{
    public List<Vector2Int> stairs = new List<Vector2Int>();
    public Stairs(List<Vector3Int> st)
    {
        foreach (var s in st)
        {
            stairs.Add(new Vector2Int(s.x, s.y));
        }
    }
}