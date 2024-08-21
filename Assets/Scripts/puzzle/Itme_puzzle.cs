using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itmepuzzle : MonoBehaviour
{
    float time;



    public int redkey;
    public int orangekey;
    public int yellowkey;
    public int Lightgreenkey;
    public int bluekey;
    public int purplekey;

    /*public door door;*/
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void key(string keycolor)
    {
        
        
        
        if(keycolor == "red")
        {
            redkey = redkey + 1;
            Debug.Log(redkey);
            
        }
        if (keycolor == "orange")
        {
            orangekey = orangekey + 1;
            Debug.Log(orangekey);

        }
        if (keycolor == "yellow")
        {
            yellowkey = yellowkey + 1;
            Debug.Log(yellowkey);

        }
        if (keycolor == "Lightgreen")
        {
            Lightgreenkey = Lightgreenkey + 1;
            Debug.Log(Lightgreenkey);

        }
        if (keycolor == "blue")
        {
            bluekey = bluekey + 1;
            Debug.Log(bluekey);

        }
        if (keycolor == "purple")
        {
            purplekey = purplekey + 1;
            Debug.Log(purplekey);

        }
    }
    public void doorcord()
    {
     /*    door door = other.GetComponent<door>();
        if (door.doorcolor =="red" && redkey > 0)
        {
            redkey = redkey - 1;
            
        }
        if (door.doorcolor == "orange" && orangekey>0)
        {
            orangekey = orangekey - 1;
            
        }
        else
        {
            Debug.Log("no");
        }*/
    }



            /*private void OnTriggerEnter2D(Collider2D other)
            {
                // 플레이어와 충돌했는지 확인
                if (other.CompareTag("Player"))
                {

                    *//* Debug.Log("플레이어와 충돌했습니다!");*//*
                    // 이 오브젝트를 제거
                    Itmepuzzle.key(keycolor);
                    Destroy(gameObject);
                }
            }*/
        
        // Update is called once per frame
        void Update()
    {
        /*time += Time.deltaTime;
        if(time>=1f)
        {
            Debug.Log(redkey);
        }*/
       
    }
}
