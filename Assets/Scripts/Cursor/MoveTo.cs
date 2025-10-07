using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private float _bankinh = 5f;
    [SerializeField] private float _currentBanKinh = 0f;
    private CircleCollider2D col;
    [SerializeField] private List<GameObject> chosen;
    private Coroutine _destroy;
    [SerializeField] private bool _active = true;
    [SerializeField] private UnitAudio _audio;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = _bankinh + 0.5f;
        _currentBanKinh = _bankinh;
    }

    void Update()
    {
        if (!checkMoveTo())
        {
            if (_destroy == null)
                _destroy = StartCoroutine(destroy());
        }
        if (checkDistance())
        {
            if (_destroy == null)
                _destroy = StartCoroutine(destroy());
        }
        if (checkDie())
        {
            if (_destroy == null)
                _destroy = StartCoroutine(destroy());
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
        MoveToManager.Instance.ReturnToPool(this);
    }

    public void SetChosen(List<GameObject> _chonsen, bool add, float _radius = -1)
    {
        _destroy = null;
        _active = true;
        if (!add)
            chosen = _chonsen;
        else
            chosen.AddRange(_chonsen);

        if (_radius > 0) // chỉ update khi có truyền vào
        {
            _bankinh = _radius;
            col.radius = _bankinh + 0.5f;
        }
        else
        {
            _bankinh = _currentBanKinh;
            col.radius = _bankinh + 0.5f;
        }
        _audio.PlayMoveToSound();
    }

    public void offActive() => _active = false;


    // trả về kết quả có còn ai đến vị trí chỉ định không
    private bool checkMoveTo()
    {
        if (chosen.Count != 0)
        {
            foreach (var linh in chosen)
            {
                Vector3 _target = linh.GetComponent<PlayerAI>().getTarget();
                if (_target.x == transform.position.x && _target.y == transform.position.y)
                {
                    return true;
                }
            }
        }
        return false;
    }


    // check co ai chet khong
    private bool checkDie()
    {
        if (chosen.Count != 0)
        {
            foreach (var linh in chosen)
            {
                if (linh.activeSelf)
                    return false;
            }
        }
        return true;
    }

    private bool checkDistance()
    {
        if (chosen.Count != 0)
        {
            foreach (var linh in chosen)
            {
                float radius = _active ? _bankinh : 2.5f;
                float dist = Vector2.Distance(linh.transform.position, transform.position);
                if (dist < radius)
                {
                    return true;
                }
            }
        }
        return false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Warrior")
            || collision.CompareTag("Archer")
            || collision.CompareTag("Lancer")
            || collision.CompareTag("Healer")
            || collision.CompareTag("TNT"))
            {
                if (chosen.Contains(collision.gameObject) && _active)
                {
                    collision.GetComponent<PlayerAI>().setIsAI(true);
                    collision.GetComponent<PlayerAI>().setIsTarget(false);
                }
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _bankinh);
    }
}
