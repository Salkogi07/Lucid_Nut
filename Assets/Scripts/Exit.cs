using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public GameObject menus;
    public Image image;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void menu()
    {
        menus.SetActive(true);
    }

    public void closemenu()
    {
        menus.SetActive(false);
    }

    public void reGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void Update()
    {
        GameObject tornadoB = GameObject.Find("Tornado_boss");
        BossScript boss = tornadoB.GetComponent<BossScript>();
        if (boss.bossdead == true && boss != null)
        {
            Debug.Log("º¸½º");
            StartCoroutine(scen());
        }
    }

    IEnumerator scen()
    {
        Debug.Log("µé¾î");
        Color color = image.color;
        color.a += (Time.deltaTime * 2);
        image.color = color;
        yield return new WaitForSecondsRealtime(8f);
        Debug.Log("¿È");
        SceneManager.LoadScene(2);
    }
}
