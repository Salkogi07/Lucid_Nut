using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

    public string keycolor;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("Player"); //접촉
    }
    // Start is called before the first frame update


   
    private void OnTriggerEnter(Collider other)
        {
            // 플레이어와 충돌했는지 확인
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어와 충돌했습니다!");
            // 이 오브젝트를 제거
            Destroy(gameObject);
            }
        }
    // Update is called once per frame
    void Update()
    {
    }   
}
