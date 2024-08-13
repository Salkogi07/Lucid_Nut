using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossmove : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 설정합니다.
    public float speed = 5f; // 적이 이동하는 속도입니다.
    public float stoppingDistance = 1f; // 플레이어와의 멈추는 거리입니다.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    void Update()
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
    }
}
