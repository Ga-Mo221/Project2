using System.Collections;
using UnityEngine;

public enum ItemType
{
    Tree,
    Rock,
    Gold
}

public class Item : MonoBehaviour
{
    [SerializeField] public ItemType _type;
    // [SerializeField] private int _value = 5;
    [SerializeField] private float _spawnTime = 60;
    [SerializeField] public int _maxStack = 3;
    [SerializeField] public bool _seleted = false;
    [SerializeField] public bool _detec = false;

    [SerializeField] private SpriteRenderer _sottingLayer;
    [SerializeField] private Transform _treePoint;
    [SerializeField] private GameObject _treeDie;
    private Collider2D _collider;

    public int _stack;

    void Start()
    {
        _stack = _maxStack;
        _sottingLayer.sortingOrder = -(int)(_treePoint.position.y * 100);
        _collider = GetComponent<Collider2D>();
    }

    public void farm()
    {
        _stack--;
        if (_stack <= 0)
        {
            StartCoroutine(colliderEnable());
            _sottingLayer.enabled = false;
            _treeDie.SetActive(true);
            Animator _anim = _treeDie.GetComponent<Animator>();
            SpriteRenderer _sprite = _treeDie.GetComponent<SpriteRenderer>();
            _sprite.sortingOrder = -(int)(_treePoint.position.y * 100);
            _anim.SetBool("Die", true);
        }
    }
    private IEnumerator colliderEnable()
    {
        yield return new WaitForSeconds(1.3f);
        _collider.enabled = false;
        StartCoroutine(resPawn());
    }

    private IEnumerator resPawn()
    {
        yield return new WaitForSeconds(_spawnTime);
        _stack = _maxStack;
        _seleted = false;
        _collider.enabled = true;
        _sottingLayer.enabled = true;
        _treeDie.SetActive(false);
        Animator _anim = _treeDie.GetComponent<Animator>();
        _anim.SetBool("Die", false);
    }
}
