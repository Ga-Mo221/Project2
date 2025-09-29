using UnityEngine;

public class OrcGFX : EnemyAI
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    protected override void Update()
    {
        base.Update();
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + 10000;

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
