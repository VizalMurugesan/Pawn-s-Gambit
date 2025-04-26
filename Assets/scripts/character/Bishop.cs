using System.Collections;
using UnityEngine;

public class Bishop : MonoBehaviour
{
    bool isTeleporting;
    Player player;
    float h_input, v_input;
    public float rayDistance;
    public float invteleportSpeed = 1f;
    Vector2 goalPosition, playerPos;
    float elapsedTime = 0.1f;
    public  AnimationCurve curve1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        h_input = Input.GetAxisRaw("Horizontal");
        v_input = Input.GetAxisRaw("Vertical");
        if (h_input !=0 && v_input!= 0 && Input.GetMouseButtonDown(1) && !isTeleporting)
        {
            if (player.GetPiece() == "bishop")
            {
                isTeleporting = true;
                StartCoroutine(Teleport());
            }
        }
    }
    private IEnumerator Teleport()
    {
        player.movable = false;
        elapsedTime = 0f;
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.sword.GetComponent<BoxCollider2D>().enabled = false;
        Vector2 rayDirection = new Vector2(h_input, v_input);
        playerPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(playerPos, rayDirection, rayDistance);
        
        if (hit.collider != null)
        {
            goalPosition = hit.collider.transform.position;
            Debug.Log("Hit object's position: " + goalPosition);
        }
        else
        {
            
            goalPosition = new Vector2 (playerPos.x+(h_input*rayDistance), playerPos.y+(v_input*rayDistance));
        }
        while (elapsedTime < invteleportSpeed)
        {

            float parameter = curve1.Evaluate(elapsedTime / invteleportSpeed);
            transform.position = Vector2.Lerp(playerPos, goalPosition,parameter );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isTeleporting = false;
        player.movable = true;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.sword.GetComponent<BoxCollider2D>().enabled = false;



    }
}
