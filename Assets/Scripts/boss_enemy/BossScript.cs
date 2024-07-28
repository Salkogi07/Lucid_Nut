using System;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public float cooldown;
    // 다른 스킬 관련 속성을 추가할 수 있습니다.
}

public class BossScript : MonoBehaviour
{

    public dash dashScript;
    public RainEnemy rainAttack;
    public suck_ob suckScript;
    public RandomRockSpawner RainSC;
    public SmallEnemy SmallST;

    private int randN;

    private void Start()
    {
        randN = 5;
        // Call the ChooseSkill method every 30 seconds
        InvokeRepeating("ChooseSkill", 0f, 10f);
    }

    private void ChooseSkill()
    {
        // Generate a random integer between 0 and 3
        int randomInt = UnityEngine.Random.Range(0, 4);
        if (randomInt == randN)
        {
            while (randN == randomInt)
            {
                Debug.Log("바뀜" + randN + randomInt);
                randomInt = UnityEngine.Random.Range(0, 4);
            }
        }
        else
        {
            Debug.Log(randomInt);
            randN = randomInt;
        }

        // Use the random integer to decide which skill to use
        UseSkill(randomInt);    
    }

    private void UseSkill(int skillIndex)
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
                else
                {
                    Debug.LogWarning("suckScript is not assigned.");
                }
                break;
            case 1:
                if (dashScript != null)
                {
                    dashScript.DS = true;
                }
                else
                {
                    Debug.LogWarning("dashScript is not assigned.");
                }
                break;
            case 2:
                if (rainAttack != null)
                {
                    rainAttack.RS = true;
                }
                else
                {
                    Debug.LogWarning("rainAttack is not assigned.");
                }
                break;
            case 3:
                if (RainSC != null)
                {
                    suckScript.SS = true;
                    RainSC.SRR = true;
                }
                else if (RainSC == null)
                {
                    Debug.LogWarning("RainSC is not assigned.");
                }
                break;
            default:
                Debug.LogWarning("Invalid skillIndex: " + skillIndex);
                break;
        }
    }
}