using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneumbrella : MonoBehaviour
{
    public GameObject umbrellaObject;
    public Itmepuzzle Itmepuzzle;
    public happy happy;
    /*public Transform player;*/
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("attack") || other.CompareTag("Player"))
        {
            Debug.Log("¸Â¾Ò´ç");
            Itmepuzzle.stoneumbrella = true;
            Destroy(gameObject);


        }

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.T))
        {

            Itmepuzzle.stoneumbrella = false;
            Debug.Log("dkskdskasds");


        }
    }
}



