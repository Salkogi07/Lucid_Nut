using UnityEngine;

public class rock_spawnMove : MonoBehaviour
{
    // 따라다닐 타겟 오브젝트를 지정합니다.
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            // 현재 오브젝트의 y, z 좌표는 유지하고 x 좌표만 타겟의 x 좌표로 변경합니다.
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
    }
}

