using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHouseSelectBox : MonoBehaviour
{
    [SerializeField] private EnemyHuoseController _enemyHouse;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private TextMeshProUGUI _content;

    private string key = "";
    private string txt = "";

    void Update()
    {
        if (_enemyHouse != null)
        {
            _HP_img.fillAmount = _enemyHouse._currentHealth / _enemyHouse._maxHealth;
            _health.text = $"{_enemyHouse._currentHealth} / {_enemyHouse._maxHealth}";
        }
    }

    public void add(EnemyHuoseController House)
    {
        _enemyHouse = House;

        key = "ui.Level";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _name.text = txt;

        key = "ui.Level";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _content.text = txt;
    }
}
