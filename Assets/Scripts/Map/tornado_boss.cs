using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornado_boss : MonoBehaviour
{
    public GameObject player_tornado;
    public GameObject tornado_boss_HpSlider;
    public GameObject tornado_boss_Hp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player_tornado.SetActive(true);
            tornado_boss_Hp.SetActive(true);
            tornado_boss_HpSlider.SetActive(true);
        }
    }
}
