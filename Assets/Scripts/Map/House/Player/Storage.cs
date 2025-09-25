using UnityEngine;

public class Storage : House
{
    [SerializeField] private bool _active = false;
    private Animator _anim;
    [SerializeField] private Transform _inPos1;
    [SerializeField] private Transform _inPos2;
    [SerializeField] private Transform _inPos3;

    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
    }

    public Vector3 getInPos()
    {
        int type = _anim.GetInteger("Type");
        if (type == 1)
            return _inPos1.position;
        else if (type == 2)
            return _inPos2.position;
        else
            return _inPos3.position;
    }

    public override void setActive(bool amount) => _active = amount;
    public override bool getActive() => _active;
}
