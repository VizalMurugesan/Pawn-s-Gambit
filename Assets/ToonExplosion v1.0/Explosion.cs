using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Parent;
    public void disableExplosion()
    {
        Parent.SetActive(false);
    }
}

