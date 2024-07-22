using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    private float downArrowLastPressedTime = -1f;
    private float aKeyLastPressedTime = -1f;

    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = player.gravityScale;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.CanJump())
            stateMachine.ChangeState(player.jumpState);

        if (player.isUmbrellaOpen && player.IsGroundDetected())
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                downArrowLastPressedTime = Time.time;
                CheckSimultaneousInput();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                aKeyLastPressedTime = Time.time;
                CheckSimultaneousInput();
            }

            player.chargeIndicator.fillAmount = 0f;
        }

        void CheckSimultaneousInput()
        {
            if (Mathf.Abs(downArrowLastPressedTime - aKeyLastPressedTime) <= player.inputThreshold)
            {
                stateMachine.ChangeState(player.chargeJump);
            }
        }
    }
}