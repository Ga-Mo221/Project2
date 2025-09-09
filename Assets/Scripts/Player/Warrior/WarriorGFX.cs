using UnityEngine;
using UnityEngine.UI;

public class WarriorGFX : PlayerAI
{
    [SerializeField] private SpriteRenderer _sotingLayer;
    protected override void Update()
    {
        base.Update();
        _sotingLayer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
