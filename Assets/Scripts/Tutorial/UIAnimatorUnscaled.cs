// Gắn script này lên UI có Animator
using UnityEngine;

public class UIAnimatorUnscaled : MonoBehaviour
{
    private void Awake()
    {
        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
}
