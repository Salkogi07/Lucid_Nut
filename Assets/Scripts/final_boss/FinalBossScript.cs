using System;
using UnityEngine;

public class FinalBossScript : MonoBehaviour
{
    //Page_Two
    public darkness_unfolds darkness_U;

    //Page_Two


    public int BossHp = 10000;

    private int randN = -1;
    private int randomInt = -1;

    private void Start()
    {
        // Call the ChooseSkill method every 30 seconds
        InvokeRepeating("ChooseSkill", 0f, 15f);
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
        if (darkness_U != null) darkness_U.Darkness_Unfolds = false;

        // 선택된 스킬을 활성화합니다.
        switch (skillIndex)
        {
            case 0:
                if (darkness_U != null)
                {
                    darkness_U.Darkness_Unfolds = true;
                }
                break;
            default:
                Debug.LogWarning("Invalid skillIndex: " + skillIndex);
                break;
        }
    }
}