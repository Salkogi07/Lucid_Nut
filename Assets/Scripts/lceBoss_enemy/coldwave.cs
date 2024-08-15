
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class coldwave : MonoBehaviour
{
    public GameObject coldPrefab;
    public GameObject redPrefab;
    bool on = true; //시간 변수 제어
    float cooldownTime = 5; //한파 소환 시간
    float Duration = 7; //한파 장벽 지속시간
    public float Warning_Time = 1f;
    float time; //시간
    public Transform player;

    private Renderer attention;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        on = true;

       
        
    }
    // Update is called once per frame



    public void coldwaveshot()
    {
        StartCoroutine(Objecton()); //한파 장벽 실행
    }




    void Update()
    {
    }
    /* if (on == true) //시간
     {
         time += Time.deltaTime;
     }

     if (time >= cooldownTime) //쿨타임이 돌면(시간이 쿨타임보다 높으면)
     {
         on = false; //시간 중지
         time = 0;  //시간 초기화*/




    /*}*/

    

    IEnumerator Objecton()
        {

        
        GameObject attention = Instantiate(redPrefab);
        attention.transform.position = player.position;
         
      

        

      
        
        /*for(int i=0;i<=10; i++)
        {
            float Visibility =0f;
            yield return new WaitForSeconds(0.1f);
            Visibility = Visibility + 0.1f;
            
        }*/
        yield return new WaitForSeconds(2f);
        GameObject coldwave = Instantiate(coldPrefab); //오브젝트 소환
            coldwave.transform.position = attention.transform.position; //플레이어 위치로 이동
        Destroy(attention);
            yield return new WaitForSeconds(Duration); //지속시간이 끝나면
            Destroy(coldwave); //오브젝트 삭제
                on = true; //시간 재가동
            
        }
    
}
