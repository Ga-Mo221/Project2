using UnityEngine;

public class FishGFX : EnemyAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100) + 10000;
        base.Update();
    }
}
