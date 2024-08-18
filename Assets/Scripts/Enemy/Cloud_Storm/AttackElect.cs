using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackElect : MonoBehaviour
{
    public GameObject explosionEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Æã");

            if (explosionEffect != null)
            {
                GameObject explosion = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            Destroy(gameObject);
        }
    }
}
