using System.Collections;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _range;
    [SerializeField] private bool _detect;
    private float _waypointTolerance = 1.5f; // khoảng cách coi như đã đến waypoint
    [SerializeField] private GameObject cacheobj;
    [SerializeField] private Vector3 _target;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private Vector3 _cacheTarget;
    [SerializeField] private bool _reachedEndOfPath = false; // đã đến path chưa
    [SerializeField] private bool _Die = false;
    [SerializeField] private bool _canMove = true;

    private Path _path; // đường
    public Seeker _seeker { get; set; }
    private Rigidbody2D _rb;
    [SerializeField] private Animator _anim;

    // float
    private float _force = 600f; // lực đẩy

    // int
    private int _currentWaypoint = 0;


    void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        
    }

    Vector3 RandomPosAround(Transform center, float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return center.position + new Vector3(randomCircle.x, randomCircle.y, 0f);
    }


    void Start()
    {
        if (!_anim)
            Debug.LogError($"[{gameObject.name}] [FindPath] Chưa Gán 'Animator'");
        //_targetPos = _target;
        _target = RandomPosAround(transform, 3);
        _targetPos = Vector3.zero;
    }

    #region API
    public void setTarget(Vector3 target, GameObject obj)
    {
        if (target != _cacheTarget)
        {
            _target = target;
            //_reachedEndOfPath = false;
            if (gameObject.activeInHierarchy)
                StartCoroutine(onpath());
            _cacheTarget = target;
            if (cacheobj != obj)
            {
                UpdatePath();
                cacheobj = obj;
            }
        }
    }
    private IEnumerator onpath()
    {
        yield return new WaitForSeconds(0.5f);
        _reachedEndOfPath = false;
    }
    

    public void setDetec(bool detect) => _detect = detect;
    public void setPropety(float maxSpeed, float range)
    {
        _maxSpeed = maxSpeed;
        _range = range;
    }
    #endregion

    public void setDie(bool amount) => _Die = amount;
    public void setCanMove(bool amount) => _canMove = amount;

    public void setTargetPos(Vector3 targetPos) => _targetPos = targetPos;
    public Vector3 getTargetPos() => _targetPos;
    public Vector3 getTarget() => _target;
    public void setCurrentWaypoint(int currentWaypoint) => _currentWaypoint = currentWaypoint;
    public void setReachedEndOfPath(bool reachedEndOfPath) => _reachedEndOfPath = reachedEndOfPath;


    #region Pathfinding
    public void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(_rb.position, _target, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.LogWarning($"[{gameObject.name}] Path error: {p.error}");
            return;
        }

        _path = p;
        _currentWaypoint = 0;
    }
    #endregion
    
    #region Movement
    private void FixedUpdate()
    {
        if (_path == null) return;

        // Nếu đã đến đích
        if (_reachedEndOfPath)
        {
            _rb.linearVelocity = Vector2.zero;
            if (_anim) _anim.SetBool("Moving", false);
            return;
        }

        if (_anim) _anim.SetBool("Moving", true);

        // Nếu vượt quá waypoint cuối
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            _rb.linearVelocity = Vector2.zero;
            _targetPos = _target;
            return;
        }

        Vector2 waypoint = (Vector2)_path.vectorPath[_currentWaypoint];
        Vector2 dir = (waypoint - _rb.position).normalized;

        // Dùng fixedDeltaTime trong FixedUpdate
        Vector2 force = dir * _force * Time.fixedDeltaTime;
        if (!_Die && _canMove)
            _rb.AddForce(force, ForceMode2D.Force);

        // Giới hạn tốc độ (nếu dùng AddForce)
        if (_rb.linearVelocity.magnitude > _maxSpeed)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;

        float dist = Vector2.Distance(_rb.position, waypoint);
        float edg = _detect ? _range - 1.2f : _waypointTolerance;
        if (dist < edg)
            _currentWaypoint++;
    }
    #endregion
}