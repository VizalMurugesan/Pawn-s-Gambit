using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshUpdater : MonoBehaviour
{
    NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }

    public void RebuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
