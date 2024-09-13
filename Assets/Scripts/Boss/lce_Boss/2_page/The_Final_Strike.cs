using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class The_Final_Strike : MonoBehaviour
{
    private Transform player;
    public float speed = 45f;
    float speedtime;
    float speedtimemax = 0.8f;
    float delay = 3f;
    public float Duration = 2f;
    public float Durationtime = 0f;
    public float cooltimetest = 7f;

    float timetest;
    private GameObject lceBoss;
    private Vector2 direction;



    int i;

    // Start is called before the first frame update
    void Start()
    {
        
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // 플레이어 객체가 존재하는지 확인하고 트랜스폼을 저장합니다.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        /*StartCoroutine();*/
        on();
    }



    void Update()
    {
    }

    /*IEnumerator*/
    void on()
    {
        /*timetest += Time.deltaTime;
        if (timetest >= cooltimetest)
        {*/

        /*for (int i = 0; i <= 3 ; i++)
        {*/

        transform.position = player.position;
        int random = Random.Range(1, 3);
        /*float xx = 
        float yy = Random.Range(1, 16);*/
        if (random == 1)
        {
            transform.position = new Vector2(player.position.x - Random.Range(-10, 10), player.position.y - 15);
            Debug.Log("1");
        }
        else if (random == 2)
        {
            transform.position = new Vector2(player.position.x - Random.Range(-10, 10), player.position.y + 15);
            Debug.Log("2");
        }
        else if (random == 3)
        {
            Debug.Log("3");
        }



        // 플레이어를 바라보도록 회전
        LookAtPlayer();

        // 플레이어가 있는 방향으로 이동
        /*StartCoroutine(MoveInDirection());*/

        speedtime = speedtimemax;
        StartCoroutine(move());
                /*}*/


            /*timetest = 0f;*/  //쿨타임 초기화
        }


    IEnumerator move()
    {
            while (speedtime > 0f)
            {
                speedtime -= Time.deltaTime;
                Debug.Log("time");

                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return null;
                }

            
        }
            
                
            
           
        
           
        
            
            

        
    

    void LookAtPlayer()
    {
        // 플레이어와 오브젝트 사이의 방향을 계산
        Vector2 direction = player.position - transform.position;

        // 회전 각도를 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 오브젝트의 Z축 회전을 설정하여 플레이어를 바라보게 만듭니다.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
    
/*
    // Update is called once per frame
    void Update()
    {
        timetest += Time.deltaTime;
        if (timetest >= cooltimetest)
        {
            timetest = 0f;

            *//*for (int a = 0; a <= 4; a++)
            {*//*
                Vector2 direction = player.position - transform.position;

                transform.position = player.position;
                float xx = Random.Range(1, 16);
                float yy = Random.Range(1, 16);
                transform.position = new Vector2(-30, 0);
                direction.Normalize();
                StartCoroutine(dush());
                IEnumerator dush()
                {
                    for (int i = 0; i <= 100; i++)
                    {

                        transform.Translate(direction * speed * Time.deltaTime);
                        yield return new WaitForSeconds(delay);
                    }
                }


                
            }




            // 적을 플레이어 방향으로 이동시킵니다.
           *//* transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            transform.Translate(0, speed * Time.deltaTime, 0); //고드름 이동*//*

            Durationtime += Time.deltaTime;
            if (Durationtime >= Duration) // 지속시간이 끝나면
            {

            }
        }


        // 플레이어와 적 사이의 방향을 계산합니다.




*/
    


/*}*/


/*   public Transform player;  // 플레이어의 Transform을 참조
   public float speed = 5f;  // 이동 속도
   public float chaseSpeed = 8f;  // 돌진 속도
   public float minDistance = 2f;  // 플레이어 근처로 이동할 거리
   public float maxDistance = 5f;  // 플레이어 근처로 이동할 최대 거리

   private Vector2 targetPosition;
   private bool isChasingPlayer = false;

   void Start()
   {
       // 플레이어 근처의 무작위 위치 설정
       SetRandomPositionNearPlayer();
   }

   void Update()
   {
       if (!isChasingPlayer)
       {
           // 무작위 위치로 이동
           MoveToTarget();
       }
       else
       {
           // 플레이어를 향해 돌진
           ChasePlayer();
       }
   }

   void SetRandomPositionNearPlayer()
   {
       // 플레이어 근처의 무작위 위치를 설정
       Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
       targetPosition = (Vector2)player.position + randomOffset;
   }

   void MoveToTarget()
   {
       // 목표 위치로 이동
       transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

       // 목표 위치에 도달하면 플레이어를 쫓기 시작
       if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
       {
           isChasingPlayer = true;
       }
   }

   void ChasePlayer()
   {
       // 플레이어를 향해 돌진
       Vector2 direction = (player.position - transform.position).normalized;
       transform.Translate(direction * chaseSpeed * Time.deltaTime);
   }
}
*/