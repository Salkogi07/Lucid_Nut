using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI1 : MonoBehaviour
{
    [SerializeField]
    public Slider BossHP;
    //public BossScript Finalboss;

    private float maxBH;
    private float curBH;
    float imsi;
    // Start is called before the first frame update
    void Start()
    {
       // maxBH = Finalboss.BossHp;
       // curBH = Finalboss.BossHp;
        BossHP.value = (float) curBH / (float) maxBH;
    }

    // Update is called once per frame
    void Update()
    {
        //curBH = Finalboss.BossHp;
        imsi = (float)curBH / (float)maxBH;
        HandleHP();
    }

    private void HandleHP()
    {
        BossHP.value = Mathf.Lerp(BossHP.value, imsi, Time.deltaTime * 10);
    }
}
