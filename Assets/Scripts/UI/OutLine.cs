using UnityEngine;

public class OutLine : MonoBehaviour
{
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _outLineMateral;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRender;

    private bool _isHovering = false;

    void Start()
    {
        if (!_spriteRender || _spriteRender == null)
            _spriteRender = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        if (!_spriteRender || _spriteRender == null)
            Debug.LogError($"[{transform.parent.name}] [OutLine] Chưa gán 'SpriteRender'");
    }

    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_collider.OverlapPoint(mouseWorldPos))
        {
            if (!_isHovering) // chỉ đổi khi mới hover lần đầu
            {
                ChangeMaterial();
                _isHovering = true;
            }
        }
        else
        {
            if (_isHovering) // reset khi chuột rời khỏi
            {
                ResetMaterial();
                _isHovering = false;
            }
        }
    }

    private void ChangeMaterial()
    {
        if (_spriteRender.enabled == false) return;
        _outLineMateral.SetColor("_Color", Color.red);
        _outLineMateral.SetFloat("_Size", 3.0f);

        if (_spriteRender != null && _spriteRender.sprite != null)
        {
            Texture2D tex = _spriteRender.sprite.texture;
            _outLineMateral.SetTexture("_MainTex", tex);
        }

        _spriteRender.material = _outLineMateral;
        CursorManager.Instance.SetSelectCursor(transform.parent.gameObject);
    }

    private void ResetMaterial()
    {
        if (_spriteRender.enabled == false) return;
        _spriteRender.material = _normalMaterial;
        CursorManager.Instance.SetNormalCursor();
    }
}
