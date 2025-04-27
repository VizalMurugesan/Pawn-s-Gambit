
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("components")]
    public NavMeshAgent agent;
    public GameObject player;
    public Animator animator;

    [Header("variables")]
    public float raydistance;
    public bool aggro = false;
    bool chasing = false;
    public Vector2 direction;
    public float movementSpeed;
    public float detectionRange = 1.0f;
    public float IntervalBeforeAttacks = 1f;

    public enum EnemyState { Idle, chasing, Attacking, Dead, Defending, UsingMeteor, Teleporting };
    public EnemyState enemyState;
    public enum EnemyType { PawnMobMelee, PawnMobArcher, Bomber };
    public EnemyType enemyType;
    public EnemyHealth health;
    
    public void Start()
    {

        if(player == null)
        {
            player = Game.Instance.player.gameObject;
        }
        
        direction = (player.transform.position - transform.position).normalized;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.speed = movementSpeed;
        enemyState = EnemyState.Idle;
    }

   
    
    
    public bool IsPlayerNear()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (!agent.pathPending)
        {
            if (distanceToPlayer < agent.stoppingDistance)
            {
                //Debug.Log("Enemy reached player!");

                return true;
            }
            Debug.Log("No Path Pending");
        }
        
        return false;
    }

    public IEnumerator ChasePlayer()
    {
        Debug.Log("chasing player");
        if(enemyType!= EnemyType.Bomber)
        {
            animator.SetBool("chasing", true);
        }
        
        enemyState = EnemyState.chasing;

        while (enemyState == EnemyState.chasing)
        {
            agent.SetDestination(player.transform.position);
            Vector3 velocity = agent.velocity;

            if (velocity.magnitude > 0.1f)
            {
                //Debug.Log("velocity, X: " + velocity.x + ", Y: " + velocity.y);
                Vector3 direction = velocity.normalized;


                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }

            if (IsPlayerNear())
            {
                if (enemyType != EnemyType.Bomber)
                {
                    animator.SetBool("chasing", false);
                }
                    
                enemyState = EnemyState.Idle;
                yield break;
            }
            //Debug.Log("loop");    

            yield return null;
        }
    }

    public bool IsChasing()
    {
        return enemyState == EnemyState.chasing;
    }

    public virtual IEnumerator Attack()
    {
        Debug.Log("bakbakbak");
        yield return null;
    }

    void ResetatkTrigger()
    {
        animator.ResetTrigger("atk");
    }
}
