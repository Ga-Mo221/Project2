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
    public bool _IsDecoOrBuiding => _type == ModelType.Deco || _type == ModelType.Buiding;
    public bool _IsTreeOrRock => _IsTree || _IsRock;

    [SerializeField] private bool _Die = false;

    [ShowIf(nameof(_IsAnimalOrEnemyOrHouse))]
    [SerializeField] private GameObject _MiniMapIcon;

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

    [HideIf(nameof(_IsDecoOrBuiding))]
    [SerializeField] private GameObject _outLine;

    [SerializeField] private List<Rada> _seemer = new List<Rada>();

    public bool _Detec = false;

    void Start()
    {
        if (_IsAnimalOrEnemyOrHouse)
        {
            if (!_MiniMapIcon)
                Debug.LogError($"[{transform.name}] [Display] Chưa gán 'Minimap Icon'");
        }
        if (_IsItem)
            if (!_item)
                Debug.LogError($"[{transform.name}] [Display] Chưa gán 'Scipt Item'");

        if (!_IsDeco && !_IsBuiding && !_outLine)
            Debug.LogError($"[{transform.name}] [Display] Chưa gán 'Out Line'");

        off();

        InvokeRepeating(nameof(updateSee), 0.2f, 0.2f);
    }


    private bool checkDist()
    {
        bool _isSee = false;
        foreach (var seemer in _seemer)
        {
            if (!seemer.getDie())
            {
                float dis = Vector3.Distance(transform.position, seemer.transform.position);
                if (dis < seemer._radius)
                    _isSee = true;
            }
        }

        return _isSee;
    }


    public void addSeemer(Rada rada)
        => _seemer.Add(rada);


    public bool removeSeemer(Rada rada)
        => _seemer.Remove(rada);

    public bool checkSeemer(Rada rada)
        => _seemer.Contains(rada);


    private void updateSee()
    {
        checkDie();
        if (!checkDist())
        {
            _seemer = new List<Rada>();
        }
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
            if (_MiniMapIcon != null)
                _MiniMapIcon.SetActive(false);
        }
        if (_IsDecoOrGold)
            _light.SetActive(false);

        if (!_IsDecoOrBuiding)
            _outLine.SetActive(false);
    }


    private void onDisplayTree()
    {
        _Detec = true;
        _item._detec = true;
        if (!_item.getDie())
            _outLine.SetActive(true);
        else
            _outLine.SetActive(false);
    }

    private void offDisplayTree()
    {
        _Detec = false;
        _item._detec = false;
        _outLine.SetActive(false);
    }

    private void onDisplayRock()
    {
        _Detec = true;
        _item._detec = true;
        if (!_item.getDie())
            _outLine.SetActive(true);
        else
            _outLine.SetActive(false);
    }

    private void offDisplayRock()
    {
        _Detec = false;
        _item._detec = false;
        _outLine.SetActive(false);
    }

    private void onDisplayGoldMine()
    {
        _item._detec = true;
        _Detec = true;
        _light.SetActive(true);
        _outLine.SetActive(true);
    }

    private void offDisplayGoldMine()
    {
        _item._detec = false;
        _Detec = false;
        _light.SetActive(false);
        _outLine.SetActive(false);
    }

    private void onDisplayAnimal()
    {
        _Detec = true;
        _MiniMapIcon.SetActive(true);
        _outLine.SetActive(true);
    }

    private void offDisplayAnimal()
    {
        _Detec = false;
        _MiniMapIcon.SetActive(false);
        _outLine.SetActive(false);
    }

    private void onCanCreate()
    {
        _check._see = true;
        _Detec = true;
        if (_check.getCanCreate())
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
        _MiniMapIcon.SetActive(true);
        _outLine.SetActive(true);
    }

    private void offDisplayEnemy()
    {
        _Detec = false;
        _MiniMapIcon.SetActive(false);
        _outLine.SetActive(false);
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
        _fire.displayFireLight(true);
        _outLine.SetActive(true);
    }

    private void offDisplayEnemyHouse()
    {
        _Detec = false;
        _MiniMapIcon.SetActive(false);
        _fire.displayFireLight(false);
        _outLine.SetActive(false);
    }
}
