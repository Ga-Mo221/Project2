using System.Collections.Generic;
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

    [Header("Right Mouse Drag")]
    [SerializeField] private bool canDrag = true;
    [SerializeField] private float dragSpeed = 0.1f;

    private Vector3 lastMousePos;
    private bool isDragging = false;

    [SerializeField] private CameraLimit _cameraLimit;

    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private PlayerAI _currentTarget;
    private Vector3 _velocity;
    private int _currentPlayerIndex = -1;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        moveSpeed = SettingManager.Instance._gameSettings._mouseSensitivity;
        canDrag = SettingManager.Instance._gameSettings._panCameraRightMouse;
        dragSpeed = SettingManager.Instance._gameSettings._speedPanCamera;
        canMove = !canDrag;
    }

    void Update()
    {
        if (GameManager.Instance.getGameOver()) return;
        if (GameManager.Instance.Tutorial) return;


        Vector3 pos = transform.position;

        // --- Di chuyển ---
        if (canMove && !CursorManager.Instance.ChoseUI)
        {
            bool moved = false;

            if (Input.mousePosition.x >= Screen.width - edgeSize)
            {
                pos.x += moveSpeed * Time.deltaTime;
                moved = true;
            }
            if (Input.mousePosition.x <= edgeSize)
            {
                pos.x -= moveSpeed * Time.deltaTime;
                moved = true;
            }
            if (Input.mousePosition.y >= Screen.height - edgeSize)
            {
                pos.y += moveSpeed * Time.deltaTime;
                moved = true;
            }
            if (Input.mousePosition.y <= edgeSize)
            {
                pos.y -= moveSpeed * Time.deltaTime;
                moved = true;
            }

            if (moved)
            {
                StopAllCoroutines();
                _currentTarget = null; // reset focus khi người chơi di chuyển tay
            }
        }

        // --- Zoom ---
        if (canZoom && !CursorManager.Instance.ChoseUI)
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

        if (canDrag)
        {
            if (Input.GetMouseButtonDown(1)) // Khi nhấn chuột phải
            {
                isDragging = true;
                lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1)) // Khi thả chuột phải
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector3 delta = Input.mousePosition - lastMousePos;
                pos.x -= delta.x * dragSpeed * Time.deltaTime;
                pos.y -= delta.y * dragSpeed * Time.deltaTime;
                lastMousePos = Input.mousePosition;
            }
        }

        // --- Giới hạn vùng camera ---
        if (_cameraLimit.useLimit)
        {
            float camHalfHeight = cam.orthographicSize;
            float camHalfWidth = camHalfHeight * cam.aspect;

            pos.x = Mathf.Clamp(pos.x, _cameraLimit.WorldMinBound.x + camHalfWidth, _cameraLimit.WorldMaxBound.x - camHalfWidth);
            pos.y = Mathf.Clamp(pos.y, _cameraLimit.WorldMinBound.y + camHalfHeight, _cameraLimit.WorldMaxBound.y - camHalfHeight);
        }

        transform.position = pos;

        SpaceClick();
    }

    void OnEnable()
    {
        SettingManager.Instance._onMouseChanged += ApplyMouseSensitivityChanged;
        SettingManager.Instance._onRightMousePanCamera += ApplyCanPanCameraMouseRight;
        SettingManager.Instance._onRightMouseSpeed += ApplySpeedPanCameraMouseRight;
    }

    void OnDisable()
    {
        SettingManager.Instance._onMouseChanged += ApplyMouseSensitivityChanged;
        SettingManager.Instance._onRightMousePanCamera += ApplyCanPanCameraMouseRight;
        SettingManager.Instance._onRightMouseSpeed -= ApplySpeedPanCameraMouseRight;
    }

    private void ApplyMouseSensitivityChanged()
    {
        moveSpeed = SettingManager.Instance._gameSettings._mouseSensitivity;
    }

    private void ApplyCanPanCameraMouseRight()
    {
        canDrag = SettingManager.Instance._gameSettings._panCameraRightMouse;
        dragSpeed = SettingManager.Instance._gameSettings._speedPanCamera;
        canMove = !canDrag;
    }
    
    private void ApplySpeedPanCameraMouseRight()
    {
        dragSpeed = SettingManager.Instance._gameSettings._speedPanCamera;
    }

    private void SpaceClick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                FocusToCastle();
            else if (Input.GetKey(KeyCode.LeftControl))
                FocusToNextImportantPlayer();
            else
                CycleToNextAlivePlayer();
        }
    }

    private void FocusToCastle()
    {
        var pos = Castle.Instance.transform.position;
        StopAllCoroutines();
        StartCoroutine(SmoothMoveTo(new Vector3(pos.x, pos.y, transform.position.z)));
    }

    private void CycleToNextAlivePlayer()
    {
        var allAlive = GetAllAlivePlayers();
        if (allAlive.Count == 0)
        {
            Debug.Log("Không có player nào còn sống để focus.");
            return;
        }

        // Nếu chưa có target -> chọn người gần camera nhất
        if (_currentTarget == null)
        {
            _currentTarget = FindClosestPlayerToCamera(allAlive);
            _currentPlayerIndex = allAlive.IndexOf(_currentTarget);
        }
        else
        {
            int currentIndex = allAlive.IndexOf(_currentTarget);
            _currentPlayerIndex = (currentIndex + 1) % allAlive.Count;
            _currentTarget = allAlive[_currentPlayerIndex];
        }

        if (_currentTarget == null) return;

        StopAllCoroutines();
        StartCoroutine(SmoothMoveTo(_currentTarget.transform.position));
    }

    private void FocusToNextImportantPlayer()
    {
        var allPlayers = GetAllAlivePlayers();
        if (allPlayers.Count == 0) return;

        PlayerAI best = null;
        float bestScore = float.MinValue;

        foreach (var p in allPlayers)
        {
            float score = p.GetPlayerPriority();
            if (score > bestScore)
            {
                bestScore = score;
                best = p;
            }
        }

        if (best == null || bestScore <= 0)
        {
            best = FindClosestPlayerToCamera(allPlayers);
        }

        _currentTarget = best;
        StopAllCoroutines();
        StartCoroutine(SmoothMoveTo(best.transform.position));
    }

    private System.Collections.IEnumerator SmoothMoveTo(Vector3 targetPos)
    {
        targetPos.z = transform.position.z;
        if (Vector3.Distance(transform.position, targetPos) <= 0.1f) yield break;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, smoothTime);
            yield return null;
        }
    }

    private PlayerAI FindClosestPlayerToCamera(List<PlayerAI> list)
    {
        PlayerAI best = null;
        float bestDist = Mathf.Infinity;

        foreach (var p in list)
        {
            float d = Vector3.Distance(transform.position, p.transform.position);
            if (d < bestDist)
            {
                best = p;
                bestDist = d;
            }
        }
        return best;
    }

    private List<PlayerAI> GetAllAlivePlayers()
    {
        List<PlayerAI> all = new List<PlayerAI>();
        if (Castle.Instance == null) return all;

        void AddAlive(List<PlayerAI> list)
        {
            foreach (var p in list)
            {
                if (p == null) continue;
                if (!p.getDie()) all.Add(p);
            }
        }

        AddAlive(Castle.Instance._ListWarrior);
        AddAlive(Castle.Instance._ListArcher);
        AddAlive(Castle.Instance._ListLancer);
        AddAlive(Castle.Instance._ListHealer);
        AddAlive(Castle.Instance._ListTNT);

        return all;
    }
}
