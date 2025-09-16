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
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
