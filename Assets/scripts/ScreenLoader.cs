using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image totalbar;
    public Image progressbar;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI progressText;
    public float LoadingSpeed = 3f;
    public GameObject mainMenu;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingScreenActivate(sceneName));
    }

    IEnumerator LoadingScreenActivate(string sceneName)
    {
        if (Game.Instance.player != null)
        {
            Game.Instance.playerHealth.StopAllCoroutines();
            Game.Instance.player.StopAllCoroutines();
            //Game.Instance.player = null;
        }
        Game.Instance.SkillIcons.SetActive(false);
        LoadingScreen.SetActive(true);
        float timer = 0f;
        SceneManager.LoadSceneAsync(sceneName);
        while (true)
        {
            progressbar.fillAmount = timer/LoadingSpeed;
            progressText.text = "" + Mathf.RoundToInt((timer / LoadingSpeed) * 100)+" %";
            timer += Time.deltaTime;

            if (timer > LoadingSpeed)
            {
                //GameObject player = GameObject.Find("Player");
                //Player player = FindObjectOfType<Player>();


                Game.Instance.AttackIcon.fillAmount = 1f;
                Game.Instance.UltimateAbilityIcon.fillAmount = 1f;
                Game.Instance.AbilityIcon.fillAmount = 1f;
                LoadingScreen.SetActive(false);
                Game.Instance.healthBar.SetActive(true);
                Game.Instance.SkillIcons.SetActive(true);
                yield break;
            }
            yield return null;

        }


    }

    public IEnumerator LoadMainmenu()
    {
        Game.Instance.healthBar.SetActive(false);
        LoadingScreen.SetActive(true);
        float timer = 0f;
        
        while (true)
        {
            progressbar.fillAmount = timer/LoadingSpeed;
            progressText.text = "" + Mathf.RoundToInt((timer / LoadingSpeed) * 100) + " %";
            timer += Time.deltaTime;

            if (timer > LoadingSpeed)
            {

                LoadingScreen.SetActive(false);
                mainMenu.SetActive(true);
                yield break;
            }
            yield return null;

        }
        
    }

    public bool isLoadingScreenActive()
    {
        return LoadingScreen.activeInHierarchy;

    }

}
