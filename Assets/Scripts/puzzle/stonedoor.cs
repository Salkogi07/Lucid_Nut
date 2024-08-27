using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonedoor : MonoBehaviour
{


    public string doorPriority;
    public int keypoint;
    public int keypointmax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(keypoint >= keypointmax)
        {
            Destroy(gameObject);
            //문이 열림
        }
    }
    public void stonedooropen(string  rockname, string Stoneumbrellastate, string priority)
    {
        if (doorPriority == priority)
        {
            if(Stoneumbrellastate == "open")
            {
                keypoint = keypoint + 1;
            }
            
        }
    }
}
