using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossai : MonoBehaviour
{
    public bool on = true;
    public float Cooldown = 1f;
    public float Coolup = 1f;
    public float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (on == true) //½Ã°£
        {
            time += Time.deltaTime;
        }
         int i = Random.Range(1, 10);

        if(i==1)
        {
            GameObject director = GameObject.Find("icicle");
            director.GetComponent<icicle>().icicleshot();
        }
    }
}
