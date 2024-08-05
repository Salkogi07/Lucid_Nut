using System;
using UnityEngine;

public class FinalBossScript : MonoBehaviour
{
    //Page_Two
    public darkness_unfolds darkness_U;         //암흑으로 뒤덮는 기술
    public SpawnT spTornado;                    //토네이도 소환 기술
    public RandomObjectSpawner randScratch;     //랜덤하게 할퀴는 기술
    public BlankBandAttack BlankandAttack;      //밑으로 사라진뒤 순간 위로 올라오는 기술
    public RandomRockSpawners energe_charge;    //에너지를 차지했다가 날리는 기술
    public feather_Attack fAttack;

    //Page_Two


    public int BossHp = 10000;

    private int randN = -1;
    private int randomInt = -1;

    private void Start()
    {
        // Call the ChooseSkill method every 30 seconds
        InvokeRepeating("ChooseSkill", 0f, 10f);
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
            //page_Two(randomInt);
        }
        else
        {
            do
            {
                randomInt = UnityEngine.Random.Range(0, 6);
            } while (randN == randomInt);

            randN = randomInt;

            // Use the random integer to decide which skill to use
            page_One(randomInt);
        }
    }

    private void page_One(int skillIndex)
    {
        // 스킬을 초기화합니다.
        if (darkness_U != null) darkness_U.Darkness_Unfolds = false;
        if (spTornado != null) spTornado.spawnT = false;
        if (randScratch != null) randScratch.RandScratch = false;
        if (BlankandAttack != null) BlankandAttack.isActive = false;
        if (energe_charge != null) energe_charge.SRR = false;
        if (fAttack != null) fAttack.feather_attack = false;

        // 선택된 스킬을 활성화합니다.
        switch (skillIndex)
        {
            case 0:
                if (darkness_U != null)
                {
                    darkness_U.Darkness_Unfolds = true;
                }
                break;
            case 1:
                if (spTornado != null)
                {
                    spTornado.spawnT = true;
                }
                break;
            case 2:
                if (randScratch != null)
                {
                    randScratch.RandScratch = true;
                }
                break;
            case 3:
                if (BlankandAttack != null)
                {
                    BlankandAttack.isActive = true;
                }
                break;
            case 4:
                if (energe_charge != null)
                {
                    energe_charge.SRR = true;
                }
                break;
            case 5:
                if (fAttack != null)
                {
                    fAttack.feather_attack = true;
                }
                break;
            default:
                Debug.LogWarning("Invalid skillIndex: " + skillIndex);
                break;
        }
    }
}