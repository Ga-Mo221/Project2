using UnityEngine;

public class AnimalAIHitDame : MonoBehaviour
{
    [SerializeField] private AnimalAI enimalAi;
    [SerializeField] private GameObject MeatOBJ;
    public void attack()
    {
        GameObject obj = enimalAi.target;
        if (obj == null) return;

        if (PlayerTag.checkTag(obj.tag))
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

    private bool checkTagEnemy(GameObject collision)
    {
        return collision.CompareTag("Enemy");
    }

    public void openMeat()
    {
        MeatOBJ.SetActive(true);
    }
}
