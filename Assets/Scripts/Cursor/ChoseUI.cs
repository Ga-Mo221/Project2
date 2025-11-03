using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoseUI : MonoBehaviour
{
    private Collider2D _collider;

    [SerializeField] private int ID = 1;

    private bool _isHovering = false;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        Vector3 mouseWorldPos = Vector3.zero;
        if (GameManager.Instance != null && GameScene.Instance != null && SceneManager.GetActiveScene().name == GameScene.Instance._mainMenuScene)
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if (CameraInfo.Instance != null)
            mouseWorldPos = CameraInfo.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
        
        mouseWorldPos.z = 0f;
        if (_collider.OverlapPoint(mouseWorldPos))
        {
            if (!_isHovering)
            {
                if (CursorManager.Instance != null)
                {
                    CursorManager.Instance.ChoseUI = true;
                    CursorManager.Instance.ID = ID;
                }
                _isHovering = true;
            }
        }
        else
        {
            if (_isHovering)
            {
                if (CursorManager.Instance != null)
                {
                    CursorManager.Instance.ChoseUI = false;
                    CursorManager.Instance.ID = 1;
                }
                _isHovering = false;
            }
        }
    }

    public int getID() => ID;
}
