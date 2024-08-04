using UnityEngine;

public class BlankBandAttack : MonoBehaviour
{
    public float chaseDuration = 6f;
    public float ascendDuration = 3f;
    public float returnSpeed = 4f;
    public float moveSpeed = 10f; // 이동 속도를 조절하는 변수
    public bool isActive = false; // 동작 여부를 제어하는 변수

    private Vector3 startPosition;
    private GameObject player;
    private float chaseStartTime;
    private float ascendStartTime;
    private bool hasMovedToInitial = false; // 초기 위치로 이동했는지 여부를 추적

    void Start()
    {
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");

        // 초기 위치 설정 (활성화되지 않은 경우 이동하지 않음)
        transform.position = startPosition;
    }

    void Update()
    {
        if (isActive)
        {
            if (!hasMovedToInitial)
            {
                // 공격 시작 시 특정 위치로 이동 (속도 증가)
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, -15, 10), (moveSpeed*2) * Time.deltaTime);

                if (Vector3.Distance(transform.position, new Vector3(0, -15, 10)) < 0.1f)
                {
                    transform.position = new Vector3(0, -15, 10); // 정확한 위치로 이동
                    hasMovedToInitial = true; // 이동 완료로 표시
                    chaseStartTime = Time.time; // 추적 시작 시간 기록
                }
            }
            else if (Time.time - chaseStartTime < chaseDuration)
            {
                // 플레이어 추적 (X축만)
                Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
            }
            else if (Time.time - chaseStartTime < chaseDuration + ascendDuration)
            {
                // 위로 이동 (X축 속도 0으로 설정)
                if (ascendStartTime == 0)
                {
                    ascendStartTime = Time.time;
                }
                float t = (Time.time - ascendStartTime) / ascendDuration;
                // 보간값 t를 그대로 사용하여 상승
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, startPosition.y, transform.position.z), t);
                // X축 속도 0으로 설정
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                // 초기 위치로 복귀 (속도 증가)
                transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);

                // 복귀가 완료되면 상태를 초기화
                if (Vector3.Distance(transform.position, startPosition) < 0.1f)
                {
                    ResetAttack();
                }
            }
        }
    }

    // Optional: Reset method to set `isActive` to false
    public void ResetAttack()
    {
        isActive = false;
        hasMovedToInitial = false;
        chaseStartTime = 0f; // 초기화
        ascendStartTime = 0f; // 초기화
        transform.position = startPosition;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void StartAttack()
    {
        isActive = true;
    }
}
