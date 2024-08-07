using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnT : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 프리팹
    public Vector2[] spawnPosition;
    public bool spawnT = false;

    void Update()
    {
        if (objectPrefab != null && spawnT)
        {
            for (int i = 0; i < spawnPosition.Length; i++)
            {
                Instantiate(objectPrefab, spawnPosition[i], Quaternion.identity);
            }
            spawnT = false;
        }
        else if (objectPrefab == null)
        {
            Debug.LogError("Object Prefab is not assigned.");
        }
    }
}
