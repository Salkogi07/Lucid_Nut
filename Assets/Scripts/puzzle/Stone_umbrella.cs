using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneumbrella : MonoBehaviour
{
    public GameObject umbrellaObject;
    public Itmepuzzle Itmepuzzle;
    public happy happy;
    /*public Transform player;*/

    public bool d;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnTriggerEnter2D(Collider2D other)
    {*/

        /*if (other.CompareTag("attack") *//*|| other.CompareTag("Player")*//*) //공격받으면 반응
        {
            Debug.Log("맞았당");
            Itmepuzzle.stoneumbrella = true;
            gameObject.SetActive(false);
            *//*Destroy(gameObject);*//*


        }*/
        

            /*    if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.T))
                {

                    Itmepuzzle.stoneumbrella = false;
                    Debug.Log("dkskdskasds");


                }*/
        /*}*/
    public void umbrellaon()
        {
            gameObject.SetActive(true);
        }
    public void umbrellanone()
    {
        gameObject.SetActive(false);
    }
}



