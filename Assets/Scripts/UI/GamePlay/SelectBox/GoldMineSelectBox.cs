using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldMineSelectBox : MonoBehaviour
{
    [SerializeField] private Item _item;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private TextMeshProUGUI _content;

    void Update()
    {
        if (_item != null)
        {
            _HP_img.fillAmount = (float)_item._value / _item._maxValue;
            _health.text = $"{_item._value} / {_item._maxValue}";
        }
    }

    public void add(Item item)
    {
        _item = item;
        _name.text = "Mỏ Vàng";
        _content.text = $"Mỏ Vàng sẽ cho sản lượng({_item._valueOneDrop}) mỗi {_item._maxStack} lần đánh. Và tối đa {_item._maxFarmers} người được farm cùng lúc.";
    }
}
