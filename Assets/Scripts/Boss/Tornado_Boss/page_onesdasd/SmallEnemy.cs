using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;  // 발사할 오브젝트의 프리팹
    public float launchSpeed = 10f;  // 발사 속도
    public bool ST = false;  // 스킬 활성화 상태를 관리하는 변수

    private Vector3 playerPosition;  // 플레이어의 위치를 저장할 변수
    private bool hasStoredPosition = false;  // 플레이어 위치를 저장했는지 여부
    private GameObject player;  // "Player" 태그를 가진 오브젝트

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // "Player" 태그를 가진 오브젝트를 찾습니다.
    }

    void Update()
    {
        if (ST && !hasStoredPosition)
        {
            // 플레이어의 위치를 저장합니다.
            playerPosition = player.transform.position;
            hasStoredPosition = true;

            // 발사체를 생성하고 발사합니다.
            LaunchProjectile();
        }
        else if (!ST)
        {
            // 스킬이 비활성화되면 상태를 초기화합니다.
            hasStoredPosition = false;
        }
    }

    void LaunchProjectile()
    {
        if (projectilePrefab != null)
        {
            // 발사체를 생성합니다.
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 플레이어 방향을 계산합니다.
            Vector3 direction = (playerPosition - transform.position).normalized;
            // 발사체를 플레이어 방향으로 발사합니다.
            projectile.GetComponent<Rigidbody2D>().velocity = direction * launchSpeed;
        }
    }
}
