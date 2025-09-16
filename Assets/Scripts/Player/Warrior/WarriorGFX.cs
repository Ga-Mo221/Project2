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
        _spriteRender.sortingOrder = -(int)(_oderSpriterPoint.position.y * 100);
        base.Update();

        if (!getIsAI()) return;
        if (target == null)
        {
            target = findEnemys();
            if (target != null)
            {
                setDetect(true);
                moveToTarget(target);
            }
        }
        if (target == null)
        {
            target = findAnimals();
            if (target != null)
            {
                setDetect(true);
                moveToTarget(target);
            }
        }
        if (target == null)
        {
            target = findItems();
            if (target != null)
            {
                setDetect(false);
                moveToTarget(target);
                _itemScript = target.GetComponent<Item>();
                if (_itemScript._type != ItemType.Gold)
                    _itemScript._seleted = true;
                else
                {
                    if (_itemScript._Farmlist.Count < _itemScript._maxFarmers)
                    {
                        bool see = false;
                        foreach (var hit in _itemScript._Farmlist)
                            if (hit == this)
                                see = true;
                        if (see)
                            _itemScript._Farmlist.Add(this);
                    }
                }
            }
        }

        goToHome(target);
        if (target == null) return;
        farm(target);
        attack(target);
    }

    public void offDetec()
        => setDetect(false);
    public void onDetec()
        => setDetect(true);
}
