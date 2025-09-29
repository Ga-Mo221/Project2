using NaughtyAttributes;
using UnityEngine;

public class Moba2DCameraController : MonoBehaviour
{
    public bool canMove = true;
    [ShowIf(nameof(canMove))]
    [SerializeField] public float moveSpeed = 20f;   // tốc độ di chuyển
    [ShowIf(nameof(canMove))]
    [SerializeField] public float edgeSize = 10f;    // khoảng cách mép màn hình để di chuyển
    public bool canZoom = true;
    [ShowIf(nameof(canZoom))]
    [SerializeField] public float zoomSpeed = 2.5f;    // tốc độ zoom
    [ShowIf(nameof(canZoom))]
    [SerializeField] public float minZoom = 10f;      // zoom nhỏ nhất (orthographicSize)
    [ShowIf(nameof(canZoom))]
    [SerializeField] public float maxZoom = 25f;     // zoom lớn nhất

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (canMove)
        {
            pos += new Vector3(0, 0, 0) * moveSpeed * Time.deltaTime;

            // --- Di chuyển bằng mép màn hình ---
            if (Input.mousePosition.x >= Screen.width - edgeSize) // mép phải
                pos.x += moveSpeed * Time.deltaTime;
            if (Input.mousePosition.x <= edgeSize) // mép trái
                pos.x -= moveSpeed * Time.deltaTime;
            if (Input.mousePosition.y >= Screen.height - edgeSize) // mép trên
                pos.y += moveSpeed * Time.deltaTime;
            if (Input.mousePosition.y <= edgeSize) // mép dưới
                pos.y -= moveSpeed * Time.deltaTime;
        }

        if (canZoom)
        {
            // --- Zoom theo con lăn chuột ---
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0)
            {
                // lấy vị trí chuột trong world
                Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

                // zoom in/out
                float oldSize = cam.orthographicSize;
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

                // tỉ lệ thay đổi
                float delta = oldSize - cam.orthographicSize;

                // dịch camera theo hướng từ cam đến chuột để "zoom vào chỗ chuột"
                Vector3 dir = mouseWorldPos - transform.position;
                pos += dir * (delta / oldSize);
            }
        }

        if (canMove || canZoom)
            transform.position = pos;
    }
}
