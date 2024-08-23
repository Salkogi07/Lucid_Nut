using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class happy : MonoBehaviour
{
    public GameObject happyrock;
    public GameObject Stoneumbrella;
    public stonedoor stonedoor;

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

}
