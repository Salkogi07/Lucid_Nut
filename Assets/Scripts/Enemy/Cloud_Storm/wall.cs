using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public CSM CS;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            CS.nextmove *= -1;
            CancelInvoke();
        }
    }
}
