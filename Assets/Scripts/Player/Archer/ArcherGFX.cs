using UnityEngine;

public class ArcherGFX : PlayerAI
{
    [Header("Archer GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[ArcherGFX] Chưa gán 'SpriteRender'");
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
                if (!findTargetMoveTo())
                    if (!getIsLock() && !getDetect())
                        findItem();
            }
            farm();
            attack();
        }
    }
}
