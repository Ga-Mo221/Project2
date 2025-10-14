using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    private float _defausRadius = 1.8f;
    [SerializeField] private float _radius = 0f;
    [SerializeField] private bool _active = true;
    [SerializeField] private bool _rally = false;
    [SerializeField] private bool _support = false;
    [SerializeField] private List<GameObject> chosen;
    

    private CircleCollider2D col;
    private Coroutine _destroy;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = _radius + 0.5f;
    }

    void Update()
    {
        checkPlayerToTarget();

        if (chosen.Count == 0 && _destroy == null)
            _destroy = StartCoroutine(destroy());
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
        MoveToManager.Instance.ReturnToPool(this);
    }


    public void SetChosen(List<GameObject> _chonsen, bool rally = false, bool add = false, bool active = true, float radius = -1, bool support = false)
    {
        _destroy = null;
        _active = active;
        _rally = rally;
        _support = support;

        if (!add && _chonsen != null && _chonsen.Count != 0)
            chosen = new List<GameObject>(_chonsen);
        else if (add && _chonsen != null && _chonsen.Count != 0)
            chosen.AddRange(new List<GameObject>(_chonsen));

        // chỉ update khi có truyền vào
        _radius = radius < 0 ? _defausRadius : radius + 0.5f;

        col.radius = _radius + 0.5f;
    }

    // trả về kết quả có còn ai đến vị trí chỉ định không
    private bool checkPlayerToTarget()
    {
        bool _isTarget = false;
        if (chosen.Count != 0)
        {
            // list dùng để lưu các player không đến target
            List<GameObject> _players = new List<GameObject>();

            foreach (var linh in chosen)
            {
                if (linh == null) continue; // check null
                var script = linh.GetComponent<PlayerAI>();
                if (script.getDie()) // check die
                {
                    _players.Add(linh);
                    continue;
                }

                if (_rally && !_support)
                    _radius = script._range;

                Vector3 _target = script.getTarget();
                float dist = Vector2.Distance(transform.position, _target);
                if (dist < _radius)
                    _isTarget = true;
                else
                {
                    _players.Add(linh);
                    continue;
                }

                dist = Vector2.Distance(linh.transform.position, transform.position);
                if (dist < _radius)
                {
                    if (!_support)
                        script.StopHere();
                    if (_active)
                    {
                        script.setIsAI(true);
                        script.setIsTarget(false);
                    }
                    _players.Add(linh);
                }
            }

            foreach (var linh in _players)
                chosen.Remove(linh);
        }
        return _isTarget;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _defausRadius);
    }
}
