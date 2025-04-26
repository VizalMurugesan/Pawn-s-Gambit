using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public GridScript grid;
    public GameObject player;

    void Start()
    {
        if (grid == null)
            grid = FindObjectOfType<GridScript>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

    }
    
    public List<GridNode> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Debug.Log($"Finding path from: {startWorldPos} to {targetWorldPos}");

        if (grid == null || grid.grid == null)
        {
            Debug.LogError("PathFinder: Grid is not initialized!");
            return null;
        }

        GridNode startNode = grid.GetNodeFromWorld(startWorldPos);
        GridNode targetNode = grid.GetNodeFromWorld(targetWorldPos);
        if (startNode == null || targetNode == null)
        {
            Debug.LogWarning("Start or Target node is null.");
            
            return null;
        }
        Debug.Log($"Start Node: ({startNode.GridX}, {startNode.GridY}) Walkable: {startNode.Walkable}");
        Debug.Log($"Target Node: ({targetNode.GridX}, {targetNode.GridY}) Walkable: {targetNode.Walkable}");
        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost ||
                    openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);
            

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (GridNode neighbor in grid.GetNeighbors(current))
            {
                Debug.DrawLine(current.WorldPos, neighbor.WorldPos, Color.yellow, 0.2f);
            }
            foreach (GridNode neighbor in grid.GetNeighbors(current))
            {
                if (!neighbor.Walkable || closedSet.Contains(neighbor)) continue;

                float newCost = current.gCost + Vector3.Distance(current.WorldPos, neighbor.WorldPos);
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = Vector3.Distance(neighbor.WorldPos, targetNode.WorldPos);
                    neighbor.Parent = current;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode current = endNode;

        while (current != startNode)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    public void ResetNodes()
    {
        grid.ResetNodes();
    }
}
