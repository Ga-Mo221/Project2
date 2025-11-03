using System.Collections.Generic;
using UnityEngine;

public enum UpDirection
{
    Right,
    Center,
    Left,
    Tower
}

public class ArcherGFX : PlayerAI
{
    [Header("Archer GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private SpriteRenderer _select;
    [SerializeField] private Transform _oderSpriterPoint;
    [SerializeField] private Transform _shootPos;
    [SerializeField] private Transform _shootStarge;

    [Header("Arrow")]
    [SerializeField] private GameObject _arrowPrefab;
    //private GameObject _targets;
    [SerializeField] private List<GameObject> _listArrow = new List<GameObject>();

    [Header("SKILL")]
    [SerializeField] private int _attack_count_SKILL = 5;
    [SerializeField] public bool _In_Castle = false;
    public UpDirection _upDirection;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[ArcherGFX] Chưa gán 'SpriteRender'");
        if (!_oderSpriterPoint)
            Debug.LogError("[ArcherGFX] Chưa gán '_oderSpriterPoint'");
        if (!_shootPos)
            Debug.LogError("[ArcherGFX] Chưa gán '_shootPos'");
        if (!_arrowPrefab)
            Debug.LogError("[ArcherGFX] Chưa gán '_arrowPrefab'");

        addCastle(Castle.Instance._ListArcher);
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

    public override void setTarget(Vector3 pos, bool controller, bool farm = false)
    {
        base.setTarget(pos, controller, farm);
        _In_Castle = false;
    }

    public void spawnArrow()
    {
        _attackCount++;

        bool _has = true;
        foreach (var arrow in _listArrow)
        {
            if (arrow != null && !arrow.activeSelf)
            {
                var _script = arrow.GetComponent<Arrow>();
                if (_attackCount == _attack_count_SKILL)
                {
                    _script.setTarget(true, this, true, getDamage(), 0, transform.localScale);
                    _attackCount = 0;
                }
                else
                    _script.setTarget(true, this, false, getDamage(), 0, transform.localScale);

                arrow.transform.position = _shootPos.position;
                arrow.SetActive(true);
                _has = false;
                break;
            }
        }

        if (_has)
        {
            GameObject _arrow = Instantiate(_arrowPrefab, _shootPos.position, Quaternion.identity, _shootStarge);
            _listArrow.Add(_arrow);
            var _script = _arrow.GetComponent<Arrow>();
            if (_attackCount == _attack_count_SKILL)
            {
                _script.setTarget(true, this, true, getDamage(), 0, transform.localScale);
                _attackCount = 0;
            }
            else
                _script.setTarget(true, this, false, getDamage(), 0, transform.localScale);
        }
    }
}
