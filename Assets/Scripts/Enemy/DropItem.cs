using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum ItemDropType
{
    Meat,
    Gold
}

public class DropItem : MonoBehaviour
{
    [SerializeField] private ItemDropType _type;

    [SerializeField] private bool _enemy = false;
    [ShowIf(nameof(_enemy))]
    [SerializeField] private Animator _animEnemy;
    [ShowIf(nameof(_enemy))]
    [SerializeField] private UnitAudio _audio;
    [ShowIf(nameof(_enemy))]
    [SerializeField] private EnemyAI _enemyAI;


    [SerializeField] private bool _animal = false;
    [ShowIf(nameof(_animal))]
    [SerializeField] private SpriteRenderer _sprite;
    [ShowIf(nameof(_animal))]
    [SerializeField] private AnimalAI _animalAI;

    
    [SerializeField] private bool _pickUP = false;

    [SerializeField] private int _maxValueDrop;
    private Coroutine des;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Apply(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Apply(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Apply(collision);
    }

    private void Apply(Collider2D collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer" };
        if (_pickUP) return;
        if (collision != null && _tag.Contains(collision.tag))
        {
            var player = collision.GetComponent<PlayerAI>();
            if (player == null) return;
            int value = Random.Range(1, _maxValueDrop);
            if (_type == ItemDropType.Meat)
            {
                if (player._meat < player._maxMeat)
                {
                    player._meat += value;
                    if (player._meat > player._maxMeat)
                        player._meat = player._maxMeat;
                    if (des == null)
                        des = StartCoroutine(destroy());
                }
            }
            if (_type == ItemDropType.Gold)
            {
                if (player._gold < player._maxGold)
                {
                    player._gold += value;
                    if (player._gold > player._maxGold)
                        player._gold = player._maxGold;
                    if (des == null)
                        des = StartCoroutine(destroy());
                }
            }
        }
    }
    
    public void ResetPickUp()
    {
        _pickUP = false;
        des = null;
    }

    private IEnumerator destroy()
    {
        _pickUP = true;
        des = null;
        if (_enemy)
            _audio.PlayFarmOrHitDamageSound();
        yield return new WaitForSeconds(2f);
        if (_animal)
        {
            if (_animalAI.getDie())
                _sprite.enabled = false;
            gameObject.SetActive(false);
        }
        else if (_enemy)
        {
            if (_enemyAI.getDie())
                _animEnemy.SetTrigger("PickUp");
            gameObject.SetActive(false);
        }
    }
}
