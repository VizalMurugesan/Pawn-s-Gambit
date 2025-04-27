using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField]PathFinder pathFinder;
    public float movementSpeed = 3f;
    public GameObject player;
    public GridScript grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // Let GridScript finish Awake()

        if (pathFinder != null)
        {
            Debug.Log("PathFinder found!");
            StartCoroutine(GoToPlayer());
        }
    }

    private void Update()
    {
        

       
    }
    public IEnumerator GoToPlayer()
    {
        //pathFinder.ResetNodes();  // Clear gCost, hCost, parent

        List<GridNode> path = pathFinder.FindPath(gameObject.transform.position, player.transform.position);

        if (path == null || path.Count == 0)
        {
            Debug.Log("No path to player found.");
            yield break;
        }
        else
        {
            foreach (var step in path)
            {
                Debug.DrawLine(step.WorldPos, step.WorldPos + Vector3.up * 0.25f, Color.green, 2f);
            }
        }

        foreach (GridNode node in path)
        {
            // Move toward node.WorldPos until close enough
            while (Vector3.Distance(transform.position, node.WorldPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, node.WorldPos, movementSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.01f); // Small pause between steps (optional)
        }
    }
}
    
