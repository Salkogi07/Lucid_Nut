using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeJump : PlayerState
{
    public PlayerChargeJump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StartCoroutine(ChargeJumpCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    private IEnumerator ChargeJumpCoroutine()
    {
        player.isChargeJump = true; // 차징 점프 시작
        player.chargeIndicator_back.gameObject.SetActive(true);

        float chargeTime = 0f;
        float maxChargeTime = 1f; // 최대 차징 시간 (예시로 2초로 설정)

        // 움직임을 천천히 멈추는 로직
        float decelerationRate = 10f; // 감속 비율
        while (player.rb.velocity.x != 0)
        {
            player.rb.velocity = new Vector2(Mathf.MoveTowards(player.rb.velocity.x, 0, decelerationRate * Time.deltaTime), player.rb.velocity.y);
            yield return null;
        }

        // 차징 시간 동안 키를 누르고 있으면 점프 힘을 축적
        while (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.A) && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;

            // UI의 fill amount 업데이트
            player.chargeIndicator.fillAmount = chargeTime / maxChargeTime;

            yield return null;
        }

        // 차징 시간이 끝나면 점프 실행
        if (chargeTime > 0)
        {
            float jumpPower = player.jumpForce * (0.5f + chargeTime / maxChargeTime); // 차징 시간에 따라 점프 힘 조정
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            rb.gravityScale = player.gravityScale;
            //player.coyoteTimeCounter = 0; // 코요테 타임 초기화
        }

        player.isChargeJump = false; // 차징 점프 종료

        // UI fill amount 초기화
        player.chargeIndicator.fillAmount = 0f;
        player.chargeIndicator_back.gameObject.SetActive(false);
        stateMachine.ChangeState(player.airState);
    }
}
