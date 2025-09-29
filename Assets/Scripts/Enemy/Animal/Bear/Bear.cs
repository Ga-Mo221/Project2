using System.IO;
using UnityEngine;

public class Bear : AnimalAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
        {
            Debug.LogError("[Bear] chua gan 'spriterender'");
        }
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100);

        target = findEnemyorPlayer();
        if (target != null)
        {
           //Debug.Log("tim thay");
            setdetec(true);
            attack(target);
        }
        else
        {
            setdetec(false);
        }
    }
}
