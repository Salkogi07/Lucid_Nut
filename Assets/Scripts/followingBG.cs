using UnityEngine;

public class followingBG : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // 배경이 따라오는 속도

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPosition = player.transform.position;
        Vector3 backgroundPosition = this.transform.position;

        backgroundPosition.x = Mathf.Lerp(backgroundPosition.x, playerPosition.x, smoothSpeed * Time.deltaTime);

        backgroundPosition.z = 2f;
        this.transform.position = backgroundPosition;
    }
}
