using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private float _bankinh = 5f;
    private List<GameObject> chosen;

    void Update()
    {
        if (!checkMoveTo())
        {
            Destroy(gameObject);
        }
        if (checkDistance())
        {
            Destroy(gameObject);
        }
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
            if (collision.CompareTag("Warrior"))
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
