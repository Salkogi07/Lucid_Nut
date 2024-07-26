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
    public Skill[] skills = new Skill[5];

    public dash dashScript;
    public RainEnemy rainAttack;
    public suck_ob suckScript;
    public suck_rock suckRock;

    private bool suckScriptActive;
    private bool dashScriptActive;
    private bool rainAttackActive;
    private bool suckRockActive;

    private void Start()
    {
        // Initialize the boolean states based on the scripts
        suckScriptActive = suckScript.SS;
        dashScriptActive = dashScript.DS;
        rainAttackActive = rainAttack.RS;
        suckRockActive = suckRock.SR;

        // Output random integer between 0 and 10
        int randomInt = UnityEngine.Random.Range(0, 3);
        Debug.Log($"Random Integer: {randomInt}");

        // Output information about each skill
        foreach (Skill skill in skills)
        {
            if (skill != null) // Ensure skill is not null
            {
                Debug.Log($"Skill Name: {skill.skillName}, Cooldown: {skill.cooldown}");
            }
        }
    }
}
