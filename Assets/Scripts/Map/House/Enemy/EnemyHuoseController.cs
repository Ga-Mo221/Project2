using UnityEngine;

public class EnemyHuoseController : MonoBehaviour
{
    [SerializeField] private HouseType _type;

    public float _maxHealth = 100f;
    public float _currentHealth = 0f;

    void Start()
    {
        _currentHealth = _maxHealth;
    }
}