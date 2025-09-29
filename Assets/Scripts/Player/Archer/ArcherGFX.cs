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
    }

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(_oderSpriterPoint.position.y * 100) + 10000;
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
        setupFolow(target);
        if (target == null) return;
        farm(target);
        attack(target);
    }

    public override void setTarget(Vector3 pos, bool controller)
    {
        base.setTarget(pos, controller);
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
                        _script.setTarget(target.transform, true, _damage);
                        _attackCount = 0;
                    }
                    else
                        _script.setTarget(target.transform, false, _damage);
                    _listArrow[i].SetActive(true);
                    break;
                }
            }
            else
            {
                GameObject _arrow = Instantiate(_arrowPrefab, _shootPos.position, Quaternion.identity, _shootPos);
                _listArrow[i] = _arrow;
                var _script = _arrow.GetComponent<Arrow>();
                if (_attackCount == _attack_count_SKILL)
                {
                    _script.setTarget(target.transform, true, _damage);
                    _attackCount = 0;
                }
                else
                    _script.setTarget(target.transform, false, _damage);
                break;
            }
        }
    }
}
