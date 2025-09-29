using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDamage : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAi;
    [SerializeField] private GameObject MeatOBJ;
    [SerializeField] private GameObject GoldOBJ;

    public void attack()
    {
        GameObject obj = enemyAi.target;
        if (obj == null) return;

        if (checkTagPlayer(obj))
        {
            var health = obj.GetComponent<PlayerHealth>();
            health.takeDamage(enemyAi._damage);
        }
        if (checkTagHouse(obj))
        {
            var health = obj.GetComponent<HouseHealth>();
            if (health.getCanDetec())
                health.takeDamage(enemyAi._damage);
        }
        if (checkTagAnimal(obj))
        {
            Debug.Log("Chua co");
        }
    }

    private bool checkTagPlayer(GameObject collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer" };
        return _tag.Contains(collision.tag);
    }
    private bool checkTagHouse(GameObject collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(GameObject collision)
        => collision.CompareTag("Animal");

    public void openMeat()
    {
        MeatOBJ.SetActive(true);
    }
    public void openGold()
    {
        GoldOBJ.SetActive(true);
    }
}
