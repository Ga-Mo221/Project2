using NaughtyAttributes;
using UnityEngine;

public class EnemyHuoseController : MonoBehaviour
{
    [SerializeField] private HouseType _type;
    private bool IsTower => _type == HouseType.Tower;

    public float _maxHealth = 100f;
    public float _currentHealth = 0f;

    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _GnollUp;
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _GnollPrefab;

    void Start()
    {
        _currentHealth = _maxHealth;
        if (IsTower && _GnollUp != null && _GnollPrefab != null)
        {
            float damage = _GnollPrefab.GetComponent<GnollGFX>()._damage;
             _GnollUp.GetComponent<GnollUpGFX>().setDamage(damage);
        }
    }

    public void updateSpriteOder(int oder)
    {
        if (!IsTower) return;
        _GnollUp.GetComponent<SpriteRenderer>().sortingOrder = oder + 1;
    }

    public void gnollCreate(Transform amount)
    {
        if (!IsTower) return;
        _GnollUp.SetActive(false);
        Instantiate(_GnollPrefab, transform.position, Quaternion.identity, amount);
    }
}