using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject DieEffect;

    public Image healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.fillAmount = maxHealth/maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthSlider.fillAmount = currentHealth/maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject DE =  Instantiate(DieEffect, this.transform.position, Quaternion.identity);
        Destroy(DE,0.3f);
        Destroy(gameObject);
    }
}
