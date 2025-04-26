using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public enum FloorMap { floor1, floor2 ,floor3};
    public FloorMap map;
    public ScreenLoader screenLoader;
    public GameObject healthBar;
    public Player player;
    public PlayerHealth playerHealth;
    public enum PieceType { Pawn, Knight, Bishop, Rook, Queen }
    public PieceType pieceType = PieceType.Pawn;

    [Header("Skill Icons")]
    public GameObject SkillIcons;
    public Image AttackIcon;
    public Image AbilityIcon;
    public Image UltimateAbilityIcon;

    public GameObject PopUp;
    public TextMeshProUGUI popUpText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        screenLoader = GetComponent<ScreenLoader>();
        
        
        Game.Instance.playerHealth = healthBar.GetComponent<PlayerHealth>();
        StartCoroutine(screenLoader.LoadMainmenu());
        map = FloorMap.floor1;
    }
    
    public void Freeze()
    {
        Time.timeScale = 0f;
    }

    public void UnFreeze()
    {
        Time.timeScale = 1f;
    }

    public void LoadFloorMap1()
    {
        screenLoader.LoadScene("TowerFloor1");
    }

    public void ShowPopUp(string Text)
    {
        popUpText.text = Text;
        PopUp.SetActive(true);
    }

    public void HidePopUp()
    {
        PopUp.SetActive(false);
    }

    public bool IsPopUpOn()
    {
        return PopUp.activeInHierarchy;
    }
   

}
