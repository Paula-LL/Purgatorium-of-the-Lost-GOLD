using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public Image healthBar;

    [SerializeField]
    public TMP_Text healthBarText;

    private void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar() { 
        healthBar.fillAmount = Player_controller.instance.currentPlayerStats.currentHealth/Player_controller.instance.currentPlayerStats.maxHealth;
        healthBarText.text = Player_controller.instance.currentPlayerStats.currentHealth + "/" + Player_controller.instance.currentPlayerStats.maxHealth; 
    }

}
