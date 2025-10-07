using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [SerializeField] private HouseType _type;
    private bool _IsTower => _type == HouseType.Tower;
    private bool _IsStorage => _type == HouseType.Storage;
    [Header("stas")]
    [SerializeField] private int _level = 1;

    public bool _new = true;

    [SerializeField] private bool _canUpdateHP = true;
    public float _maxHealth = 100;
    public float _currentHealth = 0;
    [SerializeField] private Image _HpImg;

    [ShowIf(nameof(_IsTower))]
    public InTower _inTower;
    [ShowIf(nameof(_IsTower))]
    public ArcherUP _archer;

    [ShowIf(nameof(_IsStorage))]
    public int _wood = 20;
    [ShowIf(nameof(_IsStorage))]
    public int _rock = 20;
    [ShowIf(nameof(_IsStorage))]
    public int _meat = 20;
    [ShowIf(nameof(_IsStorage))]
    public int _gold = 20;

    public UnitAudio _audio;

    protected virtual void Start()
    {
        if (_canUpdateHP)
            _currentHealth = _maxHealth;
        updateHP();
    }

    public void setLevel(int level)
    {
        int _count = 0;
        for (int i = _level; i < level; i++)
        {
            _count++;
            if (_IsTower)
            {
                // damage
                _archer.setDamage(GameManager.Instance.Info._damageBounus);
            }
            if (_IsStorage)
            {
                // current Reference
                _wood += GameManager.Instance.Info._storageBounus;
                _rock += GameManager.Instance.Info._storageBounus;
                _gold += GameManager.Instance.Info._storageBounus;
                _meat += GameManager.Instance.Info._storageBounus;

                if (!_new)
                {
                    Castle.Instance._maxWood += GameManager.Instance.Info._storageBounus;
                    Castle.Instance._maxRock += GameManager.Instance.Info._storageBounus;
                    Castle.Instance._maxMeat += GameManager.Instance.Info._storageBounus;
                    Castle.Instance._maxGold += GameManager.Instance.Info._storageBounus;
                }
            }

            // health
            _maxHealth += GameManager.Instance.Info._buidingHealthBounus;
            _currentHealth += GameManager.Instance.Info._buidingHealthBounus;
        }
        if (_count != 0)
            _audio.PlayLevelUpSound();
        updateHP();
        _level += _count;
    }
    public void updateHP() => _HpImg.fillAmount = _currentHealth / _maxHealth;
    public virtual void setActive(bool amount) { }
    public virtual bool getActive() { return false; }
    public int getLevel() => _level;
}
