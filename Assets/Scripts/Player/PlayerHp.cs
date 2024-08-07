using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    private int player_HP = 0;
    private int player_maxHP = 100;
    private bool isDead;

    [SerializeField] Image player_HpBar;
    [SerializeField] Text player_HpTxt;

    void Start()
    {
        player_HP = player_maxHP;
        Set_HP(player_HP);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("attack"))
        {
            Debug.Log("공격받음");
            Change_HP(-10);
        }
    }

    public void Change_HP(int _value)
    {
        player_HP += _value;
        Set_HP(player_HP);
    }

    private void Set_HP(int _value)
    {
        player_HP = _value;

        string txt = "";
        if (player_HP <= 0)
        {
            player_HP = 0;
            txt = "Dead";
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
