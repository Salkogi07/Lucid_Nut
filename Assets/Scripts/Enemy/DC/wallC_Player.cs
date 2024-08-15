using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallC_Player : MonoBehaviour
{
    public DC_Sc DC_S;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            Debug.Log("∫Æ¿”");
            DC_S.nextmove *= -1;
            CancelInvoke();
        }
    }
}
