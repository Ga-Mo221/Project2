using UnityEngine;

public class WarriorGFX : PlayerAI
{
    [Header("Warrior GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private Transform _oderSpriterPoint;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[WarriorGFX] Chưa gán 'SpriteRender'");
        if (!_oderSpriterPoint)
            Debug.LogError("[WarriorGFX] Chưa gán '_oderSpriterPoint'");
    }

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(_oderSpriterPoint.position.y * 100) + 10000;
        base.Update();

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
        else if (cacheTarget == null)
        {
            cacheTarget = findItems();
            target = cacheTarget;
            if (target != null && _itemScript == null)
            {
                setDetect(false);
                moveToTarget(target, true);
                _itemScript = target.GetComponent<Item>();
                if (_itemScript._type != ItemType.Gold)
                    _itemScript._seleted = true;
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

    public void offDetec()
        => setDetect(false);
    public void onDetec()
        => setDetect(true);
}
