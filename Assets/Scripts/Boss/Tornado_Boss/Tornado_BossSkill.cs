using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado_BossSkill : MonoBehaviour
{
    Tornado_BossController controller;

    public GameObject tornadoSmall_Prefab;

    public Transform[] tornadoSmall_Point;

    private void Awake()
    {
        controller = GetComponent<Tornado_BossController>();
    }

    public void Tornado_Fire_Start()
    {
        StartCoroutine(Tornado_Fire());
    }

    public void Rain_Fire_Start()
    {
        StartCoroutine(Rain_Fire());
    }

    public void Rock_Fire_Start()
    {
        StartCoroutine(Rock_Fire());
    }

    IEnumerator Tornado_Fire()
    {
        controller.isBossSkill = true;

        // 첫 번째 토네이도 생성 (오른쪽을 봄)
        GameObject tornado1 = Instantiate(tornadoSmall_Prefab, tornadoSmall_Point[0].transform.position, Quaternion.identity);

        // 두 번째 토네이도 생성 (왼쪽을 보게 180도 회전)
        GameObject tornado2 = Instantiate(tornadoSmall_Prefab, tornadoSmall_Point[1].transform.position, Quaternion.Euler(0, 180, 0));

        Destroy(tornado1,7);
        Destroy(tornado2,7);

        if(tornado1 && tornado2)
        {
            yield return null;
        }

        controller.isBossSkill = false;
    }

    IEnumerator Rain_Fire()
    {
        controller.isBossSkill = true;
        yield return new WaitForSeconds(1);
        controller.isBossSkill = false;
    }

    IEnumerator Rock_Fire()
    {
        controller.isBossSkill = true;
        yield return new WaitForSeconds(1);
        controller.isBossSkill = false;
    }
}
