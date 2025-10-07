using System.Collections.Generic;
using UnityEngine;

public class Rada : MonoBehaviour
{
    public float _radius = 20f;
    [SerializeField] private Collider2D[] hits;
    [SerializeField] private bool _on = true;
    [SerializeField] private bool _Die = false;

    void Start()
    {
        InvokeRepeating(nameof(display), 0.2f, 0.2f);
    }

    void OnDisable()
    {
        CleanupSeemer();
    }

    void OnDestroy()
    {
        CleanupSeemer();
    }

    private void CleanupSeemer()
    {
        if (hits == null) return;

        foreach (var hit in hits)
        {
            if (hit == null) continue; // collider đã destroy thì bỏ qua
            if (!checkTag(hit)) continue;

            var script = hit.GetComponent<Display>();
            if (script == null) continue;

            if (script.checkSeemer(this))
            {
                script.removeSeemer(this);
            }
        }
    }


    private void display()
    {
        if (!_on) return;
        hits = Physics2D.OverlapCircleAll(transform.position, _radius + 3);

        foreach (var hit in hits)
        {
            if (!checkTag(hit)) continue;

            var script = hit.GetComponent<Display>();
            if (script == null) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            bool isSeeing = script.checkSeemer(this);

            if (dist < _radius)
            {
                if (!isSeeing)
                    script.addSeemer(this);
            }
            else
            {
                if (isSeeing)
                {
                    script.removeSeemer(this);
                }
            }
        }
    }

    private bool checkTag(Collider2D hit)
    {
        List<string> tags = new List<string> { "Item+", "Animal", "Enemy", "Buiding", "Deco", "EnemyHouse" };
        return tags.Contains(hit.tag);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void setOn(bool amount) => _on = amount;

    public void setDie(bool amount) => _Die = amount;
    public bool getDie() => _Die;
}
