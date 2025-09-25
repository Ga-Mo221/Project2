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
    Buiding
}

public class Display : MonoBehaviour
{
    [SerializeField] private ModelType _type;
    private bool _IsAnimalOrEnemy => _type == ModelType.Animal || _type == ModelType.Enemy;
    private bool _IsItem => _type == ModelType.Tree || _type == ModelType.Rock || _type == ModelType.Gold;
    public bool _IsTree => _type == ModelType.Tree;
    public bool _IsRock => _type == ModelType.Rock;
    public bool _IsGold => _type == ModelType.Gold;
    public bool _IsDeco => _type == ModelType.Deco;
    public bool _IsAnimal => _type == ModelType.Animal;
    public bool _IsEnemy => _type == ModelType.Enemy;
    public bool _IsBuiding => _type == ModelType.Buiding;

    [ShowIf(nameof(_IsAnimalOrEnemy))]
    [SerializeField] private GameObject _MiniMapIcon;
    [ShowIf(nameof(_IsAnimalOrEnemy))]
    [SerializeField] private GameObject _HpBar;

    [ShowIf(nameof(_IsItem))]
    [SerializeField] private Item _item;

    [ShowIf(nameof(_IsBuiding))]
    [SerializeField] private CheckGroundCreate _check;

    [SerializeField] public List<Rada> _seemer;

    public bool _Detec = false;

    void Start()
    {
        if (_IsAnimalOrEnemy)
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
    }

    private void off()
    {
        if (_IsAnimalOrEnemy)
        {
            if (_HpBar != null)
                _HpBar.SetActive(false);
            if (_MiniMapIcon != null)
                _MiniMapIcon.SetActive(false);
        }
    }


    public void onDisplayTree()
    {
        _Detec = true;
        _item._detec = true;
    }

    public void offDisplayTree()
    {
        _Detec = false;
        _item._detec = false;
    }

    public void onDisplayRock()
    {
        _Detec = true;
        _item._detec = true;
    }

    public void offDisplayRock()
    {
        _Detec = false;
        _item._detec = false;
    }

    public void onDisplayGoldMine()
    {
        _item._detec = true;
        _Detec = true;
    }

    public void offDisplayGoldMine()
    {
        _item._detec = false;
        _Detec = false;
    }

    public void onDisplayAnimal()
    {
        _Detec = true;
        _HpBar.SetActive(true);
        _MiniMapIcon.SetActive(true);
    }

    public void offDisplayAnimal()
    {
        _Detec = false;
        _HpBar.SetActive(false);
        _MiniMapIcon.SetActive(false);
    }

    public void onCanCreate()
    {
        _check._see = true;
        _Detec = true;
        _check._anim.SetBool("Red", false);
    }
    public void offCanCreate()
    {
        _check._see = false;
        _Detec = false;
        _check._anim.SetBool("Red", true);
    }
}
