using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int player_HP = 0;
    private int player_maxHP = 100;
    private bool isDead;
    public float invincibilityDuration = 0.5f;
    public GameObject effect;
    public ShakeCam Shaking;

    [SerializeField] Image player_HpBar;
    [SerializeField] Text player_HpTxt;
    public GameObject menu;

    PlayerMove playerMove;
    SpriteRenderer sprite;
    Rigidbody2D rb;

    public bool isInvincible  = false;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        player_HP = player_maxHP;
        Set_HP(player_HP);
    }

    public void Change_HP(int _value)
    {
        player_HP += _value;
        Set_HP(player_HP);
    }

    public void Damage_HP(int _value)
    {
        if (isInvincible)
            return;

        if (!playerMove.isDashing)
        {
            Shaking.Shake();
            GameObject effectObj = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(effectObj, 0.5f);
            player_HP -= _value;
            Set_HP(player_HP);

            // 무적 상태 시작
            StartCoroutine(InvincibilityCoroutine());

        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        sprite.color = new Color(1, 1, 1, 1f);
    }


    private void Set_HP(int _value)
    {
        player_HP = _value;

        string txt = "";
        if (player_HP <= 0)
        {
            player_HP = 0;
            txt = "Dead";
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            if (player_HP > player_maxHP)
                player_HP = player_maxHP;
            txt = string.Format("{0}/{1}", player_HP, player_maxHP);
        }
        player_HpBar.fillAmount = (float)player_HP / player_maxHP;
        isDead = player_HP.Equals(0);

        player_HpTxt.text = txt;
    }
}
