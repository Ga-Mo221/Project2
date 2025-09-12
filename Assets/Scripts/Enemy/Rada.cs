using NaughtyAttributes;
using UnityEngine;

public enum Type
{
    Item,
    Animal,
    Enemy,
    EnemyHouse
}

public class Rada : MonoBehaviour
{
    [SerializeField] private Type _type;
    private bool IsItem => _type == Type.Item;
    // private bool IsAnimal => _type == Type.Animal;
    // private bool IsEnemy => _type == Type.Enemy;
    // private bool IsEnemyHouse => _type == Type.EnemyHouse;

    [SerializeField] private float _radius = 30f;

    [SerializeField] private SpriteRenderer _GFX;
    [SerializeField] private GameObject _outLine;

    [ShowIf(nameof(IsItem))]
    [SerializeField] private Item _item;


    void Start()
    {
        if (!_GFX)
            Debug.LogError("[Rada] Chưa gán 'SpriteRenderer GFX'");
        if (!_outLine)
            Debug.LogError("[Rada] Chưa gán 'GameObject _outLine'");
        _GFX.enabled = false;

        if (IsItem)
            if (!_item)
                Debug.LogError("[Rada] Chưa gán 'Item _item'");
    }

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Warrior")
            || hit.CompareTag("Archer")
            || hit.CompareTag("Lancer")
            || hit.CompareTag("Healer")
            || hit.CompareTag("TNT")
            || hit.CompareTag("House"))
            {
                _GFX.enabled = true;
                _outLine.SetActive(true);
                if (IsItem)
                    _item._detec = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
