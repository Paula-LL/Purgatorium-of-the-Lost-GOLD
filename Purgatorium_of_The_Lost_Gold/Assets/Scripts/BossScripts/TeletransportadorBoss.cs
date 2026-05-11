using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeletransportadorBoss : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag== "Player") 
        {
            GuardarEstadisticas.Instance.GuardarStats();
            SceneManager.LoadScene("BossBattle");
        }
    }
}
