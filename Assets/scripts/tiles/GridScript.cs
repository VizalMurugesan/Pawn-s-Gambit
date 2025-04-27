using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScript : MonoBehaviour
{
    public Tilemap ObstacletileMap;
    public Vector2Int GridSize;
    public int NodeSize = 1;
    public LayerMask unwalkableLayer;
    public GridNode[,] grid;

    private void Awake()
    {
        CreateGrid();
        Debug.Log("Grid initialized in Awake()");

    }

    void CreateGrid()
    {
        Debug.Log("Creating grid with size: " + GridSize.x + "x" + GridSize.y);
        grid = new GridNode[GridSize.x, GridSize.y];
        Vector3 BottomLeftCell = ObstacletileMap.CellToWorld(new Vector3Int(0, 0, 0));
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                Vector3 WorldPoint = BottomLeftCell + new Vector3(i, j, 0f) * NodeSize;
                bool walkable = !Physics2D.OverlapCircle(WorldPoint, NodeSize * 0.3f, unwalkableLayer);
                Debug.Log(walkable);
                grid[i, j] = new GridNode(walkable, WorldPoint, i, j);
            }
        }
    }

    public GridNode GetNodeFromWorld(Vector3 Worldpos)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt(Worldpos.x) / NodeSize, 0, GridSize.x - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(Worldpos.y) / NodeSize, 0, GridSize.y - 1);
        Debug.Log("X:" + x+ "Y: " + y);
        return grid[x, y];
    }
    public List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip the node itself

                int checkX = node.GridX + dx;
                int checkY = node.GridY + dy;

                // Check if neighbor is within bounds
                if (checkX >= 0 && checkX < GridSize.x && checkY >= 0 && checkY < GridSize.y)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
                Debug.DrawLine(node.WorldPos, grid[checkX, checkY].WorldPos, Color.yellow, 1f);
            }
        }


        return neighbors;
    }
    public void ResetNodes()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                GridNode node = grid[x, y];
                node.gCost = 0;
                node.hCost = 0;
                node.Parent = null;
            }
        }
    }

}
