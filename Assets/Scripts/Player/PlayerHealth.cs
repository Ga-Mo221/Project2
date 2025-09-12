using NaughtyAttributes;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private UnitType _unitClass;
    private bool IsWarriorOrLancer => _unitClass == UnitType.Warrior || _unitClass == UnitType.Lancer;
    [ShowIf(nameof(IsWarriorOrLancer))]
    [SerializeField] private float Armor = 2f;

    private PlayerAI _playerAI;

    void Start()
    {
        _playerAI = GetComponent<PlayerAI>();
        if (!_playerAI)
            Debug.LogError($"[{transform.parent.name}] [PlayerHealth] Chưa lấy được 'PlayerAI'");
    }


    public void takeDamage(float damage)
    {
        float _damage = damage;
        if (IsWarriorOrLancer)
            if (_playerAI.getDetect())
                _damage = damage - Armor;

        _playerAI._health -= _damage;
        if (_playerAI._health <= 0)
        {
            _playerAI.Dead();
        }
    }
}
