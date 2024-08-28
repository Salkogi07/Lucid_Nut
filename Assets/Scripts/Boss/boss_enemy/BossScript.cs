using System;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    //Page_Two
    public dash dashScript;
    public RainEnemy rainAttack;
    public suck_ob suckScript;
    public RandomRockSpawner RainSC;
    public SmallEnemy SmallST;

    //Page_Two
    public Lightning_fire lightningfire;
    public dark_clouds dark_cloud_spawn;


    public int BossHp = 10000;

    private int randN = -1;
    private int randomInt = -1;

    private void Start()
    {
        // Call the ChooseSkill method every 30 seconds
        InvokeRepeating("ChooseSkill", 0f, 5f);
    }

    private void ChooseSkill()
    {
        if (BossHp <= 2000)
        {
            do
            {
                randomInt = UnityEngine.Random.Range(0, 3);
            } while (randN == randomInt);

            randN = randomInt;

            // Use the random integer to decide which skill to use
            page_Two(randomInt);
        }
        else
        {
            do
            {
                randomInt = UnityEngine.Random.Range(0, 4);
            } while (randN == randomInt);

            randN = randomInt;

            // Use the random integer to decide which skill to use
            page_One(randomInt);
        }
    }

    private void page_One(int skillIndex)
    {
        // 스킬을 초기화합니다.
        if (suckScript != null) suckScript.SS = false;
        if (dashScript != null) dashScript.DS = false;
        if (rainAttack != null) rainAttack.RS = false;
        if (RainSC != null) RainSC.SRR = false;
        if (SmallST != null) SmallST.ST = false;

        // 선택된 스킬을 활성화합니다.
        switch (skillIndex)
        {
            case 0:
                if (SmallST != null)
                {
                    SmallST.ST = true;
                }
                break;
            case 1:
                if (dashScript != null)
                {
                    dashScript.DS = true;
                }
                break;
            case 2:
                if (rainAttack != null)
                {
                    rainAttack.RS = true;
                }
                break;
            case 3:
                if (RainSC != null)
                {
                    //suckScript.SS = true;
                    RainSC.SRR = true;
                }
                break;
            default:
                Debug.LogWarning("Invalid skillIndex: " + skillIndex);
                break;
        }
    }

    private void page_Two(int skillIndex)
    {
        // 스킬을 초기화합니다.
        if (lightningfire != null) lightningfire.LS = false;
        if (dark_cloud_spawn != null) dark_cloud_spawn.DCS = false;

        // 선택된 스킬을 활성화합니다.
        switch (skillIndex)
        {
            case 0:
                if (lightningfire != null)
                {
                    lightningfire.LS = true;
                }
                break;
            case 1:
                if (dark_cloud_spawn != null)
                {
                    dark_cloud_spawn.DCS = true;
                }
                break;
            case 2:
                if (rainAttack != null)
                {
                    dashScript.DS = true;
                }
                break;
            default:
                Debug.LogWarning("Invalid skillIndex: " + skillIndex);
                break;
        }
    }
}