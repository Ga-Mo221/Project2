using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOrEnemyTowerSelectBox : MonoBehaviour
{
    [SerializeField] private House _playerHouse;
    [SerializeField] private EnemyHuoseController _enemyHouse;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _playerTowerSprite;
    [SerializeField] private Sprite _enemyTowerSprite;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private TextMeshProUGUI _damge;
    [SerializeField] private TextMeshProUGUI _PhamVi;

    [SerializeField] private TextMeshProUGUI _content;

    [SerializeField] private TextMeshProUGUI _Lv;

    private string key = "";
    private string txt = "";

    void Update()
    {
        if (_playerHouse != null)
        {
            _HP_img.fillAmount = _playerHouse._currentHealth / _playerHouse._maxHealth;
            _health.text = $"{_playerHouse._currentHealth} / {_playerHouse._maxHealth}";
        }
        else if (_enemyHouse != null)
        {
            _HP_img.fillAmount = _enemyHouse._currentHealth / _enemyHouse._maxHealth;
            _health.text = $"{_enemyHouse._currentHealth} / {_enemyHouse._maxHealth}";
        }
    }


    public void add(House House)
    {
        _playerHouse = House;
        _enemyHouse = null;
        key = "ui.Level";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _icon.sprite = _playerTowerSprite;
        _Lv.text = $"{txt}{_playerHouse.getLevel()}";

        key = "Name.building.tower";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _name.text = txt;

        _damge.text = _playerHouse._archer.getDamage().ToString();
        _PhamVi.text = _playerHouse._archer.getRadius().ToString();

        key = "Name.building.towerContent";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _content.text = txt;
    }

    public void add(EnemyHuoseController House)
    {
        _playerHouse = null;
        _enemyHouse = House;

        _icon.sprite = _enemyTowerSprite;
        _Lv.text = "";

        key = "Name.building.enemytower";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _name.text = txt;

        var gnoll = _enemyHouse._GnollUp.GetComponent<GnollUpGFX>();
        _damge.text = gnoll.getDamage().ToString();
        _PhamVi.text = gnoll.getRadius().ToString();

        key = "Name.building.enemytowerContent";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _content.text = txt;
    }
}
