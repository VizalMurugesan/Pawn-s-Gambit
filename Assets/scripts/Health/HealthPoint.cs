using System.Collections;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] float healthPoint;
    BoxCollider2D box;
    bool Triggered = false;
    Animator anim;
    

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();    
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Debug.Log("object Triggered"+ collision.gameObject.name);
        StartCoroutine(AddHealth(collision.gameObject));
        
    }

    IEnumerator AddHealth(GameObject Character)
    {
        Triggered = true;
        box.enabled = false;
        anim.SetBool("OnPoint", true);
        while (Triggered)
        {
            if (Character.CompareTag("Player") || Character.CompareTag("sword"))
            {
                if(Game.Instance.playerHealth!= null)
                {
                    Game.Instance.playerHealth.addHealth(healthPoint);
                }
                
                
            }
            else if (Character.CompareTag("EnemyBoss"))
            {
                Character.GetComponentInChildren<EnemyHealth>().addHealth(healthPoint);
                
            }
            if (!IsOnHealingPoint(Character))
            {
                Triggered = false;
                box.enabled = true;
                anim.SetBool("OnPoint",false);
                yield break;

            }
            yield return null;

        }
        
    }

    bool IsOnHealingPoint(GameObject character)
    {
        float Distance = Vector2.Distance(character.transform.position, gameObject.transform.position);
        if(Mathf.Abs(Distance) < 1f)
        {
            return true;
        }
        return false;
    }
}
