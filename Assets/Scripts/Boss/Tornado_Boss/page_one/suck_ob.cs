using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suck_ob : MonoBehaviour
{
    public float moveSpeed = 0.5f;  // 플레이어가 이동하는 속도
    public float skillDuration = 4f;  // 스킬의 지속 시간

    private bool isUsingSkill = false;  // 스킬 사용 중인지 여부
    private float skillEndTime = 0f;  // 스킬이 끝나는 시간
    private GameObject player;  // "Player" 태그를 가진 오브젝트

    public bool SS = false;  // 스킬 활성화 상태를 관리하는 변수
    private bool playerInCollider = false;  // 플레이어가 콜라이더 안에 있는지 여부

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // "Player" 태그를 가진 오브젝트를 찾습니다.
    }

    void Update()
    {
        // 플레이어가 콜라이더 안에 있고, SS가 true이고, 스킬이 사용 중이지 않을 때
        if (playerInCollider && SS && !isUsingSkill)
        {
            Debug.Log("빨아 들이기");
            StartSkill();
        }

        // 스킬이 사용 중일 때
        if (isUsingSkill)
        {
            if (Time.time < skillEndTime)
            {
                MovePlayerTowardsObject();
            }
            else
            {
                EndSkill();
            }
        }
    }

    void StartSkill()
    {
        isUsingSkill = true;
        skillEndTime = Time.time + skillDuration;
        SS = false;  // 스킬 시작 후 SS를 false로 설정
    }

    void MovePlayerTowardsObject()
    {
        if (player != null)
        {
            // 현재 오브젝트(이 스크립트가 붙어있는 오브젝트) 방향을 계산합니다.
            Vector3 direction = (transform.position - player.transform.position).normalized;
            // 플레이어를 현재 오브젝트 방향으로 이동시킵니다.
            player.transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void EndSkill()
    {
        isUsingSkill = false;
        // 스킬 종료 후, 플레이어의 이동을 멈춥니다.
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 콜라이더에 들어오면 플레이어가 콜라이더 안에 있음을 표시합니다.
            playerInCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 콜라이더에서 나가면 플레이어가 콜라이더 안에 있지 않음을 표시합니다.
            playerInCollider = false;
            EndSkill();  // 스킬을 비활성화합니다.
        }
    }
}
