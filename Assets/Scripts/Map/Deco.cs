using UnityEngine;

public class Deco : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _point;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _spriteRenderer.sortingOrder = -(int)(_point.position.y * 100) + 10000;
    }
}
