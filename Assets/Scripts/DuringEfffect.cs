using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuringEfffect : MonoBehaviour
{
    public float duringEffectTime;
    void Start()
    {
        Destroy(gameObject, duringEffectTime);
    }
}
