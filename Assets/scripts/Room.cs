using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] Enemies;

    public void DisableEnemies()
    {
        foreach (var enemy in Enemies)
        {
            enemy.SetActive(false);
        }
    }
}
