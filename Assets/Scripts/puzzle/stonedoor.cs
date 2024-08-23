using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonedoor : MonoBehaviour
{


    public int doorPriority;
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
            //문이 열림
        }
    }
    public void stonedooropen(int priority)
    {
        if (doorPriority == priority)
        {
            
        }
    }
}
