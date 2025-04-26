using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float currentHealth;
    public float totalHealth = 100f;
    public healthbar healthbarUI;
    public abstract void takeDamage(float damage);
    

}
