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
        // ���� �Է� ó���� ���� ���� base.Update()�� ȣ������ �ʽ��ϴ�.
        //base.Update();
    }

    private IEnumerator ChargeJumpCoroutine()
    {
        // ���߿� �� �ִ��� �˻�
        if (!player.IsGroundDetected())
        {
            yield break; // ���߿� �� ������ �ڷ�ƾ ����
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
                // �÷��̾ ���鿡 �ִ��� Ȯ��
                if (!player.IsGroundDetected())
                {
                    break; // �÷��̾ ���鿡 ������ ���� ����
                }
                chargeTime += Time.deltaTime;
                player.chargeIndicator.fillAmount = chargeTime / maxChargeTime;
                yield return null;
            }

            // ���鿡 �ִ��� �ٽ� Ȯ��
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
