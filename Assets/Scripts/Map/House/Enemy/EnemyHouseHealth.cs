using UnityEngine;
using UnityEngine.UI;

public class EnemyHouseHealth : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private EnemyHuoseController _house;
    [SerializeField] private Transform _endPos;
    [SerializeField] private Image _HPimg;
    public bool _Die = false;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        int oder = -(int)(_endPos.position.y * 100) + 10000;
        _spriteRenderer.sortingOrder = oder;
        _house.updateSpriteOder(oder);
    }

    public void takeDamage(float damage)
    {
        _house._currentHealth -= damage;
        _HPimg.fillAmount = _house._currentHealth / _house._maxHealth;
        if (_house._currentHealth <= 0)
        {
            _house._currentHealth = 0;
            _house.gnollCreate(_endPos);
            _anim.SetTrigger("Die");
            _Die = true;
        }
    }
}
