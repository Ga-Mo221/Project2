using UnityEngine;

public class LancerGFX_E : EnemyAI
{
    [SerializeField] private SpriteRenderer _sotingLayer;

    protected override void Update()
    {
        base.Update();
        _sotingLayer.sortingOrder = -(int)(transform.position.y * 100) + 10000;

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
