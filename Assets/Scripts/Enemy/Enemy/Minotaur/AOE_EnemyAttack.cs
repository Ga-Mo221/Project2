using System.Collections.Generic;
using UnityEngine;

public class AOE_EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAi;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (checkTagPlayer(collision))
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

    private bool checkTagPlayer(Collider2D collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer" };
        return _tag.Contains(collision.tag);
    }
    private bool checkTagHouse(Collider2D collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(Collider2D collision)
        => collision.CompareTag("Animal");
}
