using System.Collections.Generic;
using UnityEngine;

public class AOE_EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAi;
    private CircleCollider2D _col;

    void Awake()
    {
        _col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        if (_col != null)
            _col.radius = enemyAi._range;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (PlayerTag.checkTag(collision.tag))
        {
            var health = collision.GetComponent<PlayerHealth>();
            if (health != null)
                health.takeDamage(enemyAi._damage);
        }
        if (checkTagHouse(collision))
        {
            var health = collision.GetComponent<HouseHealth>();
            if (health != null && health.getCanDetec())
                health.takeDamage(enemyAi._damage);
        }
        if (checkTagAnimal(collision))
        {
            var animalHealth = collision.GetComponent<AnimalHealth>();
            if (animalHealth != null && !animalHealth._animalAi.getDie())
                animalHealth.takeDamage(enemyAi._damage, gameObject);
        }
    }

    private bool checkTagHouse(Collider2D collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(Collider2D collision)
        => collision.CompareTag("Animal");
}
