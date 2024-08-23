using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

    public string keycolor;
    public GameObject keyObject;
    public Itmepuzzle Itmepuzzle;
    /*public int keynumber = 0;*/

    private void OnTriggerEnter2D(Collider2D other)
        {
            // 플레이어와 충돌했는지 확인
            if (other.CompareTag("Player"))
            {

            /* Debug.Log("플레이어와 충돌했습니다!");*/
            // 이 오브젝트를 제거
            Itmepuzzle.key(keycolor);
            Destroy(gameObject);
            }
        }
    // Start is called before the first frame update
    /*void Start()
    {
        *//*this.keyObject = GameObject.Find("Player");*//* //접촉
    }*/
    // Start is called before the first frame update


   
   
    // Update is called once per frame
      
}
