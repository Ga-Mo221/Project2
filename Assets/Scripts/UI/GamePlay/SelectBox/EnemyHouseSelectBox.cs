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

        _name.text = "Danh trại Quái Vật";

        _content.text = "Nơi đây là đầu sỏ của bọn quái vật. Bạn chỉ cần phá hủy nó là bọn quái vật sẽ tự rút lui khỏi lảnh thổ của bạn.";
    }
}
