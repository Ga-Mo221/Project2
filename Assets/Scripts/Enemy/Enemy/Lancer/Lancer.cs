using UnityEngine;

public class Lancer : EnemyAI
{
    [SerializeField] private SpriteRenderer _sotingLayer;


    protected override void Update()
    {
        base.Update();
        _sotingLayer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
