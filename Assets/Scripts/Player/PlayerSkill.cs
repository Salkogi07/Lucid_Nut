using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerAnimator_M playerAnimator;
    Rigidbody2D rb;
    TrailRenderer tr;

    [Header("Dash info")]
    [SerializeField] public bool canDash = true;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [Header("Umbrella info")]
    [SerializeField] public bool isUmbrellaOpen = false;
    [SerializeField] private float UmbrellaCoolTime = 0.8f;
    [SerializeField] private float UmbrellaTime = 0.8f;
    [SerializeField] public float umbrellaFallMultiplier = 0.5f;

    public GameObject Um;

    public Image Umbtn;
    public Sprite UmF;
    public Sprite UmT;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAnimator = GetComponent<PlayerAnimator_M>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && canDash)
        {
            StartCoroutine(Dash());
        }

        if(UmbrellaTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.S) && playerMove.canUmbrella())
            {
                isUmbrellaOpen = !isUmbrellaOpen;
                UmbrellaTime = UmbrellaCoolTime;
                Umbtn.sprite = isUmbrellaOpen ? UmT : UmF;
                Um.SetActive(isUmbrellaOpen);
            }
        }
        else
        {
            UmbrellaTime -= Time.deltaTime;
        }
    }

    public void DashBtn()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void UmbrellaBtn()
    {
        if (playerMove.canUmbrella())
        {
            isUmbrellaOpen = !isUmbrellaOpen;
            UmbrellaTime = UmbrellaCoolTime;
            Umbtn.sprite = isUmbrellaOpen ? UmT : UmF;
            Um.SetActive(isUmbrellaOpen);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
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
        canDash = true;
    }
}
