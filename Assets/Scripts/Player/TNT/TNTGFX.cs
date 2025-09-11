using UnityEngine;

public class TNTGFX : PlayerAI
{
    [Header("TNT GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[TNTGFX] Chưa gán 'SpriteRender'");
    }

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100);
        flip();
        if (getIsAI())
        {
            setupFolow();
            if (!checkFullInventory() && !getIsTarget())
            {
                findTargetMoveTo();
            }
        }
    }
}
