using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    BoxCollider2D box;
    public bool attacking;
    public NavMeshUpdater navMeshUpdater;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject objectHit = collision.gameObject;
        Debug.Log(objectHit.tag);
        if (attacking)
        {
            if(collision.tag == "Wood")
            {
                wood woodcomp = collision.GetComponent<wood>();

                if (woodcomp.isBreakable == true)
                {
                    objectHit.SetActive(false);
                    objectHit.GetComponent<BoxCollider2D>().enabled = false;
                    navMeshUpdater.RebuildNavMesh();
                }
                
            }
            if (collision.tag == "Enemy")
            {
                attacking = true;
                Health enemyhealth = objectHit.transform.GetChild(0).GetComponent<Health>();
                if (enemyhealth != null)
                {
                    enemyhealth.takeDamage(30);
                }
                else
                {
                    Debug.Log("enemyhealth null");
                }
                
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }
    }

    
    
}
