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

    public bool stoneumbrella = false;


    public door door;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void key(string keycolor)
    {



        if (keycolor == "red")
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
    public void doorcord(string doorcolor)
    {
        if (doorcolor == "red" && redkey > 0)
        {
            redkey = redkey - 1;
            door.dooropen();
        }
        if (doorcolor == "orange" && orangekey > 0)
        {
            orangekey = orangekey - 1;
            door.dooropen();
        }
        else
        {
            Debug.Log("no");
        }
    }



    /*private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {

            
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

