using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    private Vector3 lastDirection;
    private bool isFollowing = true;
    private float followDuration = 3f; // 플레이어를 따라가는 시간
    private float destroyTime = 7f; // 오브젝트가 파괴되기까지의 시간
    public float speed = 15f; // 오브젝트의 이동 속도

    private void Start()
    {
        // Player 태그를 가진 오브젝트를 찾아서 할당합니다.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // 3초 후에 플레이어 추적을 멈추고 마지막으로 바라본 방향으로 이동하도록 합니다.
        Invoke("StopFollowing", followDuration);
        // 7초 후에 오브젝트를 파괴합니다.
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            // 플레이어의 현재 위치를 계속 받아옵니다.
            Vector3 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // 마지막으로 바라본 방향을 업데이트합니다.
            lastDirection = direction;
        }
        else
        {
            // 마지막으로 바라본 방향으로 계속 이동합니다.
            transform.position += lastDirection * speed * Time.deltaTime;
        }
    }

    private void StopFollowing()
    {
        isFollowing = false;
    }
}