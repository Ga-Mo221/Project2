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
                    _name.text = "Cây Lá Kim Thấp";
                    _icon.sprite = _tree1;
                    _iconValue.sprite = _icontree;
                    _content.text = "Loài cây mọc chen chúc ở vùng đất lạnh, gỗ tuy nhỏ nhưng dẻo dai, thường dùng để dựng cột hoặc vũ khí đơn giản.";
                    break;
                case 2:
                    _name.text = "Cây Lá Kim Cao";
                    _icon.sprite = _tree2;
                    _iconValue.sprite = _icontree;
                    _content.text = "Vươn thẳng lên trời như mũi giáo khổng lồ. Người ta tin rằng linh hồn cổ xưa trú ngụ trong từng vòng gỗ của nó.";
                    break;
                case 3:
                    _name.text = "Cây Lá Vàng";
                    _icon.sprite = _tree3;
                    _iconValue.sprite = _icontree;
                    _content.text = "Màu vàng rực rỡ như ngọn lửa mùa thu, gỗ của nó nhẹ nhưng dễ cháy. Chặt một cây, lá vàng bay như mưa.";
                    break;
                case 4:
                    _name.text = "Cây Lá Cam";
                    _icon.sprite = _tree4;
                    _iconValue.sprite = _icontree;
                    _content.text = "Hiếm thấy, tỏa ra sắc cam ấm áp như mặt trời chiều. Dân gian đồn rằng ai ngủ dưới bóng cây này sẽ mơ thấy tương lai.";
                    break;
            }
        }
        else if (type == ItemType.Rock)
        {
            if (id == 1)
            {
                _name.text = "Đá Cuội Nhỏ";
                _icon.sprite = _rock1;
                _iconValue.sprite = _iconrock;
                _content.text = "Viên đá phổ biến, có thể dùng làm công cụ thô sơ hoặc ném lũ quái cho vui. Thường nằm rải rác khắp nơi.";
            }
            else if (id == 2)
            {
                _name.text = "Đá Cuội Lớn";
                _icon.sprite = _rock2;
                _iconValue.sprite = _iconrock;
                _content.text = "Những tảng đá to như bắp đùi, nặng nề nhưng vững chắc. Người thợ rèn ưa thích chúng để làm nền cho lò rèn.";
            }
        }
    }
}
