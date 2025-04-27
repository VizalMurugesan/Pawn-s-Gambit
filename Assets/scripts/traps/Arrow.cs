using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Vector2 spawnPosition;
    public float damage;
    public float spawnInterval = 0f;
    public float speed;


    bool isSpawned = true;
    Rigidbody2D arrow;
    

    [Header("knockBack")]
    public float KnockbackForce = 0f;
    public float knockbackDuration = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPosition = transform.position;
        arrow = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isSpawned)
        {
            arrow.linearVelocity = new Vector2(speed, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSpawned && (collision.tag == "Player" || collision.tag == "Border")) 
        {
            if (isSpawned && collision.tag == "Player")
            {
                GameObject player = collision.gameObject;
                Debug.Log("knockingbackCalled");
                StartCoroutine(player.GetComponent<Player>().Knockback(arrow.linearVelocity, KnockbackForce,knockbackDuration ));
                Game.Instance.playerHealth.takeDamage(30);
            }
            
            StartCoroutine(spawnArrow());
        }
        Debug.Log("collided with"+ collision.gameObject.name);

        Debug.Log("collided with tag" + collision.tag);
        


    }
    private IEnumerator spawnArrow()
    {
        isSpawned = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false; 
        yield return new WaitForSeconds(spawnInterval);
        transform.position = spawnPosition;
        isSpawned = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

}
