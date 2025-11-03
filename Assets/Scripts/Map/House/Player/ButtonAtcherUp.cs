using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ButtonAtcherUp : MonoBehaviour
{
    [Header("Unit Type")]
    [SerializeField] private bool isCastle = true;

    [ShowIf(nameof(isCastle))]
    [Header("Direction")]
    [SerializeField] private ButtonAtcherUp_Direction direction;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite pressedSprite;

    [Header("Script Data")]
    [ShowIf(nameof(isCastle))][SerializeField] private SelectArcherUp archerUpCastle;
    [Header("Script Data")]
    [HideIf(nameof(isCastle))] [SerializeField] private InTower archerUpTower;

    [Header("Button ID")]
    [SerializeField] private ChoseUI buttonID;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private bool _isHover;
    [SerializeField] private bool _isPressed;
    [SerializeField] private bool isHoveredNow;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite(normalSprite);
    }

    private void Update()
    {
        if (ButtonRaycaster2D.Instance == null) return;

        RaycastHit2D hit = ButtonRaycaster2D.Instance.CurrentHit;
        isHoveredNow = ButtonRaycaster2D.Instance.HasHit && hit.collider != null && hit.collider.gameObject == gameObject;

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
        if (isCastle && archerUpCastle != null)
        {
            switch (direction)
            {
                case ButtonAtcherUp_Direction.Left: archerUpCastle.button_Left(); break;
                case ButtonAtcherUp_Direction.Center: archerUpCastle.button_Centter(); break;
                case ButtonAtcherUp_Direction.Right: archerUpCastle.button_Right(); break;
            }
        }
        else if (archerUpTower != null)
        {
            archerUpTower.ButtonDown();
        }
    }
}

public enum ButtonAtcherUp_Direction
{
    Left,
    Center,
    Right
}
