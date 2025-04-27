using UnityEngine;
using System;
using System.Collections;

using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;

public class Player : MonoBehaviour
{
    // Component references
    BoxCollider2D box;
    public Rigidbody2D player;
    public Animator anim;
    List<SpriteRenderer> rendr;
    public  GameObject [] sprites;
    public PlayerHealth playerHealth;
    




    // Variables
    [Header("movement Variables")]
    public float movementSpeed;
    //public float teleportSpeed = 0.5f;
    public string piece = "pawn";
    public float diagonalLimiter = 0.7f;
    public float pieceLimiter = 0.6f;
    public float pieceMovementMultiplier = 1.3f;
    public bool movable;
    public bool running = false;
    
    public float speedX;
    public float speedY;

    [Header("hurt")]
    public float flashCount = 0f;
    public float lowAlpha = 0.3f;
    public float highAlpha = 1f;
    public float waitTime = 0.3f;
    bool ishurt = false;
    public float flashnum;
    public AnimationCurve knockbackCurve;
    public Cameracontroller CameraScript;

    [Header("attacks")]
    public GameObject sword;
    public AnimationClip attackAnimation;
    public AnimationClip ComboAnimation;
    public AnimationClip Combo2Animation;
    public float ComboWindow = 0.5f;
    public float AtkCooldown = 1f;
    bool combo1Triggered = false;
    bool combo2Triggered = false;
    private enum AttackState { Idle, Attack1, Attack2, Attack3, Cooldown }
    private AttackState attackState = AttackState.Idle;





    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rendr = new List<SpriteRenderer>();

        foreach (var sprite in sprites)
        {
            rendr.Add(sprite.GetComponent<SpriteRenderer>());
        }
        anim.applyRootMotion = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (speedX == 0 && speedY == 0) && attackState == AttackState.Idle)
        {
            StartCoroutine(Attack());

        }

        //Debug.Log("AttackState: "+attackState);

        
    }

    








    private void FixedUpdate()
    {
       
        speedX = Input.GetAxis("Horizontal") * movementSpeed;
        speedY = Input.GetAxis("Vertical") * movementSpeed;

        
        

        if (movable)
        {
            turnplayer();
            if (speedX != 0 && speedY != 0)
            {
            speedY *= diagonalLimiter;
            speedX *= diagonalLimiter;
            }
            if (Game.Instance.pieceType.Equals(Game.PieceType.Pawn))
            {
                player.linearVelocity = new Vector2(speedX, speedY);
            }
            if (Game.Instance.pieceType.Equals(Game.PieceType.Knight))
            {
                player.linearVelocity = new Vector2(speedX, speedY);
            }
            if(piece.Equals("rook", StringComparison.OrdinalIgnoreCase))
            {
                if (speedX != 0 ^ speedY != 0)
                {
                    speedX *= pieceMovementMultiplier ;
                    speedY *= pieceMovementMultiplier ;
                }
                else
                {
                    speedX *= pieceLimiter;
                    speedY *= pieceLimiter;
                }
                player.linearVelocity = new Vector2(speedX, speedY);
            }
            if (piece.Equals("bishop", StringComparison.OrdinalIgnoreCase))
            {
                if (speedX != 0 && speedY != 0)
                {
                    speedX *= pieceMovementMultiplier;
                    speedY *= pieceMovementMultiplier;
                }
                else
                {
                    speedX *= pieceLimiter;
                    speedY *= pieceLimiter;
                }
                player.linearVelocity = new Vector2(speedX, speedY);
            }
            if (piece.Equals("queen", StringComparison.OrdinalIgnoreCase))
            {
                if (speedX != 0 || speedY != 0)
                {
                    speedX *= pieceMovementMultiplier;
                    speedY *= pieceMovementMultiplier;
                }
            
                player.linearVelocity = new Vector2(speedX, speedY);
            }
            runningAnim();



        }
        else if (!ishurt)
        {
            Debug.Log("not movable");
            player.linearVelocity = Vector2.zero;
            anim.SetBool("running", false);
            running = false;
        }
        





    }


    public void SetPiece(string name)
    {
        piece = name;
    }
    public string GetPiece()
    {
        return piece;
    }
   
    IEnumerator EnterAttackCooldown()
    {
        movable = true;
        attackState = AttackState.Cooldown;
        float Timer = 0;
        while(Timer< AtkCooldown)
        {
            Game.Instance.AttackIcon.fillAmount = Timer/AtkCooldown;
            Timer += Time.deltaTime;
            yield return null;
        }
        
        attackState = AttackState.Idle;
        
    }

    void EnterCoolDownIfNotComboTriggered()
    {
        if (!combo1Triggered)
        {
            StartCoroutine(EnterAttackCooldown());
        }
    }
    void EnterCoolDownIfNotCombo2Triggered()
    {
        if (!combo2Triggered)
        {
            StartCoroutine(EnterAttackCooldown());
        }
    }


    IEnumerator Attack()
    {
        movable = false;
        attackState = AttackState.Attack1;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(ComboLoop1());
        if (combo1Triggered)
        {
            //Debug.Log("Combo 1 triggered!");
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(ComboLoop2());
        }
        //Debug.Log("combo2: "+combo2Triggered);
        
        yield return null;
    }
    IEnumerator ComboLoop1()
    {
        float timer = 0f;
        combo1Triggered = false;  // Reset before checking

        while (timer < ComboWindow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                combo1Triggered = true;
                attackState = AttackState.Attack2;
                anim.SetTrigger("combo1");
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator ComboLoop2()
    {
        float timer = 0f;
        combo2Triggered = false;
        while (timer < ComboWindow)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                combo2Triggered = true;
                attackState = AttackState.Attack3;
                ComboWindow = Combo2Animation.length;
                anim.SetTrigger("combo2");
                yield break;
                
            }
            timer += Time.deltaTime;
            yield return null;
        }
        
    }

    public IEnumerator Hurt()
    {
        ishurt = true;
        

        //anim.SetTrigger("hurt");
        flashCount = 0;

        while (flashCount < flashnum)
        {
            // Set low alpha
            if(rendr!= null)
            {
                SetPlayerAlpha(lowAlpha);
            }
            
            yield return new WaitForSeconds(waitTime);

            // Set high alpha
            if (rendr != null)
            {
                SetPlayerAlpha(highAlpha);
            }
                
            
            yield return new WaitForSeconds(waitTime);

            flashCount++;
        }

        SetPlayerAlpha(1f);
        
        ishurt = false;
    }
    public bool gethurt()
    {
        return ishurt;
    }

    void EnableSword()
    {
        sword.GetComponent<Sword>().EnableCollider();
    }
    void DisableSword()
    {
        sword.GetComponent<Sword>().DisableCollider();
    }

    public IEnumerator Knockback(Vector2 Direction, float force, float duration)
    {
        player.linearVelocity = Vector2.zero;
        movable = false;
        anim.SetBool("running", false);
        running = false;
        //CameraScript.freeze = true;
        float knockbackstrength = 0f;
        
        float timer = 0f;
        
        
        while (timer <= duration)
        {
            knockbackstrength = Mathf.Lerp(force, 0, timer / duration);
            player.linearVelocity = Direction * knockbackstrength;
            timer+= Time.deltaTime;
            
            yield return null;
        }
        
        //CameraScript.freeze = false;
        movable = true;
    }

    
   
    void runningAnim()
    {
        if (speedX == 0 && speedY == 0 && running == true)
        {
            anim.SetBool("running",false);
            running = false;
            //Debug.Log("running set false");
        }
        else if ((speedX!= 0 || speedY!= 0) && running == false )
        {
            anim.SetBool("running", true);
            running = true;
            //Debug.Log("running set true");
        }
        
    }
    private void turnplayer()
    {
        if (speedX != 0 || speedY != 0) // Only rotate when moving
        {
            float angle = Mathf.Atan2(speedY, speedX) * Mathf.Rad2Deg; // Convert to degrees
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust by -90 degrees
        }
    }
    

    void SetPlayerAlpha(float alpha)
    {
        foreach (SpriteRenderer rend in rendr)
        {
            Color c = rend.color;
            c.a = alpha;
            rend.color = c;
        }
    }

    public void takeDamage(float damage)
    {
        playerHealth.takeDamage(damage);
        
    }

    public void ResetPlayerAlpha()
    {
        ishurt = false;
        foreach (SpriteRenderer rend in rendr)
        {
            Color c = rend.color;
            c.a = 1f;
            rend.color = c;
        }
    }

    public void RootPlayer()
    {
        StopAllCoroutines();
        movable = false;
        player.linearVelocity = Vector2.zero;
        anim.SetBool("running", false);
        running = false;
    }
}
