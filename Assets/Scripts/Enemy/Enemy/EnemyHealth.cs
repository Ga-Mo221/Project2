using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyAI _enemyAI;

    public void takeDamage(float damage)
    {
        _enemyAI._currentHealth -= damage;
        if (_enemyAI._currentHealth <= 0)
        {
            _enemyAI.dead();
        }
    }
}
