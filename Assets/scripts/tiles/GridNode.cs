using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Vector3 WorldPos;
    public bool Walkable;
    public int GridX, GridY;
    public GridNode Parent;

    public float StartToTileCost;
    public float gCost; // Start to this tile
    public float hCost; // Heuristic (this to goal)
    public float fCost => gCost + hCost;


    public GridNode(bool Walkable, Vector3 WorldPos, int GridX, int GridY)
    {
        this.WorldPos = WorldPos;
        this.Walkable = Walkable;
        this.GridX = GridX;
        this.GridY = GridY;
    }
}
