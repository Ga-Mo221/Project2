using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum ModelType
{
    Tree,
    Rock,
    Gold,
    Deco,
    Animal,
    Enemy,
    Buiding,
    EnemyHouse
}

public class Display : MonoBehaviour
{
    [SerializeField] private ModelType _type;
    private bool _IsAnimalOrEnemyOrHouse => _type == ModelType.Animal || _type == ModelType.Enemy || _type == ModelType.EnemyHouse;
    private bool _IsItem => _type == ModelType.Tree || _type == ModelType.Rock || _type == ModelType.Gold;
    public bool _IsTree => _type == ModelType.Tree;
    public bool _IsRock => _type == ModelType.Rock;
    public bool _IsGold => _type == ModelType.Gold;
    public bool _IsDeco => _type == ModelType.Deco;
    public bool _IsAnimal => _type == ModelType.Animal;
    public bool _IsEnemy => _type == ModelType.Enemy;
    public bool _IsBuiding => _type == ModelType.Buiding;
    private bool _IsDecoOrGold => _type == ModelType.Deco || _type == ModelType.Gold;
    public bool _IsEnemyHouse => _type == ModelType.EnemyHouse;

    [SerializeField] private bool _Die = false;

    [ShowIf(nameof(_IsAnimalOrEnemyOrHouse))]
    [SerializeField] private GameObject _MiniMapIcon;
    [ShowIf(nameof(_IsAnimalOrEnemyOrHouse))]
    [SerializeField] private GameObject _HpBar;

    [ShowIf(nameof(_IsItem))]
    [SerializeField] private Item _item;

    [ShowIf(nameof(_IsBuiding))]
    [SerializeField] private CheckGroundCreate _check;

    [ShowIf(nameof(_IsDecoOrGold))]
    [SerializeField] private GameObject _light;

    [ShowIf(nameof(_IsAnimal))]
    [SerializeField] private AnimalAI _animal;

    [ShowIf(nameof(_IsEnemy))]
    [SerializeField] private EnemyAI _enemy;

    [ShowIf(nameof(_IsEnemyHouse))]
    [SerializeField] private EnemyHouseHealth _house;
    [ShowIf(nameof(_IsEnemyHouse))]
    [SerializeField] private BuidingFire _fire;

    [SerializeField] public List<Rada> _seemer;

    public bool _Detec = false;

    void Start()
    {
        if (_IsAnimalOrEnemyOrHouse)
        {
            if (!_MiniMapIcon)
                Debug.LogError($"[{transform.name}] [Display] Chưa gán 'Minimap Icon'");
            if (!_HpBar)
                Debug.LogError($"[{transform.name}] [Display] Chưa gán 'HpBar'");
        }
        if (_IsItem)
            if (!_item)
                Debug.LogError($"[{transform.name}] [Display] Chưa gán 'Scipt Item'");

        off();

        InvokeRepeating(nameof(updateSee), 1f, 1f);
    }


    private void updateSee()
    {
        checkDie();
        if (_seemer.Count > 0 && !_Die)
        {
            onDisplay();
        }
        else
        {
            offDisplay();
        }
    }

    void OnDisable()
    {
        offDisplay();
    }

    void OnDestroy()
    {
        offDisplay();
    }

    private void onDisplay()
    {
        if (_IsTree) onDisplayTree();
        if (_IsRock) onDisplayRock();
        if (_IsAnimal) onDisplayAnimal();
        if (_IsGold) onDisplayGoldMine();
        if (_IsBuiding) onCanCreate();
        if (_IsEnemy) onDisplayEnemy();
        if (_IsDeco) onDisplayDeco();
        if (_IsEnemyHouse) onDisplayEnemyHouse();
    }

    private void offDisplay()
    {
        if (_IsTree) offDisplayTree();
        if (_IsRock) offDisplayRock();
        if (_IsAnimal) offDisplayAnimal();
        if (_IsGold) offDisplayGoldMine();
        if (_IsBuiding) offCanCreate();
        if (_IsEnemy) offDisplayEnemy();
        if (_IsDeco) offDisplayDeco();
        if (_IsEnemyHouse) offDisplayEnemyHouse();
    }

    private void checkDie()
    {
        if (_IsEnemy)
            _Die = _enemy.getDie();
        if (_IsAnimal)
            _Die = _animal.getDie();
        if (_IsEnemyHouse)
            _Die = _house._Die;
    }

    private void off()
    {
        if (_IsAnimalOrEnemyOrHouse)
        {
            if (_HpBar != null)
                _HpBar.SetActive(false);
            if (_MiniMapIcon != null)
                _MiniMapIcon.SetActive(false);
        }
        if (_IsDecoOrGold)
            _light.SetActive(false);
    }


    private void onDisplayTree()
    {
        _Detec = true;
        _item._detec = true;
    }

    private void offDisplayTree()
    {
        _Detec = false;
        _item._detec = false;
    }

    private void onDisplayRock()
    {
        _Detec = true;
        _item._detec = true;
    }

    private void offDisplayRock()
    {
        _Detec = false;
        _item._detec = false;
    }

    private void onDisplayGoldMine()
    {
        _item._detec = true;
        _Detec = true;
        _light.SetActive(true);
    }

    private void offDisplayGoldMine()
    {
        _item._detec = false;
        _Detec = false;
        _light.SetActive(false);
    }

    private void onDisplayAnimal()
    {
        _Detec = true;
        _HpBar.SetActive(true);
        _MiniMapIcon.SetActive(true);
    }

    private void offDisplayAnimal()
    {
        _Detec = false;
        _HpBar.SetActive(false);
        _MiniMapIcon.SetActive(false);
    }

    private void onCanCreate()
    {
        _check._see = true;
        _Detec = true;
        _check._anim.SetBool("Red", false);
    }
    private void offCanCreate()
    {
        _check._see = false;
        _Detec = false;
        _check._anim.SetBool("Red", true);
    }

    private void onDisplayEnemy()
    {
        _Detec = true;
        _HpBar.SetActive(true);
        _MiniMapIcon.SetActive(true);
    }

    private void offDisplayEnemy()
    {
        _Detec = false;
        _HpBar.SetActive(false);
        _MiniMapIcon.SetActive(false);
    }

    private void onDisplayDeco()
    {
        _Detec = true;
        _light.SetActive(true);
    }

    private void offDisplayDeco()
    {
        _Detec = false;
        _light.SetActive(false);
    }

    private void onDisplayEnemyHouse()
    {
        _Detec = true;
        _MiniMapIcon.SetActive(true);
        _HpBar.SetActive(true);
        _fire.displayFireLight(true);
    }

    private void offDisplayEnemyHouse()
    {
        _Detec = false;
        _MiniMapIcon.SetActive(false);
        _HpBar.SetActive(false);
        _fire.displayFireLight(false);
    }
}
