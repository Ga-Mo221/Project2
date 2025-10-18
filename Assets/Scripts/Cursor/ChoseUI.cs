using UnityEngine;

public class ChoseUI : MonoBehaviour
{
    private Collider2D _collider;

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
                _isHovering = true;
            }
        }
        else
        {
            if (_isHovering) 
            {
                CursorManager.Instance.ChoseUI = false;
                _isHovering = false;
            }
        }
    }
}
