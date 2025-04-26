using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    BoxCollider2D box;
    public NavMeshUpdater navMeshUpdater;
    public float AttackPower = 40f;

    // Tracks which enemies have been hit during this swing
    private HashSet<GameObject> enemiesHitThisSwing = new HashSet<GameObject>();

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        box.enabled = false;
    }

    public void EnableCollider()
    {
        box.enabled = true;
        
    }

    public void DisableCollider()
    {
        box.enabled = false;
        enemiesHitThisSwing.Clear(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject objectHit = other.gameObject;
        //Debug.Log("Hit: " + objectHit.tag);

        if (other.CompareTag("Wood"))
        {
            wood woodcomp = other.GetComponent<wood>();
            if (woodcomp != null && woodcomp.isBreakable)
            {
                DestroyWood(objectHit);
            }
        }

        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            Enemy enemyComp = other.GetComponent<Enemy>();
            if (enemyComp != null)
            {
                if (enemiesHitThisSwing.Contains(objectHit))
                {
                    //Debug.Log("Enemy already hit during this swing, ignoring.");
                    return;
                }

                enemiesHitThisSwing.Add(objectHit);

                Health enemyHealth = objectHit.GetComponent<Enemy>().health;
                if (enemyHealth != null)
                {
                    enemyHealth.takeDamage(AttackPower);
                    //Debug.Log("enemy damaged for 30 health");
                }
            }
        }
        if (objectHit.CompareTag("Player") && (gameObject.CompareTag("Enemy")))
        {
            //Debug.Log("Enemy sword hit the player!");
            Game.Instance.playerHealth.takeDamage(AttackPower);
            Debug.Log("player damaged for 30 health");
        }

        
    }
    void DestroyWood(GameObject wood)
    {
        wood.SetActive(false);
        wood.GetComponent<BoxCollider2D>().enabled = false;
        navMeshUpdater.RebuildNavMesh();
    }
}
