using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBomber : Enemy
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject explosion;
    public GameObject ExplosionAnim;
    Animator anim;
    
    void Start()
    {
        base.Start();
        aggro = true;
        enemyType = EnemyType.Bomber;
        StartCoroutine(Attack());
        //agent = GetComponent<NavMeshAgent>();
        anim = ExplosionAnim.GetComponent<Animator>();
        
    }

    

    // Update is called once per frame
    public override IEnumerator Attack()
    {
        
        yield return StartCoroutine(ChasePlayer());
        Debug.Log("chasing ended");
        
        yield return new WaitForSeconds(IntervalBeforeAttacks);
        explosion.transform.position = gameObject.transform.position;
        explosion.SetActive(true);
        anim.SetTrigger("Explode");
        if (InRange(2))
        {
            Game.Instance.playerHealth.takeDamage(50);
        }
        
        gameObject.SetActive(false);
        yield return null;
    }
   

    bool InRange(float distance)
    {
        float Distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if (Distance <= distance)
        {
            return true;
        }
        return false;  
    }
}
