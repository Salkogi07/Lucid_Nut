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
        player.isChargeJump = true;
        player.chargeIndicator_back.gameObject.SetActive(true);

        float chargeTime = 0f;
        float maxChargeTime = 1f;

        float decelerationRate = 10f;
        while (player.rb.velocity.x != 0)
        {
            player.rb.velocity = new Vector2(Mathf.MoveTowards(player.rb.velocity.x, 0, decelerationRate * Time.deltaTime), player.rb.velocity.y);
            yield return null;
        }

        while (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.A) && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;
            player.chargeIndicator.fillAmount = chargeTime / maxChargeTime;
            yield return null;
        }

        if (chargeTime > 0)
        {
            float jumpPower = player.jumpForce * (0.5f + chargeTime / maxChargeTime);
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            rb.gravityScale = player.gravityScale;
        }

        player.isChargeJump = false;
        player.chargeIndicator.fillAmount = 0f;
        player.chargeIndicator_back.gameObject.SetActive(false);
        stateMachine.ChangeState(player.airState);
    }
}