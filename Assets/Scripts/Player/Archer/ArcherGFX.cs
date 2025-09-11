using UnityEngine;

public class ArcherGFX : PlayerAI
{
    [Header("Archer GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private Transform _shootPos;

    [Header("Arrow")] 
    [SerializeField] private GameObject _arrowPrefab;
    private GameObject _targets;
    [SerializeField] private GameObject[] _listArrow = new GameObject[10];

    // [Header("SKILL")]
    // [SerializeField] private int _attack_count_SKILL = 10;
    // [SerializeField] private int _attackCount = 0;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[ArcherGFX] Chưa gán 'SpriteRender'");
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100);
        _targets = flip();
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

    public void spawnArrow()
    {
        for (int i = 0; i < 10; i++)
        {
            if (_listArrow[i] != null)
            {
                var _script = _listArrow[i].GetComponent<Arrow>();
                if (_script.getTarget() == null)
                {
                    _listArrow[i].transform.position = _shootPos.position;
                    _script.setTarget(_targets.transform, _damage);
                    _listArrow[i].SetActive(true);
                    break;
                }
                else
                {
                    GameObject _arrow = Instantiate(_arrowPrefab, _shootPos.position, Quaternion.identity, _shootPos);
                    _listArrow[i + 1] = _arrow;
                    var _scripta = _arrow.GetComponent<Arrow>();
                    _scripta.setTarget(_targets.transform, _damage);
                    break;
                }
            }
            else if (_listArrow[i] == null)
            {
                GameObject _arrow = Instantiate(_arrowPrefab, _shootPos.position, Quaternion.identity, _shootPos);
                _listArrow[i] = _arrow;
                var _script = _arrow.GetComponent<Arrow>();
                _script.setTarget(_targets.transform, _damage);
                break;
            }
        }
    }
}
