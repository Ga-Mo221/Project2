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
                _name.text = "Yêu Tinh Xanh";
                _icon.sprite = _LancerSprite;
                _content.text = "Những chiến binh lùn tịt nhưng lì lợm, luôn xông lên với ngọn giáo cùn. Chúng không mạnh, nhưng cực kỳ đông và phiền phức.";
                break;
            case EnemyType.Fish:
                _name.text = "Cá Mập Mắc Cạn";
                _icon.sprite = _FishSprite;
                _content.text = "Sinh vật từng thống trị biển sâu, nay bị dòng hắc ám biến dị và lang thang trên đất liền, trông vừa đáng sợ vừa… buồn cười.";
                break;
            case EnemyType.Gnoll:
                _name.text = "Linh Cẩu";
                _icon.sprite = _GnollSprite;
                _content.text = "Loài ăn xác sống thích cười khanh khách. Đừng để tiếng cười của chúng đánh lừa, bởi móng vuốt và hàm răng ấy có thể xé toạc xương thịt.";
                break;
            case EnemyType.Orc:
                _name.text = "Mập Xanh";
                _icon.sprite = _OrcSprite;
                _content.text = "Khổng lồ da xanh, bắp tay to như thân cây. Chúng không cần chiến lược, chỉ cần một cú đấm là đủ làm rung chuyển mặt đất.";
                break;
            case EnemyType.TNT:
                _name.text = "Thuốc Nổ Di Động";
                _icon.sprite = _TNTRedSprite;
                _content.text = "Quái vật dị hợm mang trên người những thùng thuốc nổ. Đánh nó thì chết, để nó lại gần thì càng chết. Chỉ có cách chạy hoặc dụ nó nổ.";
                break;
            case EnemyType.Minotaur:
                _name.text = "Ngưu Ca (Boss)";
                _icon.sprite = _MinotaurSprite;
                _content.text = "Một con quái thú nửa người nửa bò, cư trú trong tàn tích. Hắn cầm rìu khổng lồ, mỗi bước đi như dội sấm vào đất.";
                break;
            case EnemyType.Shaman:
                _name.text = "Phù Thủy (Boss)";
                _icon.sprite = _ShamanSprite;
                _content.text = "Kẻ điều khiển năng lượng hắc ám, ẩn sau tấm mặt nạ già nua. Những lời chú ngữ của hắn khiến đất trời méo mó, kẻ địch tan xác.";
                break;
        }
    }

    private void changeSpriteAnimal(AnimalClass type)
    {
        switch (type)
        {
            case AnimalClass.Bear:
                _name.text = "Gấu Nô Ngáo";
                _icon.sprite = _BearSprite;
                _content.text = "Một con gấu to xác, thường lang thang tìm mật ong nhưng hay ngủ quên giữa chiến trường. Khi nổi điên, nó vả cả cây rừng bật gốc.";
                break;
            case AnimalClass.Sheep:
                _name.text = "Cừu Nhát Gan";
                _icon.sprite = _SheepSprite;
                _content.text = "Loài vật hiền lành, run rẩy trước tiếng động lớn. Tuy nhiên, đừng coi thường, vì khi dồn ép, nó có thể tông cả nhóm quái ngã rạp.";
                break;
            case AnimalClass.Snake:
                _name.text = "Rắn 2K1";
                _icon.sprite = _SnakeSprite;
                _content.text = "Nhanh nhẹn, trơn tuột và thích lao vào từ bóng tối. Bị gọi là '2K1' bởi lúc nào cũng thích 'cắn lén' mà không chịu đối mặt trực diện.";
                break;
            case AnimalClass.Spider:
                _name.text = "Nhền Nhện";
                _icon.sprite = _SpiderSprite;
                _content.text = "Ẩn mình trong bóng tối và giăng lưới chờ mồi. Chúng không mạnh, nhưng mạng nhện của chúng sẽ khiến bất cứ ai cũng thấy khó thở.";
                break;
        }
    }
}
