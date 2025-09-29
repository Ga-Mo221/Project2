using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public AnimalAI _animalAi;

    public void takeDamage(float damage, GameObject attackr)
    {
        _animalAi._health -= damage;
        _animalAi.FleeFrom(attackr);
        
        if (_animalAi._health <= 0)
        {
            _animalAi.Die();
        }
    }
}
