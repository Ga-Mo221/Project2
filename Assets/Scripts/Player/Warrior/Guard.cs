using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private WarriorGFX _script;
    private GameObject target;

    public void farm()
    {
        target = _script.target;
        if (target == null) return; 
        if (target.CompareTag("Item"))
            target.GetComponent<Item>().farm(_script);
    }

    public void attack()
    {
        GameObject target = _script.target;
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
            Debug.Log("chua lam");
        }
        if (checkAnimal(target))
        {
            var animalhealth = target.GetComponent<AnimalHealth>();
            if (animalhealth != null)
            {
                animalhealth.takeDamage(_script._damage, gameObject);
                if (animalhealth._animalAi._Die)
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

    public void setActive() => _script.setActive();

    public void offDetec()
        => _script.offDetec();
    public void onDetec()
        => _script.onDetec();
}
