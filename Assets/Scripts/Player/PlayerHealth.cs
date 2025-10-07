using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private UnitType _unitClass;
    private bool IsWarriorOrLancer => _unitClass == UnitType.Warrior || _unitClass == UnitType.Lancer;
    [ShowIf(nameof(IsWarriorOrLancer))]
    [SerializeField] private float Armor = 2f;
    [SerializeField] private GameObject _HP_obj;

    private PlayerAI _playerAI;

    void Start()
    {
        if (_HP_obj == null)
            Debug.LogError($"[{transform.name}] [PlayerHealth] Chưa gán 'GameObject HPbar'!");
        else
            _HP_obj.SetActive(false);
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
            GameManager.Instance.UIupdatePlayerValue();
            _playerAI.Dead();
        }

        _HP_obj.SetActive(true);
        if (_hideHP != null)
            StopCoroutine(_hideHP);
        _hideHP = StartCoroutine(hideHP());
    }

    private Coroutine _hideHP;
    private IEnumerator hideHP()
    {
        if (_playerAI.getDie())
            _HP_obj.SetActive(false);
        yield return new WaitForSeconds(5.5f);
        _HP_obj.SetActive(false);
    }
}
