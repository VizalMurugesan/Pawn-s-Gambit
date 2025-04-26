using UnityEngine;
using UnityEngine.UIElements;

public class BowArrowSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject[] Arrows;
    GameObject ArrowToSpawn;
    public float speed;
    Vector3 Direction;

    public void SpawnArrow()
    {
        foreach (var arrow in Arrows)
        {
            if (!arrow.activeInHierarchy)
            {
                ArrowToSpawn = arrow;
                break;
            }
        }
        ArrowToSpawn.transform.position = gameObject.transform.position;
        
        Direction = (player.position - ArrowToSpawn.transform.position).normalized;

        float Angle = Mathf.Atan2(Direction.y, Direction.x)* Mathf.Rad2Deg;

        ArrowToSpawn.transform.rotation = Quaternion.Euler(0,0,Angle);

        ArrowToSpawn.SetActive(true);
        ArrowToSpawn.GetComponent<Rigidbody2D>().linearVelocity = Direction  * speed;

    }
}
