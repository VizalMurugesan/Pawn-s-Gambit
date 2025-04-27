using System.Collections;
using UnityEngine;

public class Map3 : MonoBehaviour
{
    public GameObject player;
    public GameObject UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Game.Instance.UnFreeze();
        Game.Instance.map = Game.FloorMap.floor3;
        Game.Instance.playerHealth.ResetHealth();
        Game.Instance.player = player.GetComponent<Player>();
        yield return new WaitUntil(() => !Game.Instance.screenLoader.isLoadingScreenActive());

        UI.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
