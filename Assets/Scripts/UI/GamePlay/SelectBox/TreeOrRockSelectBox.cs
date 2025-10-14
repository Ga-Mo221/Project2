using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeOrRockSelectBox : MonoBehaviour
{
    [SerializeField] private Item _item;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _tree1;
    [SerializeField] private Sprite _tree2;
    [SerializeField] private Sprite _tree3;
    [SerializeField] private Sprite _tree4;
    [SerializeField] private Sprite _rock1;
    [SerializeField] private Sprite _rock2;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _stack;

    [SerializeField] private Image _iconValue;
    [SerializeField] private Sprite _icontree;
    [SerializeField] private Sprite _iconrock;

    [SerializeField] private TextMeshProUGUI _value;

    [SerializeField] private TextMeshProUGUI _content;

    private string key = "";
    private string txt = "";

    void Update()
    {
        if (_item != null)
        {
            _HP_img.fillAmount = (float)_item._stack / _item._maxStack;
            _stack.text = $"{_item._stack} / {_item._maxStack}";
        }
    }

    public void add(Item item)
    {
        _item = item;
        _value.text = _item._maxValue.ToString();

        changeSpriteItem(_item._type, _item._id);
    }


    private void changeSpriteItem(ItemType type, int id)
    {
        if (type == ItemType.Tree)
        {
            switch (id)
            {
                case 1:
                    key = "Name.item.tree1";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _name.text = txt;
                    _icon.sprite = _tree1;
                    _iconValue.sprite = _icontree;

                    key = "Name.item.tree1content";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _content.text = txt;
                    break;
                case 2:
                    key = "Name.item.tree2";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _name.text = txt;
                    _icon.sprite = _tree2;
                    _iconValue.sprite = _icontree;

                    key = "Name.item.tree2content";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _content.text =txt;
                    break;
                case 3:
                    key = "Name.item.tree3";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _name.text = txt;
                    _icon.sprite = _tree3;
                    _iconValue.sprite = _icontree;

                    key = "Name.item.tree3content";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _content.text = txt;
                    break;
                case 4:
                    key = "Name.item.tree4";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _name.text = txt;
                    _icon.sprite = _tree4;
                    _iconValue.sprite = _icontree;

                    key = "Name.item.tree4content";
                    txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                    _content.text = txt;
                    break;
            }
        }
        else if (type == ItemType.Rock)
        {
            if (id == 1)
            {
                key = "Name.item.rock1";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;
                _icon.sprite = _rock1;
                _iconValue.sprite = _iconrock;

                key = "Name.item.rock1content";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
            }
            else if (id == 2)
            {
                key = "Name.item.rock2";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;
                _icon.sprite = _rock2;
                _iconValue.sprite = _iconrock;

                key = "Name.item.rock2content";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
            }
        }
    }
}
