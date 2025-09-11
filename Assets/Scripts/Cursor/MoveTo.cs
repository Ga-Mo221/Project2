using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private float _bankinh = 5f;
    private List<GameObject> chosen;
    private Coroutine _destroy;

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

    public void SetChosen(List<GameObject> _chonsen)
    {
        chosen = _chonsen;
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
}
