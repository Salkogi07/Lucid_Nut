using UnityEngine;

public class enemyMove : MonoBehaviour
{
    public float speed = 2f; // 적의 이동 속도
    public dash dashScript; // 대쉬 스크립트

    private Transform player; // 플레이어의 트랜스폼

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

    void Update()
    {
        // 플레이어가 존재하고 대쉬 중이 아니며 대쉬 준비 중이 아닐 때만 적이 움직이도록 합니다.
        if (player != null && !dashScript.isDashing && !dashScript.isPreparingToDash)
        {
            // 플레이어와 적 사이의 방향을 계산합니다.
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            // 적을 플레이어 방향으로 이동시킵니다.
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 콜라이더 안에 태그가 "Player"인 객체가 들어왔는지 확인합니다.
        if (other.CompareTag("Player"))
        {
            // Dash 스크립트를 통해 대쉬를 시작합니다.
            dashScript.TryStartDash(other.transform.position);
        }
    }
}
