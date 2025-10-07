using NaughtyAttributes;
using UnityEngine;

public class OutLine : MonoBehaviour
{
    [SerializeField] private NodeType _type;
    private bool _IsStorage => _type == NodeType.Storage;
    private bool _IsTower => _type == NodeType.Tower;
    private bool _IsStorageOrTower => _type == NodeType.Storage || _type == NodeType.Tower;
    private bool _IsEnemyHouse => _type == NodeType.EnemyHouse;
    private bool _IsEnemyTower => _type == NodeType.EnemyTower;
    private bool _IsEnemyStorage => _type == NodeType.EnemyStorage;
    private bool _IsEnemyHouseAll => _type == NodeType.EnemyHouse || _type == NodeType.EnemyTower || _type == NodeType.EnemyStorage;
    private bool _IsPlayer => _type == NodeType.Player;
    private bool _IsAnimal => _type == NodeType.Animal;
    private bool _IsEnemy => _type == NodeType.Enemy;
    private bool _IsTreeOrRockOrGoldMine => _type == NodeType.TreeOrRock || _type == NodeType.GoldMine;
    private bool _isTreeOrRock => _type == NodeType.TreeOrRock;
    private bool _IsGoldMine => _type == NodeType.GoldMine;

    [ShowIf(nameof(_IsStorageOrTower))]
    [SerializeField] private House _playerHouse;

    [ShowIf(nameof(_IsEnemyHouseAll))]
    [SerializeField] private EnemyHuoseController _enemyHouse;

    [ShowIf(nameof(_IsPlayer))]
    [SerializeField] private PlayerAI _player;

    [ShowIf(nameof(_IsAnimal))]
    [SerializeField] private AnimalAI _animal;

    [ShowIf(nameof(_IsEnemy))]
    [SerializeField] private EnemyAI _enemy;

    [ShowIf(nameof(_IsTreeOrRockOrGoldMine))]
    [SerializeField] private Item _item;


    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _outLineMateral;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRender;

    private bool _isHovering = false;

    #region Start
    void Start()
    {
        if (!_spriteRender || _spriteRender == null)
            _spriteRender = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        if (!_spriteRender || _spriteRender == null)
            Debug.LogError($"[{transform.parent.name}] [OutLine] Chưa gán 'SpriteRender'");
    }
    #endregion


    #region Update
    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_collider.OverlapPoint(mouseWorldPos))
        {
            if (!_isHovering) // chỉ đổi khi mới hover lần đầu
            {
                ChangeMaterial();
                _isHovering = true;
            }
        }
        else
        {
            if (_isHovering) // reset khi chuột rời khỏi
            {
                ResetMaterial();
                _isHovering = false;
            }
        }

        if (!_isHovering) return;
        if (GameManager.Instance.getCanOpenWindown())
        {
            CastleClick();
            clickOther();
        }
    }
    #endregion


    #region Change Material
    private void ChangeMaterial()
    {
        if (_spriteRender.enabled == false) return;
        _outLineMateral.SetColor("_Color", Color.red);
        _outLineMateral.SetFloat("_Size", 3.0f);

        if (_spriteRender != null && _spriteRender.sprite != null)
        {
            Texture2D tex = _spriteRender.sprite.texture;
            _outLineMateral.SetTexture("_MainTex", tex);
        }

        _spriteRender.material = _outLineMateral;
        CursorManager.Instance.SetSelectCursor(transform.parent.gameObject);
    }
    #endregion


    #region  Reset Material
    private void ResetMaterial()
    {
        if (_spriteRender.enabled == false) return;
        _spriteRender.material = _normalMaterial;
        CursorManager.Instance.SetNormalCursor();
    }
    #endregion


    #region  Castle Click
    private void CastleClick()
    {
        if (_type != NodeType.Castle) return;
        if (Input.GetMouseButtonDown(0) && _type == NodeType.Castle)
            GameManager.Instance.UIsetActiveButtonUpgrade(true);
    }
    #endregion

    private void clickOther()
    {
        if (_type == NodeType.Other || _type == NodeType.Castle) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_IsTower)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openPlayerTower(_playerHouse);
            }
            if (_IsStorage)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openPlayerStorage(_playerHouse);
            }
            if (_IsEnemyHouse)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openEnemyHouse(_enemyHouse);
            }
            if (_IsEnemyTower)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openEnemyTower(_enemyHouse);
            }
            if (_IsEnemyStorage)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openEnemyStorage(_enemyHouse);
            }
            if (_IsPlayer)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openPlayer(_player);
            }
            if (_IsAnimal)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openAnimal(_animal);
            }
            if (_IsEnemy)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openEnemy(_enemy);
            }
            if (_isTreeOrRock)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openTreeOrRock(_item);
            }
            if (_IsGoldMine)
            {
                GameManager.Instance._selectBox.gameObject.SetActive(true);
                GameManager.Instance._selectBox.openGold(_item);
            }
        }
    }
}

public enum NodeType
{
    Other,
    Castle,
    Tower,
    Storage,
    Player,
    TreeOrRock,
    GoldMine,
    Animal,
    Enemy,
    EnemyHouse,
    EnemyTower,
    EnemyStorage
}
