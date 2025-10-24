using UnityEngine;

public class WarriorGFX : PlayerAI
{
    [Header("Warrior GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private SpriteRenderer _select;
    [SerializeField] private Transform _oderSpriterPoint;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[WarriorGFX] Chưa gán 'SpriteRender'");
        if (!_oderSpriterPoint)
            Debug.LogError("[WarriorGFX] Chưa gán '_oderSpriterPoint'");

        addCastle(Castle.Instance._ListWarrior);
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

        if (getCallSupport()) return;

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
        else if (cacheTarget == null)
        {
            find();
        }

        goToHome(target);
        setupFolow(target);
        if (cacheTarget != null)
        {
            target = cacheTarget;
            farm(target);
        }
        if (target == null) return;
        attack(target);
    }


    private void find()
    {
        cacheTarget = findItems();
        target = cacheTarget;
        if (target != null && _itemScript == null)
        {
            setDetect(false);
            moveToTarget(target, true);
            _itemScript = target.GetComponent<Item>();
            if (_itemScript._type != ItemType.Gold)
            {
                if (!_itemScript.add(this))
                    return;
            }
            else
            {
                if (_itemScript._Farmlist.Count < _itemScript._maxFarmers)
                {
                    bool exists = false;
                    foreach (var hit in _itemScript._Farmlist)
                    {
                        if (hit == this)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        _itemScript._Farmlist.Add(this);
                }
            }
        }
    }

    public void offDetec()
        => setDetect(false);
    public void onDetec()
        => setDetect(true);
}
