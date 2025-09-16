using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public enum ModelType
{
    Tree,
    Rock,
    Gold,
    Deco,
    Animal,
    Enemy
}

public class Display : MonoBehaviour
{
    [SerializeField] private ModelType _type;
    public bool _IsTree => _type == ModelType.Tree;
    public bool _IsDeco => _type == ModelType.Deco;
    public bool _IsRock => _type == ModelType.Rock;
    public bool _IsGold => _type == ModelType.Gold;
    private bool _IsItem => _IsTree || _IsGold || _IsRock;
    public bool _IsAnimal => _type == ModelType.Animal;
    public bool _IsTreeOrRock => _IsTree || _IsRock;

    [SerializeField] private GameObject _GFX;
    [SerializeField] private GameObject _outLine;

    [HideIf(nameof(_IsItem))]
    [SerializeField] private GameObject _HPBar;
    [HideIf(nameof(_IsItem))]
    [SerializeField] private GameObject _MiniMapIcon;

    [ShowIf(nameof(_IsTreeOrRock))]
    [SerializeField] private GameObject _Die;
    [ShowIf(nameof(_IsItem))]
    [SerializeField] private Item _item;

    [SerializeField] public List<Rada> _seemer;

    private string _sortingLayer;
    public bool _Detec = false;
    private SpriteRenderer _spriteRender;


    void Start()
    {
        if (!_GFX)
            Debug.LogError($"[{transform.name}] [Display] Chưa gán '_GFX'");
        if (!_outLine)
            Debug.LogError($"[{transform.name}] [Display] Chưa gán '_outLine'");
        _spriteRender = _GFX.GetComponent<SpriteRenderer>();
        _sortingLayer = _spriteRender.sortingLayerName;

        if (_IsTree)
            if (!_Die)
                Debug.LogError($"[{transform.name}] [Display] Chưa gán '_GFX'");

        offDisPlay();
    }

    private void offDisPlay()
    {
        _GFX.SetActive(false);
        _outLine.SetActive(false);

        if (_IsTree)
            _Die.SetActive(false);

        if (_IsAnimal)
            _HPBar.SetActive(false);
    }


    public void onDisplayTree()
    {
        if (!_IsTree) return;
        _Detec = true;
        _GFX.SetActive(true);
        _outLine.SetActive(true);
        _Die.SetActive(false);
        _item._detec = true;
    }

    public void onDisplayRock()
    {
        if (!_IsRock) return;
        _Detec = true;
        if (!_GFX.activeSelf)
            _GFX.SetActive(true);
        else
            _spriteRender.sortingLayerName = _sortingLayer;
        _outLine.SetActive(true);
        _item._detec = true;
    }

    public void offDisplayRock()
    {
        if (!_IsRock) return;
        _spriteRender.sortingLayerName = "Default";
        _Detec = false;
        _item._detec = false;
    }

    public void onDisplayAnimal()
    {
        if (!_IsAnimal) return;
        _Detec = true;
        if (!_GFX.activeSelf)
            _GFX.SetActive(true);
        else
            _spriteRender.sortingLayerName = _sortingLayer;
        _HPBar.SetActive(true);
        _outLine.SetActive(true);
        _MiniMapIcon.SetActive(true);
    }

    public void offDisplayAnimal()
    {
        if (!_IsAnimal) return;
        _Detec = false;
        _spriteRender.sortingLayerName = "Default";
        _outLine.SetActive(false);
        _MiniMapIcon.SetActive(false);
        _HPBar.SetActive(false);
    }

    public void onDisplayGoldMine()
    {
        if (!_GFX.activeSelf)
            _GFX.SetActive(true);
        else
            _spriteRender.sortingLayerName = _sortingLayer;
        _outLine.SetActive(true);
        _item._detec = true;
        _Detec = true;
    }

    public void offDisplayGoldMine()
    {
        _spriteRender.sortingLayerName = "Default";
        _outLine.SetActive(false);
        _item._detec = false;
        _Detec = false;
    }
}
