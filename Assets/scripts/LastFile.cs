using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LastFile : MonoBehaviour
{
    BoxCollider2D box;
    public GameObject eighthFileUI;
    Player player;

    //sprites
    public Sprite knight;
    public Sprite bishop;
    public Sprite Rook;
    public Sprite Queen;

    //map2 starting position
    [SerializeField] private GameObject map2startingpoint;
    public EnemySpawner enemySpawner;
    public GameObject wallpath;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Game.Instance.player.RootPlayer();
            Game.Instance.player.ResetPlayerAlpha();
            GameObject Map1 = GameObject.Find("Map1");
            Map1.GetComponent<Room>().DisableEnemies();

            Game.Instance.ShowPopUp(
            "GOOD JOB <color=#00FF00>ROOKIE</color>!!!!!!!!\n\n" +
            "Was that <color=yellow>easy</color>?? mmmm. It's gonna get harder.\n\n" +
            "Hey now before you piss your pant, <color=red>CHOOSE ONE CHESS PIECE</color> to get its abilities"
            );

            yield return new WaitUntil(() => !Game.Instance.IsPopUpOn());
            eighthFileUI.SetActive(true);
            box.enabled = false;
            Game.Instance.playerHealth.StopAllCoroutines();
            
            Game.Instance.Freeze();
            yield break;
        }
    }
    
    
    
    public void KnightPromotion()
    {
        
        Game.Instance.pieceType = Game.PieceType.Knight;
        Debug.Log("piece changed to: knight" );
        StartCoroutine(ChangeMap());

    }
    public void BishopPromotion()
    {
        Game.Instance.pieceType = Game.PieceType.Bishop;
        
        Debug.Log("piece changed to bishop");
        StartCoroutine(ChangeMap());
    }
    public void RookPromotion()
    {
        Game.Instance.pieceType = Game.PieceType.Rook;
        
        Debug.Log("piece changed to rook");
        StartCoroutine(ChangeMap());

    }
    public void QueenPromotion()
    {
        Game.Instance.pieceType = Game .PieceType.Queen;
        
        Debug.Log("piece changed to: "+player.GetPiece());
        
        
    }

    IEnumerator ChangeMap()
    {
        Game.Instance.player.movable = false;
        // resetting player
        Game.Instance.player.StopAllCoroutines();
        
        eighthFileUI.SetActive(false);
        Game.Instance.player.RootPlayer();
        Game.Instance.player.ResetPlayerAlpha();
        Game.Instance.UnFreeze();
        Game.Instance.AbilityIcon.gameObject.SetActive(true);
        Game.Instance.UltimateAbilityIcon.gameObject.SetActive(true);

        //breaking wall
        GameObject mainCamera = GameObject.Find("Main Camera");
        yield return StartCoroutine(mainCamera.GetComponent<Cameracontroller>().LerpAndAction(DisablePathwall, wallpath));


        Game.Instance.player.movable = true;
    }

    void DisablePathwall()
    {
        wallpath.SetActive(false);
    }

    void EnablePathwall()
    {
        wallpath.SetActive(true);
    }
}
