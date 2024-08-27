using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class happy : MonoBehaviour
{
    public GameObject happyrock;
    public GameObject Stoneumbrella;
    public stonedoor stonedoor;
    public Itmepuzzle Itmepuzzle;
    public stoneumbrella stoneumbrella;
    float delay;

    public bool on = false;
    public string Priority;
    public string Stoneumbrellastate = "close"; // none close open
    public string rockname = "happy";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            on = true;
            Debug.Log("ccc");
            if (other.CompareTag("Player"))
            {

            }
        }
       /* if (Input.GetKey(KeyCode.U))//공격받으면 반응
        *//*if (Itmepuzzle.stoneumbrella == false )*//*
        {
            Debug.Log("none");
            Itmepuzzle.stoneumbrella = true;
            Stoneumbrellastate = "none";
            stoneumbrella.umbrellanone();


            

        }*/
        /*if (other.CompareTag("Player"))//공격받으면 반응
        *//*if (Itmepuzzle.stoneumbrella == false )*//*
        {

        }*/
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            on = false;
            Debug.Log("eee");
        }
           
    }
        /*if (Itmepuzzle.stoneumbrella == true && Input.GetKeyDown(KeyCode.T))
        {

            Itmepuzzle.stoneumbrella = false;
            Stoneumbrellastate = "close";
            stoneumbrella.umbrellaon();
            Debug.Log("close");


        }
        if (Stoneumbrellastate == "close"  && Input.GetKeyDown(KeyCode.T))
        {


            Stoneumbrellastate = "open";
            Debug.Log("open");


        }*/


        void Update()
    {
        if(delay>0)
        {
            delay -= Time.deltaTime;
        }


        if (delay <=0&&Itmepuzzle.stoneumbrella == false && on && Input.GetKeyDown(KeyCode.T))
        {
            if(Stoneumbrellastate == "open" || Stoneumbrellastate == "close")
            {
            Stoneumbrellastate = "none";
            stoneumbrella.umbrellanone();
            Itmepuzzle.stoneumbrella = true;
                delay = 1f;
                stonedoor.stonedooropen(Priority, rockname, Stoneumbrellastate);
                Debug.Log("none");
            }
            

        }
        if (delay <= 0 && Itmepuzzle.stoneumbrella == true&&Stoneumbrellastate == "none" && on && Input.GetKeyDown(KeyCode.T))
        {
            Stoneumbrellastate = "close";
            Itmepuzzle.stoneumbrella = false;
            stoneumbrella.umbrellaon();
            delay = 1f;
            stonedoor.stonedooropen(Priority, rockname, Stoneumbrellastate);
            Debug.Log("close");

        }
      /*  if (on&&Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("close");
        }*/


        if (delay <= 0 && Stoneumbrellastate == "open" && on &&Input.GetKey(KeyCode.U))
            {
            Stoneumbrellastate = "close";
            delay = 1f;
            stonedoor.stonedooropen(Priority, rockname, Stoneumbrellastate);
            Debug.Log("close");

        }
        if (delay <= 0 && Stoneumbrellastate == "close" && on && Input.GetKey(KeyCode.U))
        {
            Stoneumbrellastate = "open";
            delay = 1f;
            stonedoor.stonedooropen(Priority,rockname ,Stoneumbrellastate);
            Debug.Log("open");

        }
        
        /*attention.transform.position = Stoneumbrella.transform.position;*/
        
            
        



    }
}