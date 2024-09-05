using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartFade : MonoBehaviour
{
    public Image image; // 유니티에서 연결할 Image 컴포넌트

    private void Update()
    {
        Color color = image.color; // 현재 이미지의 색상을 가져옴
        color.a -= (Time.deltaTime/2);
        image.color = color; // 이미지의 색상에 변경된 색상을 다시 할당
    }
}
