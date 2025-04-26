using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField] private Health CharacterHealth;
    [SerializeField] private Image totalhealthbar;
    [SerializeField] private Image currenthealthbar;

    private void Start()
    {
        CharacterHealth = GetComponent<Health>();
    }
    public void ChangeHealthUI()
    {
        currenthealthbar.fillAmount = CharacterHealth.currentHealth / CharacterHealth.totalHealth;
    }

    
}