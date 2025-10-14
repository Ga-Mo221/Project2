using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOrAnimalSelectBox : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemy;
    [SerializeField] private AnimalAI _animal;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _HP_img;
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _content;

    [Header("Icon")]
    [SerializeField] private Image _icon;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _FishSprite;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _GnollSprite;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _LancerSprite;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _MinotaurSprite;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _OrcSprite;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _TNTRedSprite;
    [Foldout("Enemy Sprite")]
    [SerializeField] private Sprite _ShamanSprite;
    [Foldout("Animal Sprite")]
    [SerializeField] private Sprite _BearSprite;
    [Foldout("Animal Sprite")]
    [SerializeField] private Sprite _SheepSprite;
    [Foldout("Animal Sprite")]
    [SerializeField] private Sprite _SnakeSprite;
    [Foldout("Animal Sprite")]
    [SerializeField] private Sprite _SpiderSprite;

    private string key = "";
    private string txt = "";


    void Update()
    {
        if (_enemy != null)
        {
            _HP_img.fillAmount = _enemy._currentHealth / _enemy._maxHealth;
            _health.text = $"{_enemy._currentHealth} / {_enemy._maxHealth}";
        }
        else if (_animal != null)
        {
            _HP_img.fillAmount = _animal._health / _animal._maxHealth;
            _health.text = $"{_animal._health} / {_animal._maxHealth}";
        }
    }

    public void add(EnemyAI enemy)
    {
        _enemy = enemy;
        _animal = null;
        changeSpriteEnemy(_enemy._type);

        _damage.text = _enemy._damage.ToString();
        _speed.text = _enemy._speed.ToString();
    }

    public void add(AnimalAI animal)
    {
        _animal = animal;
        _enemy = null;
        changeSpriteAnimal(_animal._type);

        _damage.text = _animal._damage.ToString();
        _speed.text = _animal._speed.ToString();
    }

    private void changeSpriteEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Lancer:
                key = "Name.enemy.lancer";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _LancerSprite;
                key = "Name.enemy.lancerContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case EnemyType.Fish:
                key = "Name.enemy.fish";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _FishSprite;
                key = "Name.enemy.fishContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case EnemyType.Gnoll:
                key = "Name.enemy.gnoll";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _GnollSprite;
                key = "Name.enemy.gnollContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case EnemyType.Orc:
                key = "Name.enemy.orc";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _OrcSprite;
                key = "Name.enemy.orcContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case EnemyType.TNT:
                key = "Name.enemy.tnt";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _TNTRedSprite;
                key = "Name.enemy.tntContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case EnemyType.Minotaur:
                key = "Name.enemy.minotaur";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _MinotaurSprite;
                key = "Name.enemy.minotaurContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case EnemyType.Shaman:
                key = "Name.enemy.shaman";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _ShamanSprite;
                key = "Name.enemy.shamanContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
        }
    }

    private void changeSpriteAnimal(AnimalClass type)
    {
        switch (type)
        {
            case AnimalClass.Bear:
                key = "Name.animal.Bear";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _BearSprite;
                key = "Name.animal.BearContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case AnimalClass.Sheep:
                key = "Name.animal.sheep";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _SheepSprite;
                key = "Name.animal.sheepContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case AnimalClass.Snake:
                key = "Name.animal.snack";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _SnakeSprite;
                key = "Name.animal.snackContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
            case AnimalClass.Spider:
                key = "Name.animal.spider";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _name.text = txt;

                _icon.sprite = _SpiderSprite;
                key = "Name.animal.spiderContent";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
                _content.text = txt;
                break;
        }
    }
}
