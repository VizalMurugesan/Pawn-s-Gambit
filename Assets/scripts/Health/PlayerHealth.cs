
using UnityEngine;

public class PlayerHealth : Health
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    public float coolDown;
    float timer;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            Player playerScrpt = player.GetComponent<Player>();
        }
               
        currentHealth = totalHealth;
        healthbarUI = GetComponent<healthbar>();
    }
    // Update is called once per frame
    
    public override void takeDamage(float damage)
    {
        bool hurt = Game.Instance.player.gethurt();
        if (!hurt)
        {
            //Debug.Log("hurt: " + hurt);
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, totalHealth);
            healthbarUI.ChangeHealthUI();
            timer = 0f;

            StartCoroutine(Game.Instance.player.Hurt());
            //StartCoroutine(player.GetComponent<Player>().pushback());
        }

    }
    


    public void addHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, totalHealth);
        healthbarUI.ChangeHealthUI();
    }

    public void ResetHealth()
    {
        currentHealth = totalHealth;
        healthbarUI.ChangeHealthUI();
    }
}
