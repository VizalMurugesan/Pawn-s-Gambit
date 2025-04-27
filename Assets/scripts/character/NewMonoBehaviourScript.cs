using UnityEngine;


public class tryoutplayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D player;
    public float movementspeed;
    Animator anim;
    bool running = false;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal")*movementspeed;
        float vInput = Input.GetAxis("Vertical")*movementspeed;

        if (hInput > 0 && vInput == 0) // Moving Right
        {
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        else if (hInput < 0 && vInput == 0) // Moving Left
        {
            transform.rotation = Quaternion.Euler(0, 0, 90f);
        }
        else if (hInput < 0 && vInput < 0) // Moving Down-Left
        {
            transform.rotation = Quaternion.Euler(0, 0, 135f);
        }
        else if (hInput > 0 && vInput < 0) // Moving Down-Right
        {
            transform.rotation = Quaternion.Euler(0, 0, -135f);
        }
        else if (hInput == 0 && vInput > 0) // Moving Up
        {
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
        else if (hInput == 0 && vInput < 0) // Moving Down
        {
            transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
        else if (hInput > 0 && vInput > 0) // Moving Up-Right
        {
            transform.rotation = Quaternion.Euler(0, 0, -45f);
        }
        else if (hInput < 0 && vInput > 0) // Moving Up-Left
        {
            transform.rotation = Quaternion.Euler(0, 0, 45f);
        }

        player.linearVelocity = new Vector2 (hInput, vInput);

        /**if(hInput!=0 || vInput!=0 && !running)
        {
            anim.SetTrigger("run");
            running = true;
        }
        if(hInput == 0 && vInput == 0 && running)
        {
            anim.ResetTrigger("run");
            running = false;
        }**/
        
    }
    
}
