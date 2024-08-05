using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REdead2 : MonoBehaviour
{
    public float lifetime = 5f; // 오브젝트의 생존 시간

    void Start()
    {
        // 일정 시간이 지난 후 오브젝트를 파괴
        Destroy(gameObject, lifetime);
    }
}
