using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private float _bankinh = 5f;
    private CircleCollider2D col;
    private List<GameObject> chosen;
    private Coroutine _destroy;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = _bankinh + 0.5f;
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
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SetChosen(List<GameObject> _chonsen, bool add, float? _radius = null)
    {
        if (!add)
            chosen = _chonsen;
        else
            chosen.AddRange(_chonsen);

        if (_radius.HasValue) // chỉ update khi có truyền vào
        {
            _bankinh = _radius.Value;
            col.radius = _bankinh + 0.5f;
        }
    }

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

    private bool checkDistance()
    {
        if (chosen.Count != 0)
        {
            foreach (var linh in chosen)
            {
                if (Vector3.Distance(linh.transform.position, transform.position) < _bankinh)
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
                if (chosen.Contains(collision.gameObject))
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
