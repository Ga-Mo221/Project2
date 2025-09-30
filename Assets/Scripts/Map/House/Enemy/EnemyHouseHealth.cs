using UnityEngine;

public class EnemyHouseHealth : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private EnemyHuoseController _house;
    [SerializeField] private Transform _endPos;
    public bool _Die = false;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _spriteRenderer.sortingOrder = -(int)(_endPos.position.y * 100) + 10000;
    }

    public void takeDamage(float damage)
    {
        _house._currentHealth -= damage;
        if (_house._currentHealth <= 0)
        {
            _house._currentHealth = 0;
            _anim.SetTrigger("Die");
            _Die = true;
        }
    }
}
