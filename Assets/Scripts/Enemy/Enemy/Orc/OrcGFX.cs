using UnityEngine;

public class OrcGFX : EnemyAI
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    protected override void Update()
    {
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + 10000;
        base.Update();
    }
}
