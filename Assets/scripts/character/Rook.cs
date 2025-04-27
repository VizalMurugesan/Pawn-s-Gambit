using UnityEngine;

public class Rook : MonoBehaviour
{
    public bool isFortify = false;
    Player playerScript;
    [SerializeField] GameObject fortifyEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && playerScript.GetPiece() == "rook" )
        {
            if (!isFortify)
            {
                playerScript.movementSpeed = playerScript.movementSpeed / 1.5f;
                isFortify = true;
                fortifyEffect.SetActive(true);
            }
            
                
            
            else
            {
                playerScript.movementSpeed = playerScript.movementSpeed * 1.5f;
                isFortify = false ;
                fortifyEffect.SetActive(false);
            }
            
        }

    }
}
