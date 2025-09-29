using UnityEngine;

public class Rada : MonoBehaviour
{
    [SerializeField] private float _radius = 20f;
    [SerializeField] private Collider2D[] hits;
    [SerializeField] private bool _on = true;

    void Update()
    {
        display();
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
            bool isSeeing = script._seemer.Contains(this);

            if (dist < _radius)
            {
                if (!isSeeing)
                    script._seemer.Add(this);

                if (!script._Detec)
                {
                    if (script._IsTree) script.onDisplayTree();
                    if (script._IsRock) script.onDisplayRock();
                    if (script._IsGold) script.onDisplayGoldMine();
                    if (script._IsAnimal) script.onDisplayAnimal();
                    if (script._IsBuiding) script.onCanCreate();
                    if (script._IsEnemy) script.onDisplayEnemy();
                    if (script._IsDeco) script.onDisplayDeco();
                }
            }
            else
            {
                if (isSeeing)
                {
                    script._seemer.Remove(this);

                    if (script._Detec && script._seemer.Count == 0)
                    {
                        if (script._IsTree) script.offDisplayTree();
                        if (script._IsRock) script.offDisplayRock();
                        if (script._IsAnimal) script.offDisplayAnimal();
                        if (script._IsGold) script.offDisplayGoldMine();
                        if (script._IsBuiding) script.offCanCreate();
                        if (script._IsEnemy) script.offDisplayEnemy();
                        if (script._IsDeco) script.offDisplayDeco();
                    }
                }
            }
        }
    }

    private bool checkTag(Collider2D hit)
    {
        return hit.CompareTag("Item+") || hit.CompareTag("Animal") || hit.CompareTag("Enemy") || hit.CompareTag("Buiding") || hit.CompareTag("Deco");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void setOn(bool amount) => _on = amount;
}
