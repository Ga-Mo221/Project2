using UnityEngine;

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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        if (_collider.OverlapPoint(mouseWorldPos))
        {
            if (!_isHovering)
            {
                CursorManager.Instance.ChoseUI = true;
                CursorManager.Instance.ID = ID;
                _isHovering = true;
            }
        }
        else
        {
            if (_isHovering) 
            {
                CursorManager.Instance.ChoseUI = false;
                CursorManager.Instance.ID = 1;
                _isHovering = false;
            }
        }
    }
}
