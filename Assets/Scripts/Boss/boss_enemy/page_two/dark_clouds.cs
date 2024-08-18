using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dark_clouds : MonoBehaviour
{
    public GameObject objectToSpawn; // 소환할 오브젝트를 드래그 앤 드롭으로 지정
    public Transform spawnLocation; // 오브젝트가 소환될 위치를 지정
    public bool DCS = false; // 소환 여부를 결정하는 boolean 변수

    void Update()
    {
        // DCS가 true일 때 소환을 시도
        if (DCS)
        {
            SpawnObject();
            DCS = false; // 소환 후 DCS를 false로 변경
        }
    }

    void SpawnObject()
    {
        if (objectToSpawn != null && spawnLocation != null)
        {
            Instantiate(objectToSpawn, spawnLocation.position, spawnLocation.rotation);
        }
        else
        {
            Debug.LogWarning("Object to spawn or spawn location not assigned.");
        }
    }
}
