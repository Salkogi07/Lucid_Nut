using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suck_rock : MonoBehaviour
{
    public float moveSpeed = 20f;      // 돌 이동 속도
    public float skillDuration = 5f;   // 스킬의 지속 시간
    public float spreadDelay = 5f;     // 돌을 랜덤하게 뿌리기 전 대기 시간
    public float spreadForce = 10f;    // 돌을 뿌릴 때 힘의 강도
    public float rbActivationDelay = 0.2f; // Rigidbody2D를 활성화할 지연 시간
    public float extraForce = 1f;      // 발사 후 추가적으로 돌에 가해질 힘
    public float minRotationSpeed = 50f; // 돌의 최소 회전 속도
    public float maxRotationSpeed = 200f; // 돌의 최대 회전 속도

    public bool SR = false;            // 스킬 실행을 위한 불 값
    private bool isUsingSkill = false;  // 스킬 사용 중인지 여부
    private float skillEndTime = 0f;    // 스킬이 끝나는 시간
    private List<GameObject> attractedRocks = new List<GameObject>(); // 끌어당긴 돌들을 저장할 리스트

    void Start()
    {
        // 필요시, 이 스크립트가 붙어 있는 오브젝트의 Rigidbody2D를 확인하고 추가합니다.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // SR이 true이고 스킬이 사용 중이지 않을 때
        if (SR && !isUsingSkill)
        {
            Debug.Log("돌던지기");
            StartSkill();
            SR = false; // 스킬이 시작되면 SR 값을 false로 변경
        }

        // 스킬이 사용 중일 때
        if (isUsingSkill)
        {
            if (Time.time < skillEndTime)
            {
                MoveRocksTowardsObject();
            }
            else
            {
                // 스킬 종료 0.2초 전, Rigidbody2D를 활성화합니다.
                StartCoroutine(ActivateRigidbodyAndSpreadRocks());
                EndSkill();
            }
        }
    }

    void StartSkill()
    {
        isUsingSkill = true;
        skillEndTime = Time.time + skillDuration;

        attractedRocks.Clear(); // 리스트를 초기화합니다.
        // "rock" 태그를 가진 모든 오브젝트를 찾습니다.
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("rock");

        foreach (GameObject rock in rocks)
        {
            // 중복 추가를 방지
            if (!attractedRocks.Contains(rock))
            {
                attractedRocks.Add(rock);
                // 돌의 Rigidbody2D를 비활성화합니다.
                Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = true;  // 물리적 상호작용을 비활성화합니다.
                    rb.angularVelocity = 0f; // 회전을 멈춥니다.
                }

                // 돌마다 독립적인 회전 속도를 설정합니다.
                rock.AddComponent<RotateRandomly>();
            }
        }
    }

    void MoveRocksTowardsObject()
    {
        foreach (GameObject rock in attractedRocks)
        {
            if (rock != null)
            {
                // 현재 오브젝트(이 스크립트가 붙어있는 오브젝트) 방향을 계산합니다.
                Vector3 direction = (transform.position - rock.transform.position).normalized;
                // 돌을 이 스크립트가 붙어 있는 오브젝트 방향으로 이동시킵니다.
                rock.transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    IEnumerator ActivateRigidbodyAndSpreadRocks()
    {
        yield return new WaitForSeconds(rbActivationDelay);

        foreach (GameObject rock in attractedRocks)
        {
            if (rock != null)
            {
                // Rigidbody2D를 활성화합니다.
                Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = false;  // 물리적 상호작용을 다시 활성화합니다.
                    // 랜덤한 방향으로 힘을 가합니다.
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    rb.AddForce(randomDirection * spreadForce, ForceMode2D.Impulse);

                    // 추가적인 지속적인 힘을 가합니다.
                    rb.velocity += randomDirection * extraForce;
                }
                else
                {
                    // Rigidbody2D가 없는 경우, 위치를 강제로 이동시키는 방식
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    rock.transform.position += (Vector3)randomDirection * spreadForce;
                }
            }
        }
    }

    void EndSkill()
    {
        isUsingSkill = false;

        // 스크립트가 붙어 있는 오브젝트의 Rigidbody2D를 가져옵니다.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Rigidbody2D가 있으면 이동을 멈춥니다.
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Rigidbody2D가 없는 경우, 다른 방식으로 처리할 수 있습니다.
            Debug.LogWarning("Rigidbody2D component is missing on the object.");
        }
    }
}

public class RotateRandomly : MonoBehaviour
{
    public float minRotationSpeed = 50f; // 최소 회전 속도
    public float maxRotationSpeed = 200f; // 최대 회전 속도

    private float rotationSpeed;

    void Start()
    {
        // 랜덤한 회전 속도를 설정합니다.
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    void Update()
    {
        // 매 프레임마다 회전 속도에 따라 돌을 회전시킵니다.
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
