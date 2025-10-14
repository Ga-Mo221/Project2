using UnityEngine;

public class Button_Tutorial_SetAnimation : MonoBehaviour
{
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _anim.SetBool("setting", false);
    }
}
