using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall3 : MonoBehaviour
{
    public SCM SC;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            SC.nextmove *= -1;
            CancelInvoke();
        }
    }
}
