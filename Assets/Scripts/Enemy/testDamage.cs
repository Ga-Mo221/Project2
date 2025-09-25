using System.Collections.Generic;
using UnityEngine;

public class testDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (checkTagPlayer(collision))
        {
            var health = collision.GetComponent<PlayerHealth>();
            health.takeDamage(30);
        }
        if (checkTagHouse(collision))
        {
            var health = collision.GetComponent<HouseHealth>();
            if (health.getCanDetec())
                health.takeDamage(30);
        }
    }

    private bool checkTagPlayer(Collider2D collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer"};
        return _tag.Contains(collision.tag);
    }
    private bool checkTagHouse(Collider2D collision)
    {
        return collision.CompareTag("House");
    }
}
