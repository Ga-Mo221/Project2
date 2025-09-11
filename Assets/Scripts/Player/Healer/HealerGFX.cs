using System.Collections;
using UnityEngine;

public class HealerGFX : PlayerAI
{
    [Header("Healer GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    private PlayerAI _scripTarget;

    [Header("SKILL")]
    [SerializeField] private int _heal_count_SKILL = 10;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[HealerGFX] Chưa gán 'SpriteRender'");
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100);
        flip();
        if (getIsAI())
        {
            setupFolow();
            findTargetMoveTo();
            _scripTarget = attack();
        }
    }


    private Coroutine _healspeed;
    protected override PlayerAI attack()
    {
        GameObject _nearest = getGameObject();
        if (_nearest == null) return null;
        var script = _nearest.GetComponent<PlayerAI>();

        if (script._health < script._maxHealth)
        {
            if (_healspeed == null)
                _healspeed = StartCoroutine(healSpeed());
        }
        return script;
    }
    private IEnumerator healSpeed()
    {
        _anim.SetTrigger("Heal");
        _attackCount++;
        yield return new WaitForSeconds(_attackSpeedd);
        _healspeed = null;
    }


    public void heals()
    {
        if (_scripTarget == null) return;
        _scripTarget._healPlus = _damage;
        if (_attackCount == _heal_count_SKILL)
        {
            _scripTarget._AOEHeal = true;
            _attackCount = 0;
        }
        _scripTarget._healEffect.SetActive(true);
    }
}
