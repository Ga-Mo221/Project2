using UnityEngine;

public class TNTRedGFX : EnemyAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100) + 10000;
        base.Update();
    }

    protected override void AI()
    {
        flip(target);
        if (getDie()) return;
        target = Find();
        if (target != null)
        {
            setDetect(true);
        }
    }
}
