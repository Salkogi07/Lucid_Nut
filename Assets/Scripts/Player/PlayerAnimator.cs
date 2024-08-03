using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private string lastAnimation = null;

    public void PlayAnimation(string anim)
    {
        if (lastAnimation != anim)
        {
            animator.Play(anim);
            lastAnimation = anim;
        }
    }
}
