using UnityEngine;

public class AnimalAIHitDame : MonoBehaviour
{
    [SerializeField] private AnimalAI animalAi;
    [SerializeField] private GameObject MeatOBJ;
    public void attack()
    {
        GameObject obj = animalAi.target;
        if (obj == null) return;

        if (PlayerTag.checkTag(obj.tag))
        {
            var health = obj.GetComponent<PlayerHealth>();
            health.takeDamage(animalAi._damage);
        }
        if (checkTagEnemy(obj))
        {
            var health = obj.GetComponent<EnemyHealth>();
            health.takeDamage(animalAi._damage);
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

    public void onCanMove() => animalAi.setCanMove(true);
    public void offCanMove() => animalAi.setCanMove(false);

    public void PlayAttackSoundAnimal() => animalAi.playAttackSound();
}
