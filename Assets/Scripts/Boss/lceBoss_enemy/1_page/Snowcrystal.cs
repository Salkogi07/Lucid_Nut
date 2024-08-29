using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Snowcrystal : MonoBehaviour
{
    public GameObject projectilePrefab;  // 발사할 투사체 프리팹
    public float projectileSpeed = 1f;  // 투사체 속도
    /*public float fireRate = 1f;*/     // 발사 간격
    public float time;
    public Transform firePoint;          // 투사체 발사 위치
    public float projectileRange = 35f; // 투사체 사거리
    public int snowcrystalnmber = 45; //투사체 수량
    public float delay = 0.15f; //소환 간격
    // 30 0.225
    public bool on = true;

    private float cooldownTime = 10f;     // 다음 발사 시간
       public void Snowcrystalshot()
        {
            StartCoroutine(re());
        }
    void Update()
    {
    }
        /*if (on == true)
        {
            time += Time.deltaTime;
        }

        if (time >= cooldownTime)
        {
            on = false;
            time = 0;
            

        }*/
        
            
        IEnumerator re()
        {
            
            for (int i = 0; i <= snowcrystalnmber; i++)
            {

                FireProjectile();
                yield return new WaitForSeconds(delay);

            }
            on = true;

        }

        void FireProjectile()
        {
            // 투사체 생성
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // 무작위 방향 설정
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // 투사체 이동 스크립트 부착
            ProjectileMovement projectileMovement = projectile.AddComponent<ProjectileMovement>();
            projectileMovement.Initialize(randomDirection, projectileSpeed, projectileRange);
        }
    

    public class ProjectileMovement : MonoBehaviour
    {
        private Vector2 direction;   // 이동 방향
        private float speed;         // 이동 속도
        private float range;         // 사거리
        private Vector2 startPoint;  // 시작 위치

        public void Initialize(Vector2 dir, float spd, float rng)
        {
            direction = dir;
            speed = spd;
            range = rng;
            startPoint = transform.position;
        }

        void Update()
        {
            float rangeup = 1f;
            // 투사체를 방향으로 이동
            transform.Translate(direction * speed * Time.deltaTime);

            // 사거리 체크
            if (Vector2.Distance(startPoint, transform.position) >= rangeup) //일정거리의 도달하면
            {
                speed = speed + 0.11f; //투사체 속도 증가
                rangeup = rangeup + 0.5f; // 일정거리 증가
            }
                if (Vector2.Distance(startPoint, transform.position) >= range)
            {
                Destroy(gameObject);
            }
        }
        
    }
}