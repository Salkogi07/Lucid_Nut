using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class The_Final_Strike : MonoBehaviour
{
    private Transform player;
    public float speed = 7.5f;
    float delay = 0.001f;
    public float Duration = 2f;
    public float Durationtime = 0f;
    public float cooltimetest = 7f;
    float timetest;
    private GameObject lceBoss;
   /* private Vector2 direction;*/

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // �÷��̾� ��ü�� �����ϴ��� Ȯ���ϰ� Ʈ�������� �����մϴ�.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timetest += Time.deltaTime;
        if (timetest >= cooltimetest)
        {


            for (int a = 0; a <= 4; a++)
            {
                Vector2 direction = player.position - transform.position;
                
                transform.position = player.transform.position;
                float xx = Random.Range(1, 16);
                float yy = Random.Range(1, 16);
                transform.position = new Vector2(-30 , 0);
                direction.Normalize();
                StartCoroutine(dush());
                IEnumerator dush()
                 {
                for (int i = 0; i <= 100; i++)
                {

                    transform.Translate(direction * speed * Time.deltaTime);
                    yield return new WaitForSeconds(delay);
                }
               }


                timetest = 0f;
            }




            // ���� �÷��̾� �������� �̵���ŵ�ϴ�.
            /*transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);*/
            /*transform.Translate(Vector2.down * speed * Time.deltaTime);*/
            /* transform.Translate(0, speed * Time.deltaTime, 0); //��帧 �̵�

             Durationtime += Time.deltaTime;
             if (Durationtime >= Duration) // ���ӽð��� ������
             {

             }*/
        }


        // �÷��̾�� �� ������ ������ ����մϴ�.


        /**/


    }
    

}


/*public Transform player;  // �÷��̾��� Transform�� ����
public float speed = 5f;  // �̵� �ӵ�
public float chaseSpeed = 8f;  // ���� �ӵ�
public float minDistance = 2f;  // �÷��̾� ��ó�� �̵��� �Ÿ�
public float maxDistance = 5f;  // �÷��̾� ��ó�� �̵��� �ִ� �Ÿ�

private Vector2 targetPosition;
private bool isChasingPlayer = false;

void Start()
{
    // �÷��̾� ��ó�� ������ ��ġ ����
    SetRandomPositionNearPlayer();
}

void Update()
{
    if (!isChasingPlayer)
    {
        // ������ ��ġ�� �̵�
        MoveToTarget();
    }
    else
    {
        // �÷��̾ ���� ����
        ChasePlayer();
    }
}

void SetRandomPositionNearPlayer()
{
    // �÷��̾� ��ó�� ������ ��ġ�� ����
    Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
    targetPosition = (Vector2)player.position + randomOffset;
}

void MoveToTarget()
{
    // ��ǥ ��ġ�� �̵�
    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

    // ��ǥ ��ġ�� �����ϸ� �÷��̾ �ѱ� ����
    if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
    {
        isChasingPlayer = true;
    }
}

void ChasePlayer()
{
    // �÷��̾ ���� ����
    Vector2 direction = (player.position - transform.position).normalized;
    transform.Translate(direction * chaseSpeed * Time.deltaTime);
}
*/