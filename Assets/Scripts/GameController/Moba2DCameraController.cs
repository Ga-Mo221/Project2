using NaughtyAttributes;
using UnityEngine;

public class Moba2DCameraController : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    [ShowIf(nameof(canMove))][SerializeField] private float moveSpeed = 20f;
    [ShowIf(nameof(canMove))][SerializeField] private float edgeSize = 10f;

    [Header("Zoom")]
    public bool canZoom = true;
    [ShowIf(nameof(canZoom))][SerializeField] private float zoomSpeed = 2.5f;
    [ShowIf(nameof(canZoom))][SerializeField] private float minZoom = 10f;
    [ShowIf(nameof(canZoom))][SerializeField] private float maxZoom = 25f;

    [SerializeField] private CameraLimit _cameraLimit;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.getGameOver()) return;
        if (SelectionBox.Instance.getTutorial()) return;
        Vector3 pos = transform.position;

        // --- Di chuyển ---
        if (canMove && !CursorManager.Instance.ChoseUI)
        {
            if (Input.mousePosition.x >= Screen.width - edgeSize) pos.x += moveSpeed * Time.deltaTime;
            if (Input.mousePosition.x <= edgeSize) pos.x -= moveSpeed * Time.deltaTime;
            if (Input.mousePosition.y >= Screen.height - edgeSize) pos.y += moveSpeed * Time.deltaTime;
            if (Input.mousePosition.y <= edgeSize) pos.y -= moveSpeed * Time.deltaTime;
        }

        // --- Zoom ---
        if (canZoom)
        {
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0)
            {
                Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

                float oldSize = cam.orthographicSize;
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

                float delta = oldSize - cam.orthographicSize;
                Vector3 dir = mouseWorldPos - transform.position;
                pos += dir * (delta / oldSize);
            }
        }

        // --- Giới hạn vùng camera ---
        if (_cameraLimit.useLimit)
        {
            float camHalfHeight = cam.orthographicSize;
            float camHalfWidth = camHalfHeight * cam.aspect;

            pos.x = Mathf.Clamp(pos.x, _cameraLimit.minBound.x + camHalfWidth, _cameraLimit.maxBound.x - camHalfWidth);
            pos.y = Mathf.Clamp(pos.y, _cameraLimit.minBound.y + camHalfHeight, _cameraLimit.maxBound.y - camHalfHeight);
        }

        transform.position = pos;

        SpaceClick();
    }


    // space
    private void SpaceClick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var pos = Castle.Instance.transform.position;
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
