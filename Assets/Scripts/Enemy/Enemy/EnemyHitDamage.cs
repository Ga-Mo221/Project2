using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemyHitDamage : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAi;
    [SerializeField] private GameObject MeatOBJ;
    [SerializeField] private GameObject GoldOBJ;

    public bool _rangeAttack = false;
    [ShowIf(nameof(_rangeAttack))]
    [SerializeField] private bool _boss = false;
    [ShowIf(nameof(_rangeAttack))]
    [SerializeField] private GameObject _prefeb;
    [ShowIf(nameof(_rangeAttack))]
    [SerializeField] private Transform _point;
    [ShowIf(nameof(_rangeAttack))]
    [SerializeField] private List<GameObject> _listPrefeb = new List<GameObject>();

    public void attack()
    {
        GameObject obj = enemyAi.target;
        if (obj == null) return;

        if (checkTagPlayer(obj))
        {
            var health = obj.GetComponent<PlayerHealth>();
            if (health != null)
                health.takeDamage(enemyAi._damage);
        }
        if (checkTagHouse(obj))
        {
            var health = obj.GetComponent<HouseHealth>();
            if (health != null && health.getCanDetec())
                health.takeDamage(enemyAi._damage);
        }
        if (checkTagAnimal(obj))
        {
            var animalHealth = obj.GetComponent<AnimalHealth>();
            if (animalHealth != null && !animalHealth._animalAi.getDie())
                animalHealth.takeDamage(enemyAi._damage, gameObject);
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

    public void spawnPrefab()
    {
        bool isSpawn = false;
        foreach (var obj in _listPrefeb)
        {
            if (!obj.activeSelf && enemyAi.target != null)
            {
                if (_boss)
                {
                    var script = obj.GetComponent<fireMagic>();
                    script.setProperties(enemyAi.target.transform, enemyAi._damage);
                }
                else
                {
                    var script = obj.GetComponent<ArowEnemy>();
                    script.setProperties(enemyAi.target.transform, enemyAi._damage,1, transform.localScale);
                }
                obj.transform.position = _point.position;
                obj.SetActive(true);
                isSpawn = true;
                break;
            }
        }
        if (!isSpawn && enemyAi.target != null)
        {
            GameObject obj = Instantiate(_prefeb, _point.position, Quaternion.identity, _point);
            if (_boss)
            {
                var script = obj.GetComponent<fireMagic>();
                script.setProperties(enemyAi.target.transform, enemyAi._damage);
            }
            else
            {
                var script = obj.GetComponent<ArowEnemy>();
                script.setProperties(enemyAi.target.transform, enemyAi._damage,1, transform.localScale);
            }
            _listPrefeb.Add(obj);
        }
    }

    public void openMeat()
    {
        MeatOBJ.SetActive(true);
    }
    public void openGold()
    {
        GoldOBJ.SetActive(true);
    }
}
