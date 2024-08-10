
using UnityEngine;
using System.Collections;

public class coldwave : MonoBehaviour
{
    public GameObject coldPrefab; 
    public GameObject redPrefab;
    bool on = true; //시간 변수 제어
    float cooldownTime = 5; //한파 소환 시간
    float Duration = 7; //한파 장벽 지속시간
    float time; //시간
    public Transform player;


    // Start is called before the first frame update
    void Start()
    {
         on = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (on == true) //시간
        {
            time += Time.deltaTime;
        }

        if (time >= cooldownTime) //쿨타임이 돌면(시간이 쿨타임보다 높으면)
        {
            on = false; //시간 중지
            time = 0;  //시간 초기화
            StartCoroutine(Objecton()); //한파 장벽 실행
            
        }

        IEnumerator Objecton()
        {
            /*GameObject attention = Instantiate(redPrefab);
            attention.transform.position = player.position;
            yield return new WaitForSeconds(cooltime); // 추후 경고표시후 생성되는 코드 짤 예정
            Destroy(attention);*/
            GameObject go = Instantiate(coldPrefab); //오브젝트 소환
            go.transform.position = player.position; //플레이어 위치로 이동

            yield return new WaitForSeconds(Duration); //지속시간이 끝나면
            Destroy(go); //오브젝트 삭제
                on = true; //시간 재가동
            
        }
    }
}
