using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

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
        Debug.Log(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
