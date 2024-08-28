using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornado_bHp : MonoBehaviour
{
    public bool attBoss = false;
    public GameObject boss;
    private BossScript bossScript;

    private void Start()
    {
        boss = GameObject.Find("Tornado_boss");
        bossScript = boss.GetComponent<BossScript>();
        
    }

    public void Update()
    {
        if (attBoss)
        {
            Debug.Log(attBoss);
            bossScript.BossHp = bossScript.BossHp - 100;
            Debug.Log(bossScript.BossHp);
            attBoss = false;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(boss);
            Debug.Log(bossScript);
        }
    }
}
