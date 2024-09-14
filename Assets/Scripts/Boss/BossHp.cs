using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    [SerializeField] private int hp = 0;
    [SerializeField] private int maxHp = 5000;

    [SerializeField]
    public Image BossHP;

    float imsi;
    
    void Start()
    {
        hp = maxHp;
        BossHP.fillAmount = (float)hp / (float)maxHp;
    }

    public void TakeDamage(int value)
    {
        Debug.Log(value);
        Debug.Log(Mathf.Clamp(hp - value, 0, maxHp));
        hp = Mathf.Clamp(hp - value, 0, maxHp);
    }

    void Update()
    {
        imsi = (float)hp / (float)maxHp;
        HandleHP();
    }

    private void HandleHP()
    {
        BossHP.fillAmount = Mathf.Lerp(BossHP.fillAmount, imsi, Time.deltaTime * 10);
    }
}
