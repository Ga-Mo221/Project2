using UnityEngine;
using Pathfinding;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] Vector3 _target;
    private Vector3 _targetPos;
    [SerializeField] float _speed = 300f;
    [SerializeField] float _nextWaypointDistance = 3f; // nhỏ hơn để chính xác hơn
    [SerializeField] float _repathRate = 0.5f;
    [SerializeField] float _maxSpeed = 6f; // units/sec - điều chỉnh theo game

    private Path _path;
    public int _currentWaypoint = 0;
    public bool _reachedEndOfPath = false;
    private Seeker _seeker;
    private Rigidbody2D _rb;

    void Start()
    {
        _targetPos = _target;
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, _repathRate);
    }


    private void UpdatePath()
    {
        if (_seeker.IsDone())
            if (!_reachedEndOfPath || _targetPos != _target)
                _seeker.StartPath(_rb.position, _target, OnPathComplete);
    }

    public void ChangeTarget(Vector3 pos)
    {
        Debug.Log("doi thanh cong");
        _target = pos;
    }


    private void OnPathComplete(Path p)
    {
        if (p == null) return;
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
        else
        {
            Debug.LogWarning("Path error: " + p.error);
        }
    }


    void FixedUpdate()
    {
        if (_path == null) return;

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
        if (dist < _nextWaypointDistance)
            _currentWaypoint++;


        // Lật sprite theo hướng di chuyển
        if (Mathf.Abs(_rb.linearVelocity.x) > 0.01f)
        {
            float sx = _rb.linearVelocity.x > 0 ? 1f : -1f;
            transform.localScale = new Vector3(sx, 1f, 1f);
        }
    }
}
