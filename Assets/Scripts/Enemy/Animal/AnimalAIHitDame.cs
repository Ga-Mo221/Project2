using System.Collections.Generic;
using UnityEngine;

public class AnimalAIHitDame : MonoBehaviour
{
    [SerializeField] private AnimalAI enimalAi;
    [SerializeField] private GameObject MeatOBJ;
    public void attack()
    {
        GameObject obj = enimalAi.target;
        if (obj == null) return;

        if (checkTagPlayer(obj))
        {
            var health = obj.GetComponent<PlayerHealth>();
            health.takeDamage(enimalAi._damage);
        }
        if (checkTagEnemy(obj))
        {
             var health = obj.GetComponent<EnemyHealth>();
            health.takeDamage(enimalAi._damage);
        }
       
    }

    private bool checkTagPlayer(GameObject collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer" };
        return _tag.Contains(collision.tag);
    }
    private bool checkTagEnemy(GameObject collision)
    {
        return collision.CompareTag("Enemy");
    }

    public void openMeat()
    {
        MeatOBJ.SetActive(true);
    }
   
}
