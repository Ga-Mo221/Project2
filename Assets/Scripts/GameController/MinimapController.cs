// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;

// public class MinimapController : MonoBehaviour, IPointerClickHandler
// {
//     [Header("References")]
//     public Camera minimapCamera;     // Camera phụ để render minimap
//     private Camera mainCamera;        // Camera chính
//     public RawImage minimapImage;    // UI hiển thị minimap (RawImage)

//     [SerializeField] private Collider2D _collider;

//     private bool _isHovering = false;

//     void Start()
//     {
//         GameObject mainCamObj = Camera.main?.gameObject;
//         mainCamera = mainCamObj.GetComponent<Camera>();
//     }

//     void Update()
//     {
//         Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//         if (_collider.OverlapPoint(mouseWorldPos))
//         {
//             if (!_isHovering) // chỉ đổi khi mới hover lần đầu
//             {
//                 CursorManager.Instance.ChoseMiniMap = true;
//                 _isHovering = true;
//             }
//         }
//         else
//         {
//             if (_isHovering) // reset khi chuột rời khỏi
//             {
//                 CursorManager.Instance.ChoseMiniMap = false;
//                 _isHovering = false;
//             }
//         }
//     }

//     public void OnPointerClick(PointerEventData eventData)
//     {
//         // Bước 1: Lấy tọa độ click trên RawImage
//         RectTransform rect = minimapImage.rectTransform;
//         Vector2 localPoint;
//         if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint))
//             return;

//         // Bước 2: Quy đổi sang UV (0–1)
//         Vector2 normalized = new Vector2(
//             Mathf.InverseLerp(rect.rect.xMin, rect.rect.xMax, localPoint.x),
//             Mathf.InverseLerp(rect.rect.yMin, rect.rect.yMax, localPoint.y)
//         );

//         // Bước 3: Từ UV -> viewport point trong minimap camera
//         Vector3 viewport = new Vector3(normalized.x, normalized.y, 0);

//         // Bước 4: Raycast từ minimap camera xuống world
//         Ray ray = minimapCamera.ViewportPointToRay(viewport);
//         Plane groundPlane = new Plane(Vector3.forward, Vector3.zero); // giả sử game nằm ở mặt phẳng Z=0
//         float enter;
//         if (groundPlane.Raycast(ray, out enter))
//         {
//             Vector3 worldPos = ray.GetPoint(enter);

//             // Giữ nguyên Z của camera chính
//             worldPos.z = mainCamera.transform.position.z;

//             // Bước 5: Dịch chuyển main camera
//             mainCamera.transform.position = worldPos;
//         }
//     }
// }

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    public Camera minimapCamera;     // Camera phụ để render minimap
    private Camera mainCamera;       // Camera chính
    public RawImage minimapImage;    // UI hiển thị minimap (RawImage)

    [SerializeField] private Collider2D _collider;

    //private bool _isHovering = false;
    private bool _isDragging = false;

    void Start()
    {
        GameObject mainCamObj = Camera.main?.gameObject;
        mainCamera = mainCamObj.GetComponent<Camera>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        MoveCameraTo(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            MoveCameraTo(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    private void MoveCameraTo(PointerEventData eventData)
    {
        RectTransform rect = minimapImage.rectTransform;
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint))
            return;

        // Bước 2: Quy đổi sang UV (0–1)
        Vector2 normalized = new Vector2(
            Mathf.InverseLerp(rect.rect.xMin, rect.rect.xMax, localPoint.x),
            Mathf.InverseLerp(rect.rect.yMin, rect.rect.yMax, localPoint.y)
        );

        // Bước 3: Từ UV -> viewport point trong minimap camera
        Vector3 viewport = new Vector3(normalized.x, normalized.y, 0);

        // Bước 4: Raycast từ minimap camera xuống world
        Ray ray = minimapCamera.ViewportPointToRay(viewport);
        Plane groundPlane = new Plane(Vector3.forward, Vector3.zero); // giả sử game nằm ở mặt phẳng Z=0
        float enter;
        if (groundPlane.Raycast(ray, out enter))
        {
            Vector3 worldPos = ray.GetPoint(enter);

            // Giữ nguyên Z của camera chính
            worldPos.z = mainCamera.transform.position.z;

            // Bước 5: Dịch chuyển main camera
            mainCamera.transform.position = worldPos;
        }
    }
}
