using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileManager))]
public class TileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TileManager manager = (TileManager)target;

        if (GUILayout.Button("Generate Base Tiles"))
        {
            manager.GenerateBaseEditor(); // WeÅfll expose this method shortly
        }

        if (GUILayout.Button("Generate Chessboard"))
        {
            manager.GenerateChessBoardEditor();
        }

        
        
        


    }
}
