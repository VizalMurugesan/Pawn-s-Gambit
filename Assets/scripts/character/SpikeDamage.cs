using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    BoxCollider2D box;
    public float damage;
    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("damageTaken");
        if (collision.gameObject.CompareTag("Player"))
        {
            Game.Instance.playerHealth.takeDamage(damage);


        }
    }
}
