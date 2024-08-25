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

    public int Priority;
    public string Stoneumbrellastate; // none close open
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*GameObject attention = Instantiate(Stoneumbrella);
        attention.transform.position = Stoneumbrella.transform.position;*/
        if (Stoneumbrellastate == "open")
        {
            stonedoor.stonedooropen(Priority);
        }



    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
            if (Itmepuzzle.stoneumbrella == false /*&& *//*Input.GetKey(KeyCode.U)*/) //공격받으면 반응
        {
            Debug.Log("none");
            Itmepuzzle.stoneumbrella = true;
            Stoneumbrellastate = "none";
            stoneumbrella.umbrellanone();
            /*gameObject.SetActive(false);*/
            /*Destroy(gameObject);*/


        }
        {
            if (Itmepuzzle.stoneumbrella == true && Input.GetKeyDown(KeyCode.T))
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


            }
        }
    }
}