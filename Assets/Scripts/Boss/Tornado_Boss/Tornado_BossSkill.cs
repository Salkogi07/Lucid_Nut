using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado_BossSkill : MonoBehaviour
{
    [Header("Componet")]
    Tornado_BossController controller;

    [Header("Tornado_Fire info")]
    public GameObject tornadoSmall_Prefab;
    public Transform[] tornadoSmall_Point;

    [Header("Rain_Fire info")]
    public GameObject rainProjectilePrefab;
    public GameObject dropletPrefab;
    public GameObject explosionPrefab;
    public int numberOfDroplets = 8;

    private void Awake()
    {
        controller = GetComponent<Tornado_BossController>();
    }

    public void Tornado_Fire_Start()
    {
        StartCoroutine(Tornado_Fire());
    }

    public void Rain_Fire_Start()
    {
        StartCoroutine(Rain_Fire());
    }

    public void Rock_Fire_Start()
    {
        StartCoroutine(Rock_Fire());
    }

    IEnumerator Tornado_Fire()
    {
        controller.isBossSkill = true;

        GameObject tornado1 = Instantiate(tornadoSmall_Prefab, tornadoSmall_Point[0].transform.position, Quaternion.identity);
        GameObject tornado2 = Instantiate(tornadoSmall_Prefab, tornadoSmall_Point[1].transform.position, Quaternion.Euler(0, 180, 0));

        Destroy(tornado1, 7);
        Destroy(tornado2, 7);

        yield return new WaitForSeconds(7);

        controller.isBossSkill = false;
    }

    IEnumerator Rain_Fire()
    {
        controller.isBossSkill = true;

        GameObject rainProjectile = Instantiate(rainProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rainRb = rainProjectile.GetComponent<Rigidbody2D>();

        float elapsedTime = 0f;
        float duration = 1f; // 더 빠르게 올라가는 시간 (1초로 설정)
        float maxDistance = 8f; // 발사체가 도달할 최대 높이

        // 초기 속도는 0으로 설정
        rainRb.velocity = Vector3.up * 0f;

        // 천천히 올라가면서 점점 가속하는 루프
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float speed = Mathf.Lerp(0f, 30f, elapsedTime / duration); // 점점 빠르게 증가 (속도 최대 30)
            rainRb.velocity = Vector3.up * speed;

            // 보스와 발사체 간의 거리를 계산
            float distanceToBoss = Vector3.Distance(rainProjectile.transform.position, transform.position);

            // 보스와의 거리가 maxDistance 이상이면 멈추기
            if (distanceToBoss >= maxDistance)
            {
                rainRb.velocity = Vector3.zero; // 속도를 0으로 설정하여 멈춤
                break;
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 발사체가 최대 높이에 도달하면 크기 증가 및 터지는 효과
        float explosionTime = 0f;
        float maxExplosionTime = 3f; // 1초 동안 크기 증가

        Vector3 originalScale = rainProjectile.transform.localScale;
        Vector3 targetScale = originalScale * 5f; // 최대 3배까지 커지게 설정

        while (explosionTime < maxExplosionTime)
        {
            explosionTime += Time.deltaTime;
            rainProjectile.transform.localScale = Vector3.Lerp(originalScale, targetScale, explosionTime / maxExplosionTime);
            yield return null; // 다음 프레임까지 대기
        }

        // 터지는 효과 구현
        GameObject explosion = Instantiate(explosionPrefab, rainProjectile.transform.position, Quaternion.identity); // 터짐 이펙트 생성
        Destroy(explosion, 0.5f);
        Destroy(rainProjectile); // 발사체 제거

        // 이제 물방울을 생성 (폭발 후)
        for (int i = 0; i < numberOfDroplets; i++)
        {
            float angle = i * (360f / numberOfDroplets);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            GameObject droplet = Instantiate(dropletPrefab, rainProjectile.transform.position + randomOffset, Quaternion.identity);
            Rigidbody2D dropletRb = droplet.GetComponent<Rigidbody2D>();
            dropletRb.velocity = direction * 5f;

            //droplet이새끼 지우는거 만들어야됨
        }

        controller.isBossSkill = false;
    }




    IEnumerator Rock_Fire()
    {
        controller.isBossSkill = true;
        yield return new WaitForSeconds(1);
        controller.isBossSkill = false;
    }
}
