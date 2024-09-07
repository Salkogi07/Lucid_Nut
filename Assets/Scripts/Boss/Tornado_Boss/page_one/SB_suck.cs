using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_suck : MonoBehaviour
{
    public float attractionSpeed = 5f; // 빨아들이는 속도

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Wall 태그를 가진 오브젝트가 콜라이더에 들어왔을 때
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject); // 현재 오브젝트 파괴
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Player 태그를 가진 오브젝트가 콜라이더 안에 있을 때
        if (other.CompareTag("Player"))
        {
            // Player 오브젝트를 현재 오브젝트로 빨아들임
            Vector3 direction = transform.position - other.transform.position;
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, attractionSpeed * Time.deltaTime);
        }
    }
}