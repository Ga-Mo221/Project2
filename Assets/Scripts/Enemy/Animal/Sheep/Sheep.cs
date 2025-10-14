using System.Collections;
using System.IO;
using UnityEngine;

public class Sheep : AnimalAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        if (!_spriteRender)
            Debug.LogError("[Sheep] chua gan 'spriterender'");
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100) + 10000;
    }

    protected override void AI()
    {
        flip(target, _canAction);
    }


    public override void FleeFrom(GameObject attackr)
    {
        Vector2 FleeDir = (transform.position - attackr.transform.position).normalized;
        SetNewPatrol(FleeDir);
    }
    
}
