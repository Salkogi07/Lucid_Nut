using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall2 : MonoBehaviour
{
    public ICM IC;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            IC.nextmove *= -1;
            CancelInvoke();
        }
    }
}
