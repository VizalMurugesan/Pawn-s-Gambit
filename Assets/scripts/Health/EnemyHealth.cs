using UnityEngine;

public class EnemyHealth : Health
{
    public Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        healthbarUI = GetComponent<healthbar>();
    }
    public override void takeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, totalHealth);
        healthbarUI.ChangeHealthUI();
        if (currentHealth <= 0f)
        {
            Die();
        }

    }
    void Die()
    {
        enemy.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    public void ResetHealth()
    {
        currentHealth = totalHealth;
        healthbarUI.ChangeHealthUI();
        healthbarUI.gameObject.SetActive(true);
    }
    public void addHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, totalHealth);
        healthbarUI.ChangeHealthUI();
    }
}
