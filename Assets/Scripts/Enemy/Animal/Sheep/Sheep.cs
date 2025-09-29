using System.Collections;
using System.IO;
using UnityEngine;

public class Sheep : AnimalAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
        {
            Debug.LogError("[Sheep] chua gan 'spriterender'");
        }
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100);
    }


    public override void FleeFrom(GameObject attackr)
    {
        Vector2 FleeDir = (transform.position - attackr.transform.position).normalized;
        SetNewPatrol(FleeDir);
    }
    
}
