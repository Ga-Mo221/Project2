using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOrEnemyStorageSelectBox : MonoBehaviour
{
    [SerializeField] private House _playerHouse;
    [SerializeField] private EnemyHuoseController _enemyHouse;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _playerStorageSprite;
    [SerializeField] private Sprite _enemyStorageSprite;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private TextMeshProUGUI _wood;
    [SerializeField] private TextMeshProUGUI _rock;
    [SerializeField] private TextMeshProUGUI _meat;
    [SerializeField] private TextMeshProUGUI _gold;

    [SerializeField] private TextMeshProUGUI _content;

    [SerializeField] private TextMeshProUGUI _Lv;

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

        _icon.sprite = _playerStorageSprite;
        _Lv.text = $"LV.{_playerHouse.getLevel()}";

        _name.text = "Nhà Kho";

        _wood.text = _playerHouse._wood.ToString();
        _rock.text = _playerHouse._rock.ToString();
        _meat.text = _playerHouse._meat.ToString();
        _gold.text = _playerHouse._gold.ToString();

        _content.text = "Nơi chứa tài nguyên quan trọng. Hãy bảo vệ nơi này. nếu bị đánh hỏng sẽ mất tài nguyên và không khôi phục được.";
    }
    public void add(EnemyHuoseController House)
    {
        _playerHouse = null;
        _enemyHouse = House;

        _icon.sprite = _enemyStorageSprite;
        _Lv.text = "";

        _name.text = "Trại Lính";

        _wood.text = _enemyHouse._wood.ToString();
        _rock.text = _enemyHouse._rock.ToString();
        _meat.text = _enemyHouse._meat.ToString();
        _gold.text = _enemyHouse._gold.ToString();

        _content.text = "Nơi bọn quái vật sẽ xuất hiện và tấn công nhà bạn vào lúc nữa đêm. Và là nơi chứa tài nguyên của bọn quái tàn ác.";
    }
}
