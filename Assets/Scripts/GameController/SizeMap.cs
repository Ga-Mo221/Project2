using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class SizeMap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Color gizmoColor = Color.cyan;
    [SerializeField] private bool drawCircle = true;
    [SerializeField] private bool drawCornerMarkers = true;

    private Vector3 center;
    private Vector2 size;
    private float radius;
    private bool updateLimit = false;
    private bool updateMinimapCam = false;
    private bool hasTile = false;

    void Start()
    {
        if (tilemap == null)
            gameObject.SetActive(false);
    }

    void Update()
    {
        CalculateBounds(); // ✅ luôn tính lại mỗi frame trong editor/runtime

        // Khi tilemap đã có tile và chưa sync camera → sync một lần
        if (!updateLimit && hasTile && CameraInfo.Instance != null)
        {
            updateLimit = true;
            Debug.Log("[SizeMap] Map ready — Syncing with camera...");

            // // 🔹 Gọi DetectionManager khởi tạo grid
            // if (DetectionManager.Instance != null)
            // {
            //     DetectionManager.Instance.InitializeGrid(center, size);
            // }

            CameraInfo.Instance.SyncLimitWithMap(this);
        }
        if (!updateMinimapCam && hasTile && GameManager.Instance != null && GameManager.Instance._cameraMiniMap != null)
        {
            updateMinimapCam = true;
            Debug.Log("[SizeMap] Map ready — Syncing with minimap camera...");
            updateCameraMinimap();
        }

        if (updateLimit && updateMinimapCam)
            gameObject.SetActive(false); // tắt script sau khi sync xong
    }

    private void updateCameraMinimap()
    {
        GameManager.Instance._cameraMiniMap.transform.position = new Vector3(center.x, center.y, GameManager.Instance._cameraMiniMap.transform.position.z);
        GameManager.Instance._cameraMiniMap.orthographicSize = radius + 10f;
    }

    private void CalculateBounds()
    {
        if (tilemap == null)
            return;
        BoundsInt b = tilemap.cellBounds;
        Vector3 cellSize = tilemap.cellSize;

        float minX = float.PositiveInfinity;
        float minY = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float maxY = float.NegativeInfinity;
        hasTile = false;

        for (int x = b.xMin; x < b.xMax; x++)
        {
            for (int y = b.yMin; y < b.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(cell)) continue;
                hasTile = true;

                Vector3 worldBL = tilemap.CellToWorld(cell);
                float wx0 = worldBL.x;
                float wy0 = worldBL.y;
                float wx1 = worldBL.x + cellSize.x;
                float wy1 = worldBL.y + cellSize.y;

                if (wx0 < minX) minX = wx0;
                if (wy0 < minY) minY = wy0;
                if (wx1 > maxX) maxX = wx1;
                if (wy1 > maxY) maxY = wy1;
            }
        }

        // Nếu map rỗng → reset về 0
        if (!hasTile)
        {
            size = Vector2.zero;
            center = Vector3.zero;
            radius = 0f;
            return;
        }

        float width = maxX - minX;
        float height = maxY - minY;
        size = new Vector2(width, height);
        center = new Vector3(minX + width * 0.5f, minY + height * 0.5f, transform.position.z);
        radius = Mathf.Sqrt(width * width / 4f + height * height / 4f);
    }

    private void OnDrawGizmos()
    {
        if (tilemap == null || size == Vector2.zero)
            return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 0.01f));

        if (drawCornerMarkers)
        {
            float markerSize = Mathf.Min(size.x, size.y) * 0.02f + 0.05f;
            Vector3 half = new Vector3(size.x / 2f, size.y / 2f, 0);
            Gizmos.DrawSphere(center + new Vector3(-half.x, -half.y, 0), markerSize); // BL
            Gizmos.DrawSphere(center + new Vector3(-half.x, half.y, 0), markerSize);  // TL
            Gizmos.DrawSphere(center + new Vector3(half.x, -half.y, 0), markerSize);  // BR
            Gizmos.DrawSphere(center + new Vector3(half.x, half.y, 0), markerSize);   // TR
        }

        if (drawCircle)
        {
            int segments = 64;
            Vector3 prev = center + new Vector3(radius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                Vector3 next = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }

#if UNITY_EDITOR
        UnityEditor.Handles.Label(center + Vector3.up * (Mathf.Max(size.y, 1f) * 0.05f),
            $"Size: {size.x:F2} x {size.y:F2}\nCenter: {center.x:F2},{center.y:F2}\nRadius: {radius:F2}");
#endif
    }

    // Public getter nếu cần dùng ở nơi khác
    public Vector3 GetCenter() => center;
    public Vector2 GetSize() => size;
    public float GetRadius() => radius;
}
