using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public AnimationClip explodeAnimation;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Player>()!= null) 
            {
                Game.Instance.playerHealth.takeDamage(Damage);
                StartCoroutine(Explode());
            }
            
        }
        else if (collision.CompareTag("Border") || collision.CompareTag("Wood"))
        {
            StartCoroutine(Explode());
            
        }
        

        
    }

    IEnumerator Explode()
    {
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        anim.SetTrigger("Explode");
        yield return new WaitForSeconds(explodeAnimation.length);
        gameObject.SetActive(false );
    }
    
    
}
