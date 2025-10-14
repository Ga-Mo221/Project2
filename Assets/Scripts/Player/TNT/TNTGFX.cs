using UnityEngine;

public class TNTGFX : PlayerAI
{
    [Header("TNT GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private SpriteRenderer _select;
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
        int _yOder = getOderInLayer();
        _spriteRender.sortingOrder = _yOder;
        _select.sortingOrder = _yOder - 1;
        base.Update();
    }

    public override int getOderInLayer()
        => -(int)(_oderSpriterPoint.position.y * 100) + 10000;

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
