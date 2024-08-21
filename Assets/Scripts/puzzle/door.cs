using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public string doorcolor;
    public Itmepuzzle Itmepuzzle;
    public GameObject doorObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            /*Itmepuzzle.doorcord(doorcolor);*/
            

            /* Debug.Log("플레이어와 충돌했습니다!");*/
            // 이 오브젝트를 제거
            /*Itmepuzzle Itmepuzzle = other.GetComponent<Itmepuzzle>();*/
            if (doorcolor == "red" && Itmepuzzle.redkey > 0)
            {
                Itmepuzzle.redkey = Itmepuzzle.redkey - 1;
                Itmepuzzle.doorcord();
                Destroy(gameObject);
            }
            if (doorcolor == "orange" && Itmepuzzle.orangekey > 0)
            {
                Itmepuzzle.orangekey = Itmepuzzle.orangekey - 1;
                Itmepuzzle.doorcord();

                Destroy(gameObject);

            }
            else
            {
                Debug.Log("no");
            }
            /* Debug.Log("플레이어와 충돌했습니다!");*/
            // 이 오브젝트를 제거


        }
    }
    
           /* public void dooropen()
            {
               Destroy(gameObject);
            }*/














        // Update is called once per frame
        void Update()
    {
        
    }
}
