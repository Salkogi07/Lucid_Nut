using UnityEngine;

public class moveT : MonoBehaviour
{
    public float speed = 8f; // 이동 속도
    private bool movingLeft = true; // 초기 방향은 왼쪽으로 설정
    public float lifetime = 14f; // 오브젝트의 생명 시간

    void Start()
    {
        // 오브젝트를 12초 후에 파괴
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 현재 방향에 따라 이동
        if (movingLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            Debug.Log("방향전환"); // 충돌 감지 확인용 로그
            movingLeft = !movingLeft;
        }
    }

    // 디버그용: 콜라이더 범위를 시각적으로 표시
    void OnDrawGizmos()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, collider.bounds.size);
        }
    }
}
