using UnityEngine;

public class ButtonRaycaster2D : MonoBehaviour
{
    public static ButtonRaycaster2D Instance { get; private set; }

    private Camera _mainCam;
    private RaycastHit2D _hit;
    private bool _hasHit;

    public RaycastHit2D CurrentHit => _hit;
    public bool HasHit => _hasHit;

    void Awake()
    {
        Instance = this;
        _mainCam = CameraInfo.Instance.cameraMain;
    }

    void Update()
    {
        Vector2 mouseWorldPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        _hasHit = _hit.collider != null;
    }
}
