using System.Collections;
using UnityEngine;

public class EnemyPawn : Enemy
{
    EnemyPawnAI AI;
    public float IgnoreAttackWeight = 0.4f;
    bool attacking = false;
    public float IntervalBetweenAttacks = 1f;
    
    public GameObject Sword;
    public GameObject Bow;
    private void Start()
    {
        base.Start();
        AI = GetComponent<EnemyPawnAI>();
    }
    void Update()
    {
        if (!aggro)
        {
            AggroCheck();
        }
        else if (aggro && !AI.AIon)
        {
            //Debug.Log("AI is on");
            AI.AIon = true;
            StartCoroutine(AI.AIisOn());

        }

        
    }
    public bool  AggroCheck()
    {
        direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, raydistance);
        foreach (RaycastHit2D ray in hits)
        {
            if (ray.collider.tag == "Player")
            {
                //Debug.Log("hit player");
                aggro = true;
                break;
            }
            else if (ray.collider.tag == "Wood")
            {
                //Debug.Log("hit wall");
                aggro = false;
                break;
            }
            else
            {
                continue;
            }
        }
        return aggro;
    }
    public override IEnumerator Attack()
    {
        enemyState = EnemyState.Attacking;
        yield return new WaitForSeconds(IntervalBeforeAttacks);
        
        direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        float RandNum = Random.Range(0, 1f);

        if (RandNum< 0.33f || enemyType!= EnemyType.PawnMobMelee)
        {
            animator.SetTrigger("atk");
        }
        else if(RandNum< 0.66f)
        {
            animator.SetTrigger("atk2");
        }
        else
        {
            animator.SetTrigger("atk3");
        }

        yield return new WaitForSeconds(IntervalBetweenAttacks);
        enemyState = EnemyState.Idle;
    }

    public bool IsAttacking()
    {
        return enemyState == EnemyState.Attacking;
    }
    public void DoNothing()
    {
        
    }
    public void EnableSword()
    {
        Sword.GetComponent<Sword>().EnableCollider();
    }
    public void DisableSword()
    {
        Sword.GetComponent<Sword>().DisableCollider();
    }

    private void EnableArrow()
    {
        Bow.GetComponent<BowArrowSpawner>().SpawnArrow();
    }
}



