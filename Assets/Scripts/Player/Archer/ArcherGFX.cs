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
    [SerializeField] private Transform _oderSpriterPoint;
    [SerializeField] private Transform _shootPos;
    [SerializeField] private Transform _shootStarge;

    [Header("Arrow")]
    [SerializeField] private GameObject _arrowPrefab;
    //private GameObject _targets;
    [SerializeField] private GameObject[] _listArrow = new GameObject[10];

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
        _spriteRender.sortingOrder = -(int)(_oderSpriterPoint.position.y * 100) + 10000;
        base.Update();
    }

    public override void Ai()
    {
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
        for (int i = 0; i < _listArrow.Length; i++) // hoặc _listArrow.Count
        {
            if (_listArrow[i] != null)
            {
                var _script = _listArrow[i].GetComponent<Arrow>();

                if (_script.getTarget() == null)
                {
                    _listArrow[i].transform.position = _shootPos.position;
                    if (_attackCount == _attack_count_SKILL)
                    {
                        _script.setTarget(true, this, true, _damage, 0, transform.localScale);
                        _attackCount = 0;
                    }
                    else
                        _script.setTarget(true, this, false, _damage, 0, transform.localScale);
                    _listArrow[i].SetActive(true);
                    break;
                }
            }
            else
            {
                GameObject _arrow = Instantiate(_arrowPrefab, _shootPos.position, Quaternion.identity, _shootStarge);
                _listArrow[i] = _arrow;
                var _script = _arrow.GetComponent<Arrow>();
                if (_attackCount == _attack_count_SKILL)
                {
                    _script.setTarget(true, this, true, _damage, 0, transform.localScale);
                    _attackCount = 0;
                }
                else
                    _script.setTarget(true, this, false, _damage, 0, transform.localScale);
                break;
            }
        }
    }
}
