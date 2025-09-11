using UnityEngine;

public class HealerGFX : PlayerAI
{
    [Header("Healer GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[HealerGFX] Chưa gán 'SpriteRender'");
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
