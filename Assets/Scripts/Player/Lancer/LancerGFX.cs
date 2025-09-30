using Unity.Mathematics;
using UnityEngine;

public class LancerGFX : PlayerAI
{
    [Header("Lance GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private Transform _pos;
    [SerializeField] private Vector3 _DirectionCheck;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[LancerGFX] Chưa gán 'SpriteRender'");
        if (!_pos)
            Debug.LogError("[LancerGFX] Chưa gán 'Transform _pos'");
    }

    protected override void Update()
    {
        _spriteRender.sortingOrder = -(int)(_pos.position.y * 100) + 10000;
        base.Update();
        
        if (!getIsAI()) return;
        target = findEnemys();
        if (target == null)
        {
            target = findAnimals();
            if (target == null)
                target = findEnemyHouse();
        }
        if (target != null)
        {
            setDetect(true);
            moveToTarget(target);
        }
        if (target == null)
        {
            target = findItems();
            if (target != null)
            {
                setDetect(false);
                moveToTarget(target);
                _itemScript = target.GetComponent<Item>();
                if (_itemScript._type != ItemType.Gold)
                    _itemScript._seleted = true;
                else
                {
                    if (_itemScript._Farmlist.Count < _itemScript._maxFarmers)
                    {
                        bool see = false;
                        foreach (var hit in _itemScript._Farmlist)
                            if (hit == this)
                                see = true;
                        if (see)
                            _itemScript._Farmlist.Add(this);
                    }
                }
            }
        }

        goToHome(target);
        setupFolow(target);
        if (target == null) return;
        farm(target);
        enemyDirection();
        attack(target);
    }

    private void enemyDirection()
    {
        if (target != null)
        {
            float _neX = target.transform.position.x;
            float _neY = target.transform.position.y;
            float _X = _pos.position.x;
            float _Y = _pos.position.y;
            float _xdis = math.abs(math.abs(_neX) - math.abs(_X));
            if (_xdis > _DirectionCheck.x / 2)
            {
                if (_neY > _Y + (_DirectionCheck.y / 2))
                    setAnimDirection(2);
                else if (_neY < _Y - (_DirectionCheck.y / 2))
                    setAnimDirection(4);
                else
                    setAnimDirection(3);
            }
            else if (_xdis < _DirectionCheck.x / 2)
            {
                if (_neY > _Y + (_DirectionCheck.y / 2))
                    setAnimDirection(1);
                else if (_neY < _Y - (_DirectionCheck.y / 2))
                    setAnimDirection(5);
                else
                    setAnimDirection(3);
            }
        }
    }

    private void setAnimDirection(int i) => _anim.SetInteger("Direction", i);

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, _DirectionCheck);
    }

    public void offDetec()
        => setDetect(false);
    public void onDetec()
        => setDetect(true);
}
