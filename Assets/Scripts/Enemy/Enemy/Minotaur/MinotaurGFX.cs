using UnityEngine;

public class MinotaurGFX : EnemyAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100) + 10000;

        if (getDie()) return;
        target = Find();
        if (target != null)
        {
            setDetect(true);
            attack();
        }

        pantrol();
    }
}
