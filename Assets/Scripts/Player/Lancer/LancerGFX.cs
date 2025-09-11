using Unity.Mathematics;
using UnityEngine;

public class LancerGFX : PlayerAI
{
    [Header("Lance GFX")]
    [SerializeField] private SpriteRenderer _spriteRender;
    private GameObject _nearest;
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
        _spriteRender.sortingOrder = -(int)(_pos.position.y * 100);
        _nearest = flip();
        enemyDirection();
        if (getIsAI())
        {
            setupFolow();
            if (!checkFullInventory() && !getIsTarget())
            {
                if (!findTargetMoveTo())
                    if (!getIsLock() && !getDetect())
                        findItem();
            }
            farm();
            attack();
        }
    }

    private void enemyDirection()
    {
        if (_nearest != null)
        {
            float _neX = _nearest.transform.position.x;
            float _neY = _nearest.transform.position.y;
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

    public void offCanAttack()
        => _canAttack = false;
    public void onCanAttack()
        => _canAttack = true;
}
