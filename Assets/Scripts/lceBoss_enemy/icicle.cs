
using UnityEngine;
using System.Collections;

public class icicle : MonoBehaviour
{
    public GameObject iciclePrefab; 
    bool on = true; //시간 변수 제어
    float cooldownTime = 5; //한파 소환 시간
    float delay = 0.38f; //고드름 소환 격차
    public float time; //시간
    float cicileunmber = 15; //고드름 수
    /*public Transform player;*/
    private GameObject icicleObjects;

    // Start is called before the first frame update
    void Start()
    {
         on = true;
    }
    public void icicleshot()
        {
            StartCoroutine(Objecton());  //한파 장벽 실행
        }
    // Update is called once per frame
    void Update()
    {
        /*        if (on == true) //시간
                {
                    time += Time.deltaTime;
                }

                if (time >= cooldownTime) //쿨타임이 돌면(시간이 쿨타임보다 높으면)
                {
                    on = false; //시간 중지
                    time = 0;  //시간 초기화
        }       public void DecreaseHp()
        */
        
        }



        
        
        IEnumerator Objecton()
        {
            for (int i = 0; i <= cicileunmber; i++)
            {
                
                Vector3 shooterPosition = transform.position; 
                Vector3 spawnPosition = shooterPosition + new Vector3(Random.Range(-30.0f, 20.0f), 10, 0); //위치 랜덤지정
                icicleObjects = Instantiate(iciclePrefab, spawnPosition, Quaternion.identity);
                GameObject icicle = Instantiate(icicleObjects); //오브젝트 소환
                yield return new WaitForSeconds(delay);

            }

            /*on = true; //시간 재가동*/


            /*transform.Translate(speed * Time.deltaTime);*/





        
    }
        
}
