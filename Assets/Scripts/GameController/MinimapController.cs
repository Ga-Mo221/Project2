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
        mainCamera = CameraInfo.Instance.cameraMain;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.Tutorial) return;
        _isDragging = true;
        MoveCameraTo(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.Tutorial) return;
        if (_isDragging)
        {
            MoveCameraTo(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.Tutorial) return;
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
