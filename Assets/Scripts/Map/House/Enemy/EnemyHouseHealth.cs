using UnityEngine;
using System.Collections;

public class EnemyHouseHealth : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private EnemyHuoseController _house;
    [SerializeField] private Transform _endPos;
    [SerializeField] private HPBar _HPimg;
    public bool _Die = false;
    [SerializeField] private BuidingFire _fire;
    [SerializeField] private GameObject _HP_obj;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        int oder = -(int)(_endPos.position.y * 100) + 10000;
        _fire.oder(oder);
        _spriteRenderer.sortingOrder = oder;
        _house.updateSpriteOder(oder);
        if (_HP_obj == null)
            Debug.LogError($"[{transform.name}] [EnemyHouseHealth] Chưa gán 'GameObject HPbar'!");
        else
            _HP_obj.SetActive(false);
    }

    public void takeDamage(float damage)
    {
        _house._currentHealth -= damage;
        _HPimg.SetHealth(_house._currentHealth / _house._maxHealth);
        onfire();
        if (_house._currentHealth <= 0)
        {
            _house._currentHealth = 0;
            _house.gnollCreate(_endPos);
            _anim.SetTrigger("Die");
            _Die = true;
            _fire.gameObject.SetActive(false);
            _house.die();
            _house._audio.PlayDieSound();
        }

        _HP_obj.SetActive(true);
        if (_hideHP != null)
            StopCoroutine(_hideHP);
        _hideHP = StartCoroutine(hideHP());
    }


    private Coroutine _hideHP;
    private IEnumerator hideHP()
    {
        if (_house.getDie())
            _HP_obj.SetActive(false);
        yield return new WaitForSeconds(5.5f);
        _HP_obj.SetActive(false);
    }

    private void onfire()
    {
        if (_house._currentHealth / _house._maxHealth < 0.3f)
        {
            _fire.gameObject.SetActive(true);
            _house._audio.PlayFireSound();
        }
    }
}
