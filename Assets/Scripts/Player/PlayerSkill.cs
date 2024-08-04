using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerAnimator playerAnimator;
    Rigidbody2D rb;
    TrailRenderer tr;

    [Header("Dash info")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;


    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && playerMove.canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        playerMove.canDash = false;
        playerMove.isDashing = true;
        playerAnimator.PlayAnimation("Dash");
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * -1, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        playerMove.isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        playerMove.canDash = true;
    }
}
