using UnityEngine;

public class moveT : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private bool movingLeft = true; // 초기 방향은 왼쪽으로 설정

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트의 태그가 "wall"인지 확인
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("방향전환"); // 충돌 감지 확인용 로그
            // 방향 전환
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
