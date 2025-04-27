using UnityEngine;

public class Map1 : MonoBehaviour
{
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Game.Instance.player = player.GetComponent<Player>();

    }
}
