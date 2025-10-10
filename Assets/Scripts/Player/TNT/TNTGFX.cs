using UnityEngine;

public class TNTGFX : PlayerAI
{
    [Header("TNT GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private Transform _oderSpriterPoint;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[TNTGFX] Chưa gán 'SpriteRender'");
        if (!_oderSpriterPoint)
            Debug.LogError("[TNTGFX] Chưa gán '_oderSpriterPoint'");

        addCastle(Castle.Instance._ListTNT);
    }

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(_oderSpriterPoint.position.y * 100) + 10000;
        base.Update();
    }

    public override void Ai()
    {
        base.Ai();
        if (!getIsAI()) return;
        target = findEnemys();
        if (target == null)
        {
            target = findAnimals();
            if (target == null)
                target = findEnemyHouse();
        }
        if (target != null)
        {
            setDetect(true);
            moveToTarget(target);
        }
        setupFolow(target);
    }
}
