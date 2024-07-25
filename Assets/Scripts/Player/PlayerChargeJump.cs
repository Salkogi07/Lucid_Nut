using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeJump : PlayerGroundState
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
        // 점프 입력 처리를 막기 위해 base.Update()를 호출하지 않습니다.
        //base.Update();
    }

    private IEnumerator ChargeJumpCoroutine()
    {
        // 공중에 떠 있는지 검사
        if (!player.IsGroundDetected())
        {
            yield break; // 공중에 떠 있으면 코루틴 종료
        }
        else
        {
            player.isChargeJump = true;
            player.chargeIndicator_back.gameObject.SetActive(true);

            float chargeTime = 0f;
            float maxChargeTime = 1f;

            float decelerationRate = 20f;
            while (player.rb.velocity.x != 0)
            {
                player.rb.velocity = new Vector2(Mathf.MoveTowards(player.rb.velocity.x, 0, decelerationRate * Time.deltaTime), player.rb.velocity.y);
                yield return null;
            }

            while (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.Space) && chargeTime < maxChargeTime)
            {
                // 플레이어가 지면에 있는지 확인
                if (!player.IsGroundDetected())
                {
                    break; // 플레이어가 지면에 없으면 루프 종료
                }
                chargeTime += Time.deltaTime;
                player.chargeIndicator.fillAmount = chargeTime / maxChargeTime;
                yield return null;
            }

            // 지면에 있는지 다시 확인
            if (player.IsGroundDetected() && chargeTime > 0)
            {
                float jumpPower = player.jumpForce * (0.5f + chargeTime / maxChargeTime);
                player.rb.velocity = new Vector2(player.rb.velocity.x, jumpPower);
                player.rb.gravityScale = player.gravityScale;
            }

            player.isChargeJump_inputKey = false;
            player.isChargeJump = false;
            player.chargeIndicator.fillAmount = 0f;
            player.chargeIndicator_back.gameObject.SetActive(false);
            player.stateMachine.ChangeState(player.airState);
        }
    }
}
