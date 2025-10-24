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

        if (PlayerTag.checkTag(obj.tag))
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

    private bool checkTagHouse(GameObject collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(GameObject collision)
        => collision.CompareTag("Animal");

    public void spawnPrefab()
    {
        if (enemyAi.target == null) return;
        if (enemyAi.target.tag == "Animal")
        {
            var animal = enemyAi.target.GetComponent<AnimalAI>();
            if (animal.getDie())
                return;
        }
        if (PlayerTag.checkTag(enemyAi.target.tag))
        {
            var player = enemyAi.target.GetComponent<PlayerAI>();
            if (player.getDie())
                return;
        }
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
                    var script = obj.GetComponent<ArrowEnemy>();
                    script.setProperties(enemyAi.target.transform, enemyAi._damage, 1, transform.localScale);
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
                var script = obj.GetComponent<ArrowEnemy>();
                script.setProperties(enemyAi.target.transform, enemyAi._damage, 1, transform.localScale);
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

    public void onCanMove() => enemyAi.setCanMove(true);
    public void offCanMove() => enemyAi.setCanMove(false);

    public void PlayAttackSoundEnemy() => enemyAi.playAttackSoundEnemy();
}
