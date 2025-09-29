using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemDropType
{
    Meat,
    Gold
}

public class DropItem : MonoBehaviour
{
    [SerializeField] private ItemDropType _type;

    [SerializeField] private int _maxValueDrop;
    private Coroutine des;
    void OnTriggerEnter2D(Collider2D collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer" };
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

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(2f);
        transform.parent.parent.gameObject.SetActive(false);
    }
}
