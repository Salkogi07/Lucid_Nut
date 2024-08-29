using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossai : MonoBehaviour
{
    public bool on = true;
    public float Cooldown = 10f;
    public float Coolup = 1f;
    public float time = 0f;
    public float delay = 0f;
    public float down = 0f;

    public icicle icicle;
    public coldwave coldwave;
    public Snowcrystal snowcrystal;
    public Bossmove bossmove;


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
            int i = Random.Range(1, 4) ;
            time = 0;

           /* Debug.Log("¿Ã¿◊");*/
           /*GameObject director = GameObject.Find("icicle");
           director.GetComponent<icicle>().icicleshot();*/
        if(i==1)
        {
                /*Debug.Log("¿Ã¿Ã¿◊");*/
                icicle.icicleshot();
                delay = 1.5f;
                bossmove.stopdirector(delay);
                i = 0;
        }
            if (i == 2)
            {
                snowcrystal.Snowcrystalshot();
                /*down = 1.8f;*/
                bossmove.speeddirector();
                i = 0;
            }
            if (i == 3)
            {
                coldwave.coldwaveshot();
                delay = 2f;
                bossmove.stopdirector(delay);
                i = 0;
            }
        }
        
    }
}
