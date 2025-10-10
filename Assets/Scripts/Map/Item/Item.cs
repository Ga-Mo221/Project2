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
    public ItemType _type;
    public int _id = 0;
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
    [ShowIf(nameof(_IsGold))]
    public int _maxFarmers = 2;
    [HideIf(nameof(_IsGold))]
    [SerializeField] private PlayerAI _seleted;
    public bool _detec = false;

    [Header("Reference")]
    public SpriteRenderer _spriteRender;
    [HideIf(nameof(_IsRock))]
    public Animator _anim;
    [SerializeField] private Transform _OderPoin;
    [HideIf(nameof(_IsGold))]
    [SerializeField] private GameObject _DiePrefab;
    public GameObject _outLine;

    public UnitAudio _audio;
    private bool _Die = false;

    [Header("Value")]
    public int _value = 0;
    public int _stack;

    #region Start
    protected virtual void Start()
    {
        _value = _maxValue;
        _stack = _maxStack;
        _spriteRender.sortingOrder = -(int)(_OderPoin.position.y * 100) + 10000;
    }
    #endregion


    #region farm
    public virtual void farm(PlayerAI _playerAI)
    {
        _audio.PlayFarmOrHitDamageSound();
        _stack--;
        if (_IsTree)
            _anim.SetTrigger("Farm");
        if (_stack <= 0 && !_IsGold)
        {
            _Die = true;
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
            {
                _outLine.SetActive(false);
                _anim.SetBool("Die", true);
            }
            if (_IsRock)
            {
                _outLine.SetActive(false);
                _spriteRender.enabled = false;
            }
            _audio.PlayDieSound();
        }
    }
    #endregion


    #region Respawn
    private IEnumerator resPawn()
    {
        yield return new WaitForSeconds(_spawnTime);
        _Die = false;
        _outLine.SetActive(true);
        _stack = _maxStack;
        _seleted = null;
        _spriteRender.enabled = true;
        _DiePrefab.SetActive(false);
        Animator _animDie = _DiePrefab.GetComponent<Animator>();
        _animDie.SetBool("Die", false);
        if (_IsTree)
            _anim.SetBool("Die", false);
        if (_IsRock)
            _spriteRender.enabled = true;
    }
    #endregion


    #region add Player select
    public bool add(PlayerAI player)
    {
        if (_seleted == null)
        {
            _seleted = player;
            return true;
        }
        return false;
    }

    public void removeSelect(string name)
    {
        _seleted = null;
    }

    public bool checkSelectNull()
        => _seleted == null;
    public bool checkSelect(PlayerAI player)
        => _seleted == player;
    #endregion

    public bool getDie()
        => _Die;
}
