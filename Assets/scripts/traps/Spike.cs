using UnityEngine;
using System.Collections;
using System.Threading;

public class Spike : MonoBehaviour
{
    BoxCollider2D box;
    
    public float spawnInterval;
    public float spawnlength;
    public float spawntimer = 0f;
    bool spawned = false;
    public Sprite spawn;
    public Sprite notspawn;
    SpriteRenderer rend;


 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponentInParent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (spawned)
        {
            if (spawntimer >= spawnlength)
            {
                rend.sprite = notspawn;
                box.enabled = false;
                spawned = false;
                spawntimer = 0f;
            }
        }
        else if (!spawned)
        {
            if (spawntimer >= spawnInterval)
            {
                spawned = true;
                rend.sprite = spawn;
                box.enabled = true;
                spawntimer = 0f;
            }
        }
        spawntimer += Time.deltaTime;
        //Debug.Log(spawned);
    }

    
}
