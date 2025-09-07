using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class PlayerAI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float _helth = 100; // máu
    [SerializeField] public float _damage = 10; // sát thương
    [SerializeField] private float _maxSpeed = 6f; // tốc độ tối đa
    [SerializeField] private float _range = 1.5f; // tầm đánh

    [Header("AI")]
    [SerializeField] private Image _hpBar;
    [SerializeField] private GameObject _selet;
    private Vector3 _target;
    private Vector3 _targetPos;

    private float _speed = 600f; // lực đẩy
    private float _repathRate = 0.5f;
    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
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


    public void isSetSelcted(bool amount)
    {
        _selet.SetActive(amount);
    }

    public void ChangeTarget(Vector3 pos)
    {
        _target = pos;
    }

    private void setHPBar()
    {
        _hpBar.fillAmount = _helth / 100;
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
        if (dist < _range)
            _currentWaypoint++;


        // Lật sprite theo hướng di chuyển
        if (Mathf.Abs(_rb.linearVelocity.x) > 0.01f)
        {
            float sx = _rb.linearVelocity.x > 0 ? 1f : -1f;
            transform.localScale = new Vector3(sx, 1f, 1f);
        }
    }
}
