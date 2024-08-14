using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossmove : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform을 설정합니다.
    public float speed = 5f; // 적이 이동하는 속도입니다.
    public float stoppingDistance = 1f; // 플레이어와의 멈추는 거리입니다.
    public bool stop = false;
    public float delaymove = 0f;
    // Start is called before the first frame update
    /*private Transform player; // 플레이어의 트랜스폼*/

    void Start()
    {
        // 태그가 "Player"인 객체를 찾습니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // 플레이어 객체가 존재하는지 확인하고 트랜스폼을 저장합니다.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    public void stopdirector(float delay)
    {
        delaymove = delay;
        StartCoroutine(stopon());
    }

    IEnumerator stopon()
    {
        stop = true;
        yield return new WaitForSeconds(delaymove);
        stop = false;
    }


    void Update()
    {
       
       if(stop == false)
        {
            // 플레이어와 적 사이의 방향을 계산합니다.
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            // 적을 플레이어 방향으로 이동시킵니다.
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
            
        
    }




    /*void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 플레이어가 특정 거리보다 멀리 있을 때 추적
        if (distance > stoppingDistance)
        {
            // 플레이어 방향으로 이동
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // 플레이어를 바라보도록 회전
            transform.LookAt(player);
        }
    }*/
}
