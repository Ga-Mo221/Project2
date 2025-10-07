using NaughtyAttributes;
using UnityEngine;

public class PlayerHitDamage : MonoBehaviour
{
    public bool _isArown = false;
    [HideIf(nameof(_isArown))]
    [SerializeField] private PlayerAI _script;
    private GameObject target;
    [SerializeField] private bool _isPlayerAi = true;

    public void setPlayerAI(PlayerAI script) => _script = script;
    public void setTarget(GameObject obj) => target = obj;

    public void attack(bool isPlayer, float damage, GameObject obj, bool isArown = false)
    {
        _isPlayerAi = isPlayer;
        if (_isPlayerAi && !isArown)
            target = _script.target;
        else
            target = obj;

        if (target == null) return;
        if (checkEnemy(target))
        {
            var enemyHealth = target.GetComponent<EnemyHealth>();

            if (_isPlayerAi)
                enemyHealth.takeDamage(_script._damage);
            else
                enemyHealth.takeDamage(damage);

            if (_isPlayerAi && enemyHealth._enemyAI.getDie())
            {
                _script.target = null;
                _script.resetItemSelect();
            }
        }
        if (checkEnemyHouse(target))
        {
            var houseHealth = target.GetComponent<EnemyHouseHealth>();
            if (houseHealth != null && !houseHealth._Die)
            {
                houseHealth.takeDamage(_script._damage);
            }
        }
        if (checkAnimal(target))
        {
            var animalhealth = target.GetComponent<AnimalHealth>();
            if (animalhealth != null)
            {
                if (_isPlayerAi)
                    animalhealth.takeDamage(_script._damage, gameObject);
                else 
                    animalhealth.takeDamage(damage, gameObject);

                if (_isPlayerAi && animalhealth._animalAi.getDie())
                {
                    _script.target = null;
                    _script.resetItemSelect();
                }
            }
        }
    }

    public void attackEvent()
    {
        target = _script.target;

        if (target == null) return;
        if (checkEnemy(target))
        {
            var enemyHealth = target.GetComponent<EnemyHealth>();

            enemyHealth.takeDamage(_script._damage);

            if (enemyHealth._enemyAI.getDie())
            {
                _script.target = null;
                _script.resetItemSelect();
            }
        }
        if (checkEnemyHouse(target))
        {
            var houseHealth = target.GetComponent<EnemyHouseHealth>();
            if (houseHealth != null && !houseHealth._Die)
            {
                houseHealth.takeDamage(_script._damage);
            }
        }
        if (checkAnimal(target))
        {
            var animalhealth = target.GetComponent<AnimalHealth>();
            if (animalhealth != null)
            {
                animalhealth.takeDamage(_script._damage, gameObject);
                if (animalhealth._animalAi.getDie())
                {
                    _script.target = null;
                    _script.resetItemSelect();
                }
            }
        }
    }
    private bool checkEnemy(GameObject obj)
        => obj.CompareTag("Enemy");
    private bool checkAnimal(GameObject obj)
        => obj.CompareTag("Animal");
    private bool checkEnemyHouse(GameObject obj)
        => obj.CompareTag("EnemyHouse");
}
