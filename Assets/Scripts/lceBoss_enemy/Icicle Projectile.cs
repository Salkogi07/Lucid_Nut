using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleProjectile : MonoBehaviour
{
    public float speed = -30f; //고드름 속도
    public float Duration = 4f; //고드름 지속시간
    public float Durationtime = 0f; //고드름 사라지는 시간
    // Start is called before the first frame update
    void Start()
    {
       Durationtime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(0,speed * Time.deltaTime,0); //고드름 이동

        Durationtime += Time.deltaTime;
        if (Durationtime >= Duration) // 지속시간이 끝나면
        {
            Destroy(gameObject); //오브젝트 삭제
        }
        
    }
   /* IEnumerator start()
    {
        

    }*/
}

