using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayetJumpRoot : MonoBehaviour
{
    public Animator animator;
    private bool sk = true;

    private IEnumerator jump()
    {
        animator.SetBool("yo",true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("yo", false);
    }
    void Update()
    {
        jump();
    }
}
