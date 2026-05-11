using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeletransPregunta : MonoBehaviour
{
    public GameObject canvasEntradaNivel;
    public GameObject DantePlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvasEntradaNivel.SetActive(true);
        }
    }

    public void CambioDeEscena()
    {
        SceneManager.LoadScene("lujuria");
    }
    public void DecidirNo()
    {
        Vector3 posDante = DantePlayer.transform.position;
        posDante = posDante + new Vector3(45, 0, 0);
        DantePlayer.transform.position = posDante;
        canvasEntradaNivel.SetActive(false);
        
    }
}
