using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemies : MonoBehaviour
{
    [SerializeField]
    public Image healthBar;
    public GameObject Enemigo;
    private Camera cameraActual;
    public Canvas canvasVida;


    [SerializeField]
    public TMP_Text healthBarText;

    private void Start()
    {
        cameraActual = Camera.main;
        canvasVida.GetComponent<Canvas>().worldCamera = cameraActual;

        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = Enemigo.GetComponent<EnemigoBase>().stats.currentHealth / Enemigo.GetComponent<EnemigoBase>().stats.maxHealth;
        healthBarText.text = Enemigo.GetComponent<EnemigoBase>().stats.currentHealth + "/" + Enemigo.GetComponent<EnemigoBase>().stats.maxHealth;
    }
}
