using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

[ExecuteInEditMode]
public class TileManager : MonoBehaviour
{
    [SerializeField] private int _height =0, _width = 0;

    [Header("tiles")]
    [SerializeField] private Tile blackSquare;
    [SerializeField] private Tile whiteSquare;
    [SerializeField] private Tile wood;
    [SerializeField] private Tile tableTop;

    [Header("prefabs")]
    [SerializeField] private GameObject spikeTrapPrefab;
    [SerializeField] private GameObject characterPrefab;

    [Header("tileMaps")]
    [SerializeField] private Tilemap baseMap;
    [SerializeField] private Tilemap ChessBoard;
    [SerializeField] private Tilemap ObstacleMap;
    [SerializeField] private Tilemap trapMap;
    [SerializeField] private Tilemap UnitMap;

    [Header("occurenceVar")]
    [SerializeField] private float woodOc;
    [SerializeField] private float trapOc;
    
    [SerializeField]public Grid grid;

    private List<GameObject> instantiatedTraps = new List<GameObject>();
    private List<GameObject> instantiatedUnits = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //GenerateBase(_height,_width);
        //int BoardStartX = 40;
        //int BoardStartY = 40;
        //GenerateChessBoard(BoardStartX,BoardStartY);
        //GenerateObstacles(BoardStartX, BoardStartY);

    }
    void GenerateBase( int mapHeight, int mapWidth)
    {
        baseMap.ClearAllTiles();
       
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                
                baseMap.SetTile(new Vector3Int(x, y, 0), tableTop);
                
                
            }
        }
    }
    void GenerateChessBoard(int startX, int startY)
    {

        ChessBoard.ClearAllTiles();
        bool isBlack = true;
        for (int y = startY; y < 8+startY; y++)
        {
            for (int x = startX; x < 8+startX; x++)
            {

                isBlack = (x + y) % 2 == 0;
                Tile tileToPlace = isBlack ? blackSquare : whiteSquare;
                ChessBoard.SetTile(new Vector3Int(x, y, 0), tileToPlace);
                


            }
        }
    }
    
    
    
    void DestroyUnits()
    {
        foreach (GameObject unit in instantiatedUnits)
        {
            DestroyImmediate(unit);  // Destroy each trap
        }
        instantiatedUnits.Clear();
    }
    
    bool isAnythingOnTheTile(Vector3Int pos)
    {
        if(isObstacleOnTile(pos) || isTrapOnTile(pos) || isUnitsOnTile(pos))
        {
            return true;
        }
        return false;
    }
    bool isObstacleOnTile(Vector3Int pos)
    {
        return ObstacleMap.HasTile(pos);
    }
    bool isUnitsOnTile(Vector3Int pos)
    {
        Vector3 worldPos = ChessBoard.CellToWorld(pos) + new Vector3(0.5f, 0.2f, 0);
        foreach (GameObject unit in instantiatedUnits)
        {
            if (unit.transform.position == worldPos)
            {
                return true;
            }
        }
        return false;
    }
    bool isTrapOnTile(Vector3Int pos)
    {
        Vector3 worldPos = ChessBoard.CellToWorld(pos) + new Vector3(0.5f, 0.2f, 0);  
        foreach (GameObject trap in instantiatedTraps)
        {
            if (trap.transform.position == worldPos)
            {
                return true; 
            }
        }
        return false;
    }

    // editor
    public void GenerateChessBoardEditor()
    {
        int boardStartX = 66;
        int boardStartY = 50;
        GenerateChessBoard(boardStartX, boardStartY);
    }

   

    public void GenerateBaseEditor()
    {
        GenerateBase(_height, _width);
    }
    
    
    void ClearAllTiles()
    {
        baseMap.ClearAllTiles();
        ChessBoard.ClearAllTiles();
    }

    //path
    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        frontier.Enqueue(start);
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            if (current == goal)
            {
                break; // Found the path to the goal
            }
            Vector3Int[] directions = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0), // Right
                new Vector3Int(-1, 0, 0), // Left
                new Vector3Int(0, 1, 0), // Up
                new Vector3Int(0, -1, 0) // Down
            };

            foreach (var direction in directions)
            {
                Vector3Int next = current + direction;

                // Use the existing isAnythingOnTheTile function to check if the tile is walkable
                if (isAnythingOnTheTile(next)) continue; // Skip if the tile is not walkable

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        // Reconstruct the path
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentTile = goal;
        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = cameFrom[currentTile];
        }

        path.Reverse();
        return path;
    }

}
