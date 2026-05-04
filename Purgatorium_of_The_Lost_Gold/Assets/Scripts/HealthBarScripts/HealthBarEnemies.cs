using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBarEnemies : MonoBehaviour
{
    [SerializeField]
    public Image healthBar;
    public GameObject Enemigo;
   
    public Canvas canvasVida;


    [SerializeField]
    public TMP_Text healthBarText;

    private void Update()
    {
        healthBar.canvas.transform.position = Enemigo.transform.position;
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        if (Enemigo.GetComponent<EnemigoBase>() != null)
        {
            healthBar.fillAmount = Enemigo.GetComponent<EnemigoBase>().stats.currentHealth / Enemigo.GetComponent<EnemigoBase>().stats.maxHealth;
            healthBarText.text = Enemigo.GetComponent<EnemigoBase>().stats.currentHealth + "/" + Enemigo.GetComponent<EnemigoBase>().stats.maxHealth;
        }
        else if (Enemigo.GetComponent<EnemigoDist>() != null)
        {
            healthBar.fillAmount = Enemigo.GetComponent<EnemigoDist>().stats.currentHealth / Enemigo.GetComponent<EnemigoDist>().stats.maxHealth;
            healthBarText.text = Enemigo.GetComponent<EnemigoDist>().stats.currentHealth + "/" + Enemigo.GetComponent<EnemigoDist>().stats.maxHealth;
        }
    }

    private void LateUpdate()
    {
            canvasVida.transform.LookAt(Camera.main.transform.position);
    }
}
