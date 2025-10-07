using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectBox : MonoBehaviour
{
    [SerializeField] private PlayerAI _player;
    private bool _normal;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _warriorSprite;
    [SerializeField] private Sprite _archerSprite;
    [SerializeField] private Sprite _lancerSprite;
    [SerializeField] private Sprite _healerSprite;
    [SerializeField] private Sprite _TNTSprite;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _Lv;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private TextMeshProUGUI _damge;
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _slot;

    [SerializeField] private GameObject _referentces;
    [SerializeField] private TextMeshProUGUI _wood;
    [SerializeField] private TextMeshProUGUI _rock;
    [SerializeField] private TextMeshProUGUI _meat;
    [SerializeField] private TextMeshProUGUI _gold;


    void Update()
    {
        if (_player != null)
        {
            _HP_img.fillAmount = _player._health / _player._maxHealth;
            _health.text = $"{_player._health} / {_player._maxHealth}";

            if (_normal)
            {
                _wood.text = $"{_player._wood}/{_player._maxWood}";
                _rock.text = $"{_player._rock}/{_player._maxRock}";
                _meat.text = $"{_player._meat}/{_player._maxMeat}";
                _gold.text = $"{_player._gold}/{_player._maxGold}";
            }
        }
    }

    public void add(PlayerAI player)
    {
        _player = player;
        changeSprite(_player._unitClass);

        _referentces.SetActive(_normal);

        _Lv.text = $"LV.{_player._level}";

        _damge.text = _player._damage.ToString();
        _speed.text = _player._maxSpeed.ToString();
        _slot.text = _player._slot.ToString();
    }

    private void changeSprite(UnitType type)
    {
        switch (type)
        {
            case UnitType.Warrior:
                _icon.sprite = _warriorSprite;
                _name.text = "Dũng Sĩ";
                _normal = true;
                break;
            case UnitType.Archer:
                _icon.sprite = _archerSprite;
                _name.text = "Cung Thủ";
                _normal = true;
                break;
            case UnitType.Lancer:
                _icon.sprite = _lancerSprite;
                _name.text = "Thương Sĩ";
                _normal = true;
                break;
            case UnitType.Healer:
                _icon.sprite = _healerSprite;
                _name.text = "Pháp Sư Phục Hồi";
                _normal = false;
                break;
            case UnitType.TNT:
                _icon.sprite = _TNTSprite;
                _name.text = "Boom Cảm Tử";
                _normal = false;
                break;
        }
    }
}
