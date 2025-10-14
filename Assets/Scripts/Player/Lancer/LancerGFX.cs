using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LancerGFX : PlayerAI
{
    [Header("Lance GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private SpriteRenderer _select;
    [SerializeField] private Transform _pos;
    [SerializeField] private Vector3 _DirectionCheck;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
            Debug.LogError("[LancerGFX] Chưa gán 'SpriteRender'");
        if (!_pos)
            Debug.LogError("[LancerGFX] Chưa gán 'Transform _pos'");

        addCastle(Castle.Instance._ListLancer);
    }

    protected override void Update()
    {
        int _yOder = getOderInLayer();
        _spriteRender.sortingOrder = _yOder;
        _select.sortingOrder = _yOder - 1;
        base.Update();
        enemyDirection();
    }

    public override int getOderInLayer()
        => -(int)(_pos.position.y * 100) + 10000;

    public override void Ai()
    {
        base.Ai();
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
        else if (cacheTarget == null)
        {
            find();
        }

        goToHome(target);
        setupFolow(target);
        if (cacheTarget != null)
        {
            target = cacheTarget;
            farm(target);
        }
        if (target == null) return;
        attack(target);
    }


    private void find()
    {
        cacheTarget = findItems();
        target = cacheTarget;
        if (target != null && _itemScript == null)
        {
            setDetect(false);
            moveToTarget(target, true);
            _itemScript = target.GetComponent<Item>();
            if (_itemScript._type != ItemType.Gold)
            {
                if (!_itemScript.add(this))
                    return;
            }
            else
            {
                if (_itemScript._Farmlist.Count < _itemScript._maxFarmers)
                {
                    bool exists = false;
                    foreach (var hit in _itemScript._Farmlist)
                    {
                        if (hit == this)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        _itemScript._Farmlist.Add(this);
                }
            }
        }
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
