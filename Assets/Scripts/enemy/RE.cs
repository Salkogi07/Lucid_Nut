using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE : MonoBehaviour
{
    public float lifetime = 5f; // ������Ʈ�� ���� �ð�

    void Start()
    {
        // ���� �ð��� ���� �� ������Ʈ�� �ı�
        Destroy(gameObject, lifetime);
    }
}
