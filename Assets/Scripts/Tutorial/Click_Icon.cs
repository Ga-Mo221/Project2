using UnityEngine;

public class Click_Icon : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private Transform _target;
    

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }


    public void setTarget(Transform target)
    {
        _target = target;

        transform.position = getPos();
    }


    private Vector2 getPos()
    {
        Vector2 pos = _target.transform.position;
        return pos;
    }
}
