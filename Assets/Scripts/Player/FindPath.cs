using Pathfinding;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _range;
    [SerializeField] private bool _detect;
    [SerializeField] private Vector3 _target;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private bool _reachedEndOfPath = false; // đã đến path chưa

    private Path _path; // đường
    public Seeker _seeker { get; set; }
    private Rigidbody2D _rb;
    [SerializeField] private Animator _anim;

    // float
    private float _speed = 600f; // lực đẩy

    // int
    private int _currentWaypoint = 0;


    void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        if (!_anim)
            Debug.LogError($"[{gameObject.name}] [FindPath] Chưa Gán 'Animator'");
        //_targetPos = _target;
        _targetPos = Vector3.zero;
    }

    public void setTarget(Vector3 target) => _target = target;
    public Vector3 getTarget() => _target;
    public void setDetec(bool detec) => _detect = detec;
    public void setCurrentWaypoint(int currentWaypoint) => _currentWaypoint = currentWaypoint;
    public void setReachedEndOfPath(bool reachedEndOfPath) => _reachedEndOfPath = reachedEndOfPath;
    public void setPropety(float maxSpeed, float range)
    {
        _maxSpeed = maxSpeed;
        _range = range;
    }

    #region Create Path
    public void UpdatePath()
    {
        if (!_reachedEndOfPath || _targetPos != _target)
            _seeker.StartPath(_rb.position, _target, OnPathComplete);
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
        float edg = _detect ? _range - 0.8f : 1.5f;
        if (dist < edg)
            _currentWaypoint++;
    }
    #endregion
}
