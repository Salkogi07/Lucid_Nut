using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    public PlayerHp playerhp;
    public GameObject menu;
    // Update is called once per frame
    void Update()
    {
        if (playerhp.player_HP <= 0)
        {
            Debug.Log("Á×¾úÀ½");
            menu.SetActive(true);
        }
    }
}
