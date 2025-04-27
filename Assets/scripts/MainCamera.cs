using System;
using System.Collections;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{

    public Transform player;
    public bool freeze = false;
    public float inverseSpeed;
    

    void Update()

    {
        if (!freeze)
        {
            
            gameObject.transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }    
    }

    public IEnumerator lerpCamera(Vector2 target)
    {
        float timer = 0f;
        
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z), timer / inverseSpeed);
            timer += Time.deltaTime;
            if (timer > inverseSpeed)
            {
                
                yield break;
            }
            Debug.Log("lerping");
            yield return null;
        }
    }

    public IEnumerator LerpAndAction(Action action, GameObject target)
    {
        freeze = true;

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(lerpCamera(new Vector2(target.transform.position.x, target.transform.position.y)));

        yield return new WaitForSeconds(0.5f);
        action.Invoke();

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(lerpCamera(new Vector2(player.transform.position.x, player.transform.position.y)));

        freeze = false;

    }
}