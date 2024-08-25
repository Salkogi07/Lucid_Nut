using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutotial_player_M : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    private bool toggle = false;
    public float interval = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(flipP());
    }

    void think()
    {
        float nextTimeThink = Random.Range(2f, 4f);
        flipP();
        Invoke("think", nextTimeThink);
    }

    IEnumerator flipP()
    {
        while (true)
        {
            toggle = !toggle;
            spriteRenderer.flipX = toggle;
            yield return new WaitForSeconds(interval);
        }
    }
}
