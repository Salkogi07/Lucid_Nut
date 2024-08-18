using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charge_E : MonoBehaviour
{
    public GameObject sideObject;    // 양옆에 배치할 오브젝트
    public GameObject topObject;     // 위에 배치할 오브젝트

    public float sideOffset = 10.0f;  // 양옆 오브젝트의 위치 오프셋
    public float topOffset = 5.0f;   // 위 오브젝트의 위치 오프셋

    public bool AE_charge = true;    // AE_charge 변수를 추가하여 오브젝트 배치를 제어합니다.
    public float deleteDelay = 8.0f; // 오브젝트 삭제 지연 시간

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    void Start()
    {
        if (AE_charge)
        {
            PlaceObjects();
            StartCoroutine(DeleteObjectsAfterDelay());
        }
    }

    void PlaceObjects()
    {
        if (sideObject == null || topObject == null)
        {
            Debug.LogError("양옆 또는 위 오브젝트가 할당되지 않았습니다.");
            return;
        }

        // 현재 스크립트가 붙어 있는 게임 오브젝트를 기본 오브젝트로 사용
        Vector3 basePosition = transform.position;

        // 양옆 오브젝트 위치 설정
        GameObject leftSideObject = Instantiate(sideObject, basePosition + new Vector3(-sideOffset, 0, 0), Quaternion.identity);
        GameObject rightSideObject = Instantiate(sideObject, basePosition + new Vector3(sideOffset, 0, 0), Quaternion.identity);

        // 위 오브젝트 위치 설정
        GameObject top = Instantiate(topObject, basePosition + new Vector3(0, topOffset, 0), Quaternion.identity);

        // 소환된 오브젝트 리스트에 추가
        instantiatedObjects.Add(leftSideObject);
        instantiatedObjects.Add(rightSideObject);
        instantiatedObjects.Add(top);
    }

    IEnumerator DeleteObjectsAfterDelay()
    {
        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(deleteDelay);

        // 위에 소환된 오브젝트를 제외한 나머지 오브젝트 삭제
        foreach (GameObject obj in instantiatedObjects)
        {
            // 삭제할 오브젝트가 topObject와 동일한지 비교
            if (obj != null && obj.name != topObject.name)
            {
                Destroy(obj);
            }
        }

        // 리스트 초기화
        instantiatedObjects.Clear();
    }
}
