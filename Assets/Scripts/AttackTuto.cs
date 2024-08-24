using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTuto : MonoBehaviour
{
    public Animator animator;
    public bool attackP = false;

    // Update is called once per frame
    void Update()
    {
        if (attackP)
        {
            animator.SetBool("Attack", true);
            StartCoroutine(time());
        }
    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("Attack", false);
        attackP = false;
    }
}
