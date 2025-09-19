using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum ItemType
{
    Tree,
    Rock,
    Gold
}

public class Item : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] public ItemType _type;
    private bool _IsTree => _type == ItemType.Tree;
    private bool _IsRock => _type == ItemType.Rock;
    private bool _IsGold => _type == ItemType.Gold;

    [Header("Propety")]
    public int _maxValue = 5;
    [ShowIf(nameof(_IsGold))]
    public int _valueOneDrop = 5;
    [HideIf(nameof(_IsGold))]
    public float _spawnTime = 60;
    public int _maxStack = 3;
    [ShowIf(nameof(_IsGold))]
    public List<PlayerAI> _Farmlist;
    public int _maxFarmers = 2;
    [HideIf(nameof(_IsGold))]
    public bool _seleted = false;
    public bool _detec = false;

    [Header("Reference")]
    public SpriteRenderer _spriteRender;
    [HideIf(nameof(_IsRock))]
    public Animator _anim;
    [SerializeField] private Transform _OderPoin;
    [HideIf(nameof(_IsGold))]
    [SerializeField] private GameObject _DiePrefab;

    [Header("Value")]
    public int _value = 0;
    [ShowIf(nameof(_IsGold))]
    public int _stack;

    protected virtual void Start()
    {
        _value = _maxValue;
        _stack = _maxStack;
        _spriteRender.sortingOrder = -(int)(_OderPoin.position.y * 100) + 10000;
    }

    public virtual void farm(PlayerAI _playerAI)
    {
        _stack--;
        if (_IsTree)
            _anim.SetTrigger("Farm");
        if (_stack <= 0 && !_IsGold)
        {
            _playerAI.target = null;
            _playerAI._canAction = false;
            StartCoroutine(resPawn());
            _DiePrefab.SetActive(true);
            Animator _animDie = _DiePrefab.GetComponent<Animator>();
            SpriteRenderer _sprite = _DiePrefab.GetComponent<SpriteRenderer>();
            var _pickup = _DiePrefab.GetComponent<PickUp>();
            _pickup.setDropItem(_type, _value, _playerAI);
            _sprite.sortingOrder = -(int)(_OderPoin.position.y * 100);
            _animDie.SetBool("Die", true);
            if (_IsTree)
                _anim.SetBool("Die", true);
            if (_IsRock)
            {
                _spriteRender.enabled = false;
            }
        }
    }

    private IEnumerator resPawn()
    {
        yield return new WaitForSeconds(_spawnTime);
        _stack = _maxStack;
        _seleted = false;
        _spriteRender.enabled = true;
        _DiePrefab.SetActive(false);
        Animator _animDie = _DiePrefab.GetComponent<Animator>();
        _animDie.SetBool("Die", false);
        if (_IsTree)
            _anim.SetBool("Die", false);
        if (_IsRock)
            _spriteRender.enabled = true;
    }
}
