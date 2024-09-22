using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMove playerMove;  // PlayerMove 스크립트를 참조

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;  // 이펙트가 생성될 고정 위치
    public Vector2 boxSize;
    public GameObject SlashEffect;    // 휘두르는 이펙트
    public GameObject StingEffect;    // 찌르기 이펙트

    public AudioClip attackSound;     // 공격 사운드 클립 추가
    private AudioSource audioSource;  // 오디오 소스 추가

    private bool showGizmos = false;  // 기즈모 표시 여부
    private Vector2 gizmoCenter;      // 기즈모 중앙 위치
    private Vector2 gizmoSize;        // 기즈모 크기
    private float gizmoDisplayTime = 0.5f; // 기즈모가 표시될 시간
    private float gizmoTimer = 0f;    // 기즈모 타이머

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();  // PlayerMove 스크립트 가져오기
        audioSource = GetComponent<AudioSource>();  // 오디오 소스 컴포넌트 가져오기

        if (audioSource == null)
        {
            // 만약 오디오 소스가 없으면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        Vector2 len = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos.transform.position;
        float z = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;

        pos.transform.rotation = Quaternion.Euler(0, 0, z);

        if (curTime <= 0)
        {
            // 마우스 좌클릭 (휘두르기 공격)
            if (Input.GetMouseButtonDown(0) && !playerMove.isAttack)
            {
                int direction = len.x > 0 ? 1 : -1;  // 캐릭터를 기준으로 오른쪽이면 1, 왼쪽이면 -1

                PerformSlashAttack(direction);  // 휘두르기 공격 수행
                curTime = coolTime;  // 쿨타임 적용
            }

            // 마우스 우클릭 (찌르기 공격)
            if (Input.GetMouseButtonDown(1) && !playerMove.isAttack)
            {
                int direction = len.x > 0 ? 1 : -1;  // 캐릭터를 기준으로 오른쪽이면 1, 왼쪽이면 -1

                PerformStingAttack(direction);  // 찌르기 공격 수행
                curTime = coolTime;  // 쿨타임 적용
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        // 기즈모 타이머 업데이트
        if (gizmoTimer > 0)
        {
            gizmoTimer -= Time.deltaTime;
        }
        else
        {
            showGizmos = false;  // 타이머가 0 이하가 되면 기즈모 표시를 비활성화
        }
    }


    private void PerformSlashAttack(int direction)
    {
        playerMove.isAttack = true;

        playerMove.FlipAttack(direction);  // 플레이어의 방향을 공격 방향으로 설정

        GameObject SE = Instantiate(SlashEffect, transform.position, pos.transform.rotation,transform);  // 휘두르는 이펙트 생성
        SE.gameObject.GetComponent<SpriteRenderer>().flipY = direction < 0 ? true : false;
        Destroy(SE, 0.3f);  // 짧은 시간 후 이펙트 삭제

        audioSource.PlayOneShot(attackSound);  // 공격할 때 사운드 재생
    }

    private void PerformStingAttack(int direction)
    {
        playerMove.isAttack = true;

        playerMove.FlipAttack(direction);  // 플레이어의 방향을 공격 방향으로 설정

        GameObject SE = Instantiate(StingEffect, transform.position, pos.transform.rotation, transform);  // 휘두르는 이펙트 생성
        SE.gameObject.GetComponent<SpriteRenderer>().flipY = direction < 0 ? true : false;
        Destroy(SE, 0.3f);  // 짧은 시간 후 이펙트 삭제

        audioSource.PlayOneShot(attackSound);  // 공격할 때 사운드 재생
    }
}
