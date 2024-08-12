using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossai : MonoBehaviour
{
    public bool on = true;
    public float Cooldown = 7f;
    public float Coolup = 1f;
    public float time = 0f;


    public icicle icicle;
    public coldwave coldwave;
    public Snowcrystal snowcrystal;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (on == true) //Ω√∞£
        {
            time += Time.deltaTime;
        }
         
        if(time>= Cooldown)
        {
            int i = Random.Range(1, 3) ;
            time = 0;

           /* Debug.Log("¿Ã¿◊");*/
           /*GameObject director = GameObject.Find("icicle");
           director.GetComponent<icicle>().icicleshot();*/
        if(i==1)
        {
                /*Debug.Log("¿Ã¿Ã¿◊");*/
                icicle.icicleshot();
                i = 0;
        }
            if (i == 2)
            {
                snowcrystal.Snowcrystalshot();
                i = 0;
            }
            if (i == 3)
            {
                coldwave.coldwaveshot();
                i = 0;
            }
        }
        
    }
}
