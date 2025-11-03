using NaughtyAttributes;
using UnityEngine;

public class ButtonCreateBuilding : MonoBehaviour
{
    [Header("Script Data")]
    [SerializeField] private CheckGroundCreate _create;

    [Header("Unit Type")]
    [SerializeField] private bool isCreate = true;

    [Header("Unit Type")]
    [SerializeField] private bool isStorage = false;

    [Header("Mode")]
    [ShowIf(nameof(isStorage))]
    [SerializeField] private bool isRotation = false;

    private bool _isLorR => isStorage && isRotation;
    [Header("Rotation Left - Right")]
    [ShowIf(nameof(_isLorR))]
    [SerializeField] private bool isLeft = false;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite pressedSprite;

    [Header("Button ID")]
    [SerializeField] private ChoseUI buttonID;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private bool _isHover;
    [SerializeField] private bool _isPressed;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite(normalSprite);
    }

    private void Update()
    {
        if (ButtonRaycaster2D.Instance == null) return;

        RaycastHit2D hit = ButtonRaycaster2D.Instance.CurrentHit;
        bool isHoveredNow = ButtonRaycaster2D.Instance.HasHit && hit.collider != null && hit.collider.gameObject == gameObject;

        // Khi hover thay đổi
        if (isHoveredNow != _isHover)
        {
            _isHover = isHoveredNow;
            if (_isHover && CursorManager.Instance != null && CursorManager.Instance.ID == buttonID.getID())
                SetSprite(hoverSprite);
            else
                SetSprite(normalSprite);
        }

        // Xử lý nhấn
        if (_isHover && CursorManager.Instance != null && CursorManager.Instance.ID == buttonID.getID())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isPressed = true;
                SetSprite(pressedSprite);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isPressed)
                {
                    _isPressed = false;
                    OnClick();
                    SetSprite(hoverSprite);
                }
            }
        }
        else
        {
            _isPressed = false;
        }
    }

    private void SetSprite(Sprite sprite)
    {
        if (_spriteRenderer.sprite != sprite)
            _spriteRenderer.sprite = sprite;
    }

    private void OnClick()
    {
        if (!_isLorR)
        {
            if (isCreate && _create != null)
            {
                _create.Create();
            }
            else if (!isCreate && _create != null)
            {
                _create.Cancle();
            }
        }
        else
        {
            if (isLeft)
            {
                _create.RotationLeft();
            }
            else
            {
                _create.RotationRight();
            }
        }
    }
}
