using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] float CooldownDuration = 0f;
    [SerializeField] GameObject Timer;
    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] float TotalTime;
    [SerializeField] GameObject player;
    [SerializeField] GameObject wallpath;
    [SerializeField] GameObject wall2;


    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {

        Game.Instance.player.RootPlayer();
        Game.Instance.player.ResetPlayerAlpha();
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<Cameracontroller>().freeze = true;
        yield return StartCoroutine(mainCamera.GetComponent<Cameracontroller>().LerpAndAction(EnablePathwall, wallpath));
        Game.Instance.player.movable = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(SpawnTimer());
        SpawnEnemy();
        yield break;



    }

    void EnablePathwall()
    {
        wallpath.SetActive(true);
    }
    void DisablePathwal2()
    {
        wall2.SetActive(false);
    }

    public void SpawnEnemy()
    {
        GameObject EnemyToSpawn = null;
        GameObject LocationToSpawn = null;

        foreach (var enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
            {
                if(EnemyToSpawn == null)
                {
                    EnemyToSpawn = enemy;
                }
                else
                {
                    if (Random.value > 0.66f)
                    {
                        EnemyToSpawn = enemy;
                    }
                }
                
            }
        }
        int RandNum = Random.Range(0, enemies.Length);
        LocationToSpawn = spawnPoints[RandNum];
        if (EnemyToSpawn != null)
        {
            EnemyToSpawn.transform.position = LocationToSpawn.transform.position;
            EnemyToSpawn.GetComponent<Enemy>().health.ResetHealth();
            
            EnemyToSpawn.GetComponent<Enemy>().enemyState = Enemy.EnemyState.Idle;
            if (EnemyToSpawn.GetComponent<EnemyPawn>() != null)
            {
                EnemyToSpawn.GetComponent<EnemyPawn>().aggro = true;
            }

            EnemyToSpawn.SetActive(true);
            if (EnemyToSpawn.GetComponent<EnemyPawnAI>() != null)
            {
                EnemyToSpawn.GetComponent<EnemyPawnAI>().AIon = false;
            }
            else
            {
                StartCoroutine(EnemyToSpawn.GetComponent<EnemyBomber>().Attack());
            }

            Debug.Log("spawning Enemies");
        }
        StartCoroutine(EnterCooldown());

    }

    IEnumerator EnterCooldown()
    {
        float timer = 0f;
        while (timer < CooldownDuration)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        SpawnEnemy();
    }

    public IEnumerator SpawnTimer()
    {
        TimerText.text = TotalTime.ToString("F0");
        Timer.SetActive(true);
        float timeLeft = TotalTime;
        while (timeLeft>=0)
        {
            TimerText.text = timeLeft.ToString("F0");
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        if (timeLeft <= 0f)
        {
            DisableMap2stuff();
            Game.Instance.player.movable = false;
            GameObject mainCamera = GameObject.Find("Main Camera");
            mainCamera.GetComponent<Cameracontroller>().freeze = true;
            yield return StartCoroutine(mainCamera.GetComponent<Cameracontroller>().LerpAndAction(DisablePathwal2, wall2));
            Game.Instance.player.movable = true;
            Game.Instance.map = Game.FloorMap.floor3;
            
            
            

        }
        
    }
    void DisableMap2stuff()
    {
        StopAllCoroutines();
        foreach (var enemy in enemies)
        {
            enemy.SetActive(false);
        }
        Timer.SetActive(false);
        TimerText.enabled = false;
    }
}
    
    
