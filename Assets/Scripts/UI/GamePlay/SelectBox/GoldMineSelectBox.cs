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

    private string key = "";
    private string txt = "";

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
        key = "Name.item.goldmine";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _name.text = txt;

        key = "Name.item.goldminecontent";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _content.text = string.Format(txt, _item._valueOneDrop, _item._maxStack, _item._maxFarmers);
    }
}
