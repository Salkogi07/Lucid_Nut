using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallcheck_CloudDark : MonoBehaviour
{
    public CloudDark DC_S;

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
