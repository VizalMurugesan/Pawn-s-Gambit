using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossMage : Enemy
{

    [Header("Components")]
    Animator anim;
    Rigidbody2D enemy;
    
    //NavMeshAgent agent;
    //Animator MeteoriteAnim;
    EnemyBossMageAI AI; 

    [Header("Variables")]
    public float Range = 0f;
    public float DangerRange = 0f;
    public float TeleportCooldown = 0f;
    public float MeteorCooldown = 0f;
    public float delayBeforeBlast = 0f;
    public float MeteorDetectionRnage = 0f;
    public float MeteorDamage = 0f;
    public enum Ability { Idle, BeingUsed, Cooldown};
    Ability teleport = Ability.Idle;
    Ability Meteor = Ability.Idle;
    bool isAttacking = false;
    bool AIisOn = false;


    [Header("GameObjects")]
    //public GameObject player;
    public GameObject Meteorite;
    public GameObject FireBallSpawn;
    public Animator MeteorAnim;
    public GameObject MeteoriteCircle;
    public GameObject HealthPoint;

    public List<GameObject> TeleportPoints = new List<GameObject>();
    public List<GameObject> FireBalls = new List<GameObject>();
    GameObject FireballToSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        //anim = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        
        enemy = GetComponent<Rigidbody2D>();
        AI = GetComponent<EnemyBossMageAI>();
        FireballToSpawn = FireBalls[0];
    }

    private void Update()
    {

        if (!AIisOn)
        {
            if(DistanceCheck2(gameObject, player, 10f) < 8f)
            {
                AIisOn = true;
                StartCoroutine(AI.AIisOn());
            }
        }
        
    }


    public bool IsPlayerNear()
    {
        float distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if(distance<= DangerRange)
        {
            return true;
        }
        return false;
    }

    public bool IsInRange()
    {
        float distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= Range)
        {
            return true;
        }
        return false;
    }

    public bool CanDoNormalAttack()
    {
        Debug.Log("candonormalatk");
        if (!IsInRange())
        {
            Debug.Log("done1");
            return false;
        }
        Vector3 direction = (player.transform.position - gameObject.transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, direction, Range);
        if(ray.collider == null || ray.collider.CompareTag("EnemyBoss"))
        {
            Debug.Log("done2");
            return true;
        }
        Debug.Log("done");
        return false;
        
    }

    public bool IsInNeedOfHealing()
    {
        if(health.currentHealth < health.totalHealth / 2)
        {
            return true;
        }
        return false;
    }

    public bool IsTPAvailable()
    {
        if(teleport == Ability.Idle)
        {
            return true;
        }
        return false;
    }

    public bool IsMeteorAvailable()
    {
        if(Meteor == Ability.Idle)
        {
            return true;
        }
        return false;
    }

    public bool IsHealing()
    {
        float distance = Vector2.Distance(HealthPoint.transform.position, gameObject.transform.position);
        if (distance <= 0.5f)
        {
            return true;
        }
        return false;
    }

    public IEnumerator EnterTeleportCooldown()
    {
        teleport = Ability.Cooldown;
        yield return new WaitForSeconds(TeleportCooldown);
        teleport = Ability.Idle;
    }

    public IEnumerator EnterMeteorCooldown()
    {
        Meteor = Ability.Cooldown;

        yield return new WaitForSeconds(MeteorCooldown);
        Meteor = Ability.Idle;
    }

    public void TeleportToHealth()
    {
        gameObject.transform.position = HealthPoint.transform.position;
        TurnTowardPlayer();
    }

    public void TeleportAwayFromPlayer()
    {
        teleport = Ability.BeingUsed;

        GameObject pointToTeleport = TeleportPoints[0];
        float maxDistance = Vector2.Distance(pointToTeleport.transform.position, player.transform.position);

        foreach (var point in TeleportPoints)
        {
            float distance = Vector2.Distance(point.transform.position, player.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                pointToTeleport = point;
            }
        }

        if (pointToTeleport != null)
        {
            transform.position = pointToTeleport.transform.position;
        }

        TurnTowardPlayer();
        StartCoroutine(EnterTeleportCooldown());
    }


    public IEnumerator UseAbilityMeteor()
    {
        enemyState = EnemyState.UsingMeteor;
        TurnTowardPlayer();
        Meteor = Ability.BeingUsed;
        MeteoriteCircle.SetActive(true);
        MeteoriteCircle.transform.position = player.transform.position;
        yield return new WaitForSeconds(delayBeforeBlast);
        Meteorite.SetActive(true);
        Meteorite.transform.position = MeteoriteCircle.transform.position;
        MeteorAnim.SetTrigger("Explode");
        float DistanceBetweenPlayerandMeteor = DistanceCheck2(Meteorite, player, MeteorDetectionRnage);
        if (DistanceBetweenPlayerandMeteor< MeteorDetectionRnage) 
        {
            float MeteorDamageInstance = ((MeteorDetectionRnage - DistanceBetweenPlayerandMeteor)/ MeteorDetectionRnage)*MeteorDamage;
            Game.Instance.playerHealth.takeDamage(MeteorDamageInstance);
        }
        MeteoriteCircle.SetActive(false);
        yield return new WaitForSeconds(1f);
        Meteorite.SetActive(false);
        StartCoroutine(EnterMeteorCooldown());
        enemyState = EnemyState.Idle;
    }

    public override IEnumerator Attack()
    {
        Debug.Log("doing normalATK");
        enemyState = EnemyState.Attacking;
        TurnTowardPlayer();
        animator.SetTrigger("Attack");
        
        if(FireballToSpawn == null)
        {
            Debug.Log("fireball null");
        }
        foreach(var fireball in FireBalls)
        {
            if (!fireball.activeInHierarchy)
            {
                FireballToSpawn = fireball;
                break;
            }
        }
        
        yield return new WaitForSeconds(2f);
        
        enemyState = EnemyState.Idle;
    }

    public void Move(Transform Target)
    {
        agent.SetDestination(Target.position);
    }

    public void SpawnFireBall()
    {
        FireballToSpawn.transform.position = FireBallSpawn.transform.position;
        float FireballSpeed = FireballToSpawn.GetComponent<FireBall>().Speed;

        Vector2 Direction = (player.transform.position - FireballToSpawn.transform.position).normalized;
        ;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        FireballToSpawn.SetActive(true);
        FireballToSpawn.GetComponent<Rigidbody2D>().SetRotation(angle);
        FireballToSpawn.GetComponent<Rigidbody2D>().linearVelocity = Direction * FireballSpeed;
    }

    bool DistanceCheck(GameObject A, GameObject B, float distance)
    {
        float Distance = Vector2.Distance(A.transform.position, B.transform.position);
        if(Distance < distance)
        {
            return true;
        }
        return false;
    }

    float DistanceCheck2(GameObject A, GameObject B, float distance)
    {
        float Distance = Vector2.Distance(A.transform.position, B.transform.position);
        if (Distance < distance)
        {
            return Distance;
        }
        return 100f;
    }

    void TurnTowardPlayer()
    {
        direction = (player.transform.position - gameObject.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
