using UnityEngine;

public class BowArrow : MonoBehaviour
{
    public float Damage = 0f;
    public float Knockbackspeed=3f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Game.Instance.playerHealth.takeDamage(Damage);
            Vector2 knockBackDir = gameObject.GetComponent<Rigidbody2D>().linearVelocity.normalized;
            
            //Debug.Log("Archer damaged for " + Damage + " health");
            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Border") || collision.CompareTag("Wood"))
        {
            gameObject.SetActive(false);
        }
        
    }
}
