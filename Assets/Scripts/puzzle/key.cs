using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

    public string keycolor;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("Player"); //����
    }
    // Start is called before the first frame update


   
    private void OnTriggerEnter(Collider other)
        {
            // �÷��̾�� �浹�ߴ��� Ȯ��
            if (other.CompareTag("Player"))
            {
                Debug.Log("�÷��̾�� �浹�߽��ϴ�!");
            // �� ������Ʈ�� ����
            Destroy(gameObject);
            }
        }
    // Update is called once per frame
    void Update()
    {
    }   
}
