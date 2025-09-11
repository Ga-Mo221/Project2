using UnityEngine;

public class WarriorGFX : PlayerAI
{
    [Header("Warrior GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[WarriorGFX] Chưa gán 'SpriteRender'");
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

    public void offCanAttack()
        => _canAttack = false;
    public void onCanAttack()
        => _canAttack = true;
}
