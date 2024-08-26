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

    public bool on = false;
    public int Priority;
    public string Stoneumbrellastate; // none close open
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

        }
        if (other.CompareTag("Player"))//공격받으면 반응
        /*if (Itmepuzzle.stoneumbrella == false )*/
        {
            Debug.Log("none");
            Itmepuzzle.stoneumbrella = true;
            Stoneumbrellastate = "none";
            stoneumbrella.umbrellanone();


            

        }
        if (other.CompareTag("Player"))//공격받으면 반응
        /*if (Itmepuzzle.stoneumbrella == false )*/
        {

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            on = false;
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
       

        if(/*Itmepuzzle.stoneumbrella == false*/Stoneumbrellastate == "open" && on &&Input.GetKey(KeyCode.U))
            {
            Stoneumbrellastate = "close";
            
        }
        if (/*Itmepuzzle.stoneumbrella == false*/Stoneumbrellastate == "close" && on && Input.GetKey(KeyCode.U))
        {
            Stoneumbrellastate = "open";

        }
        
        /*attention.transform.position = Stoneumbrella.transform.position;*/
        if (Stoneumbrellastate == "open")
        {
            stonedoor.stonedooropen(Priority);
        }



    }
}