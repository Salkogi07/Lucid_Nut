using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    PlayerMove playerMove;
    Animator animator;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
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

    public void Update()
    {
        // 현재 애니메이션 상태 정보를 가져옴
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 특정 애니메이션이 재생 중인지 확인
        if (stateInfo.IsName("Attack"))
        {
            // 애니메이션이 끝났는지 확인 (normalizedTime이 1.0f 이상이면 애니메이션이 종료된 것)
            if (stateInfo.normalizedTime >= 1.0f)
            {
                playerMove.isAttack = false;   
            }
        }
    }
}
