using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

    public string keycolor;
    public GameObject keyObject;
    public Itmepuzzle Itmepuzzle;
    /*public int keynumber = 0;*/

    private void OnTriggerEnter2D(Collider2D other)
        {
            // �÷��̾�� �浹�ߴ��� Ȯ��
            if (other.CompareTag("Player"))
            {

            /* Debug.Log("�÷��̾�� �浹�߽��ϴ�!");*/
            // �� ������Ʈ�� ����
            Itmepuzzle.key(keycolor);
            Destroy(gameObject);
            }
        }
    // Start is called before the first frame update
    /*void Start()
    {
        *//*this.keyObject = GameObject.Find("Player");*//* //����
    }*/
    // Start is called before the first frame update


   
   
    // Update is called once per frame
      
}
