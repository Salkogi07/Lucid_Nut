using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public string doorcolor;
    public Itmepuzzle Itmepuzzle;
    public GameObject doorObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            Itmepuzzle.doorcord(doorcolor);
            
            /* Debug.Log("�÷��̾�� �浹�߽��ϴ�!");*/
            // �� ������Ʈ�� ����
            
            
        }
    }
    
            public void dooropen()
            {
               Destroy(gameObject);
            }














        // Update is called once per frame
        void Update()
    {
        
    }
}
