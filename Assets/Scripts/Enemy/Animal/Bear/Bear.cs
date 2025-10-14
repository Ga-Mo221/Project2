using System.IO;
using UnityEngine;

public class Bear : AnimalAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        if (!_spriteRender)
            Debug.LogError("[Bear] chua gan 'spriterender'");
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100)+ 10000;
    }
}
