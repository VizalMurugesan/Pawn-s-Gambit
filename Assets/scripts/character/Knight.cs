using System.Collections;
using System.Threading;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public float invteleportSpeed = 1f;
    //knight variablesS
    public AnimationCurve curve;
    bool isTeleporting;
    bool CanTeleport = true;
    Player player;
    float h_input, v_input;
   
    public float rayDistance;
  
    public float TeleportCooldown = 0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var myValue = Mathf.Lerp(0, 10, 0.5f);
        player =GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        h_input = Input.GetAxisRaw("Horizontal");
        v_input = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(1) && !isTeleporting && CanTeleport)
        {
            if (Game.Instance.pieceType.Equals(Game.PieceType.Knight))
            {
                isTeleporting = true;
                StartCoroutine(Teleport());
            }
        }
        
    }


    private IEnumerator Teleport()
    {
        Vector2 goalPosition = Vector3.zero;
        
        
        
        
        Vector2 start = transform.position;
        Vector2 rayDirection = new Vector2(h_input, v_input);

        if(h_input>0 && v_input>0)
        {
            goalPosition = Game.Instance.player.transform.position+ new Vector3(2f,4f,0f);
        }
        else if (h_input > 0 && v_input < 0)
        {
            goalPosition = Game.Instance.player.transform.position + new Vector3(2f, -4f, 0f);
        }
        else if (h_input < 0 && v_input < 0)
        {
            goalPosition = Game.Instance.player.transform.position + new Vector3(-2f, -4f, 0f);
        }
        else if (h_input < 0 && v_input > 0)
        {
            goalPosition = Game.Instance.player.transform.position + new Vector3(-2f, 4f, 0f);
        }
        else
        {
            isTeleporting = false;
            player.movable = true;
            yield break;
        }


        float elapsedTime = 0f;

        if (IsObstacleInMiddle(rayDirection) && !IsObstacleInEnd(goalPosition))
        {
            player.GetComponent<BoxCollider2D>().enabled = false;
            player.sword.GetComponent<BoxCollider2D>().enabled = false;
            player.movable = false;
            while (elapsedTime < invteleportSpeed)
            {
                float t = elapsedTime / invteleportSpeed;
                float curvedT = curve.Evaluate(t);
                transform.position = Vector2.Lerp(start, goalPosition, curvedT);
                float pingPong = Mathf.PingPong(elapsedTime * 2f, 1f);
                transform.localScale = Vector2.Lerp(new Vector2(0.2f, 0.2f), new Vector2(0.4f, 0.4f), pingPong);

                elapsedTime += Time.deltaTime;
                yield return null; 
            }


            transform.position = goalPosition;
            StartCoroutine(EnterTeleportCooldown());
        }
        isTeleporting = false;
        player.movable = true;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.sword.GetComponent<BoxCollider2D>().enabled = true;
        
        

    }

    bool IsObstacleInMiddle(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance);
        if (hit.collider != null)
        {
            Debug.Log("Move blocked by: " + hit.collider.gameObject.name);
            return true;
        }
        else
        {
            return false;
        }

    }

    bool IsObstacleInEnd(Vector2 Position)
    {
        Collider2D overlap = Physics2D.OverlapBox(Position, Vector2.one, 0f);
        if (overlap != null )
        {
            if(overlap.gameObject.CompareTag("Wood") || overlap.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("object at goalPosition" + overlap.gameObject.name);
                return true;
            }
            
        }
        return false;
    }

    IEnumerator EnterTeleportCooldown()
    {
        CanTeleport = false;
        float Timer = 0f;
        while (Timer< TeleportCooldown)
        {
            Game.Instance.AbilityIcon.fillAmount = Timer;
            yield return null;
            Timer += Time.deltaTime;
        }
        CanTeleport = true;
    }
}
