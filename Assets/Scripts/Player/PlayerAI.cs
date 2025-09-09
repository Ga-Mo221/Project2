using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using System.Collections;

public class PlayerAI : MonoBehaviour
{
    #region Value

    [Header("Stats")]
    [SerializeField] public float _helth = 100; // máu
    [SerializeField] public float _damage = 10; // sát thương
    [SerializeField] private float _maxSpeed = 6f; // tốc độ tối đa
    [SerializeField] private float _range = 1.5f; // tầm đánh
    [SerializeField] private float _attackSpeedd = 2.5f; // thời gian sau mỗi đồn đánh.

    [Header("AI Find Items")]
    [SerializeField] private float _radius = 10f; // bán kính phát hiện Items, Enemys, Animals
    [SerializeField] private float _radius_farm = 1.5f; // tầm farm
    [SerializeField] private float _farmSpeed = 1f; // thời gian sau mỗi đòn farm

    [Header("Inventory")]
    [SerializeField] public int _roock;
    private int _maxRoock = 10;

    [Header("GFX")]
    [SerializeField] private Image _hpBar; // image thanh máu
    [SerializeField] private GameObject _selet; // phát hiện đã được chọn

    [Header("Component")]
    [SerializeField] private Animator _anim; // animation của đối tượng

    private Vector3 _target; // vị trí chỉ định
    private Vector3 _targetPos; // biến tạm để lưu vị trí chỉ định dùng để so sánh và cập nhật path mới

    // bool
    [Header("Status")]
    [SerializeField] private bool _detectEnemy = false; // phát hiện kẻ địch
    [SerializeField] private bool _reachedEndOfPath = false; // đã đến path chưa
    [SerializeField] private bool _isLock = false; // khóa lại không cho về và tìm item dựa vào thành chính.
    [SerializeField] private bool _isAI = true; // để đối tượng được lựa chọn mục tiêu nhắm đến
    [SerializeField] private bool _isTarget = false; // có đang hướng đến mục tiêu nào hay không

    // float
    private float _speed = 600f; // lực đẩy
    private float _repathRate = 0.5f; // thời gian lặp lại tiềm đường

    // int
    private int _currentWaypoint = 0;

    // scrip
    private Item _itemScript; // dùng để tắt chọn đối với item.

    private Path _path; // đường
    private Seeker _seeker;
    private Rigidbody2D _rb;

    #endregion



    void Start()
    {
        if (!_anim)
            Debug.LogError($"[{gameObject.name}] [PlayerAI] Chưa Gán 'Animator'");

        _targetPos = _target;
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, _repathRate);
    }


    #region Update
    protected virtual void Update()
    {
        if (_isAI)
        {
            GameObject _nearest = GetNearestItem();
            if (_nearest != null && _nearest.tag != "Item" && _isTarget)
                _target = _nearest.transform.position;
            if (_nearest == null || _nearest.tag == "Item")
                setDetectEnemy(false);


            if (_roock < _maxRoock && !_isTarget)
            {
                if (_nearest != null)
                {
                    switch (_nearest.tag)
                    {
                        case "Item":
                            var _scrip = _nearest.GetComponent<Item>();
                            _scrip._seleted = true;
                            _itemScript = _scrip;
                            break;
                    }
                    _target = _nearest.transform.position;
                    Castle.Instance._canFind = true;
                    setIsTarget(true);
                }
                else
                {
                    if (!_isLock && !_detectEnemy)
                        findItem();
                }
            }
            farm();
            Attack();
        }
    }
    #endregion


    #region Create Path
    private void UpdatePath()
    {
        if (_seeker.IsDone())
            if (!_reachedEndOfPath || _targetPos != _target)
                _seeker.StartPath(_rb.position, _target, OnPathComplete);
    }
    #endregion


    #region Set Selected
    public void isSetSelected(bool amount)
    {
        _selet.SetActive(amount);
    }
    #endregion


    #region Set Target
    public void setTarget(Vector3 pos)
    {
        if (_itemScript != null)
        {
            _itemScript._seleted = false;
            _itemScript = null;
        }
        setIsAI(false);
        _isTarget = true;
        _target = pos;
    }
    public Vector3 getTarget() => _target;
    #endregion


    #region HP Bar Sprite
    private void setHPBar()
    {
        _hpBar.fillAmount = _helth / 100;
    }
    #endregion


    #region Attack
    private Coroutine _attackSpeed;
    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _range);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Animal"))
            {
                if (_attackSpeed == null)
                    _attackSpeed = StartCoroutine(attackSpeed());
            }
            else if (hit.CompareTag("Enemy"))
            {
                if (_attackSpeed == null)
                    _attackSpeed = StartCoroutine(attackSpeed());
            }
        }
    }
    private IEnumerator attackSpeed()
    {
        _anim.SetTrigger("attack");
        yield return new WaitForSeconds(_attackSpeedd);
        _attackSpeed = null;
    }
    #endregion


    #region Farm
    private Coroutine _farmCoroutine;
    private void farm()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius_farm);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Item"))
            {
                var _script = hit.gameObject.GetComponent<Item>();
                if (_farmCoroutine == null)
                    _farmCoroutine = StartCoroutine(farmTrigger(_script));
            }
        }
    }
    private IEnumerator farmTrigger(Item _script)
    {
        ItemType type = _script._type;
        if (_script._stack > 0 && _script._seleted)
        {
            if (type == ItemType.Tree || type == ItemType.Pumpkin)
            {
                _anim.SetInteger("TypeFarm", 1);
                _anim.SetTrigger("Farm");
            }
            else if (type == ItemType.Iron || type == ItemType.Gold)
            {
                _anim.SetInteger("TypeFarm", 2);
                _anim.SetTrigger("Farm");
            }
        }
        else if (_script._stack <= 0 && _script._seleted)
        {
            setIsTarget(false);
            _script._seleted = false;
        }
        yield return new WaitForSeconds(_farmSpeed);
        _farmCoroutine = null;
    }
    #endregion


    #region AI Find Item
    private GameObject GetNearestItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Item"))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                var _script = hit.gameObject.GetComponent<Item>();
                if (!_script._seleted && _script._stack > 0)
                {
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = hit.gameObject;
                    }
                }
            }
            else if (hit.CompareTag("Animal"))
            {
                setDetectEnemy(true);
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                var _script = hit.gameObject.GetComponent<Item>();
                // if (!_script._seleted && _script._stack > 0)
                // {
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.gameObject;
                }
                //}
            }
            else if (hit.CompareTag("Enemy"))
            {
                setDetectEnemy(true);
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                var _script = hit.gameObject.GetComponent<Item>();
                // if (!_script._seleted && _script._stack > 0)
                // {
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.gameObject;
                }
                //}
            }
        }
        return nearest; // null nếu không có item nào trong bán kính
    }
    private void findItem()
    {
        if (Castle.Instance._canFind)
        {
            Castle.Instance._canFind = false;
            foreach (var item in Castle.Instance._allItems)
            {
                var _scrip = item.GetComponent<Item>();
                if (!_scrip._seleted && _scrip._stack > 0)
                {
                    _scrip._seleted = true;
                    Castle.Instance._canFind = true;
                    setIsTarget(true);
                    _itemScript = _scrip;
                    _target = _scrip.transform.position;
                    return;
                }
            }
            Castle.Instance._canFind = true;
        }
    }
    #endregion


    #region Reset Way Value
    private void OnPathComplete(Path p)
    {
        if (p == null) return;
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
        else
            Debug.LogWarning("Path error: " + p.error);
    }
    #endregion


    #region Find the way
    void FixedUpdate()
    {
        if (_path == null) return;

        if (_reachedEndOfPath)
        {
            _rb.linearVelocity = Vector2.zero; // dừng máy
            _anim.SetBool("Moving", !_reachedEndOfPath);
        }
        else
            _anim.SetBool("Moving", !_reachedEndOfPath);

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            if (!_reachedEndOfPath)
            {
                _reachedEndOfPath = true;
                _rb.linearVelocity = Vector2.zero; // dừng máy
                _targetPos = _target;
            }
            return;
        }

        _reachedEndOfPath = false;

        Vector2 waypoint = (Vector2)_path.vectorPath[_currentWaypoint];
        Vector2 dir = (waypoint - _rb.position).normalized;

        // Dùng fixedDeltaTime trong FixedUpdate
        Vector2 force = dir * _speed * Time.fixedDeltaTime;
        _rb.AddForce(force, ForceMode2D.Force);

        // Giới hạn tốc độ (nếu dùng AddForce)
        if (_rb.linearVelocity.magnitude > _maxSpeed)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;

        float dist = Vector2.Distance(_rb.position, waypoint);
        if (dist < (_range - 0.8f))
            _currentWaypoint++;


        // Lật sprite theo hướng di chuyển
        if (Mathf.Abs(_rb.linearVelocity.x) > 0.01f)
        {
            float sx = _rb.linearVelocity.x > 0 ? 1f : -1f;
            transform.localScale = new Vector3(sx, 1f, 1f);
        }
    }
    #endregion


    #region Draw
    private void OnDrawGizmosSelected()
    {
        // find
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);

        // farm
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius_farm);
        // attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    #endregion



    #region Set funsiton
    // Target
    public void setIsTarget(bool amount) => _isTarget = amount;
    public bool getIsTarget() => _isTarget;

    // AI
    public void setIsAI(bool amount) => _isAI = amount;
    public bool getIsAI() => _isAI;

    // Lock
    public void setIsLock(bool amount) => _isLock = amount;
    public bool getIsLock() => _isLock;

    // Dectec Enemy
    public void setDetectEnemy(bool amount)
    {
        _detectEnemy = amount;
        _anim.SetBool("Detectenemy", amount);
    }
    public bool getDetectEnemy() => _detectEnemy;
    #endregion
}
