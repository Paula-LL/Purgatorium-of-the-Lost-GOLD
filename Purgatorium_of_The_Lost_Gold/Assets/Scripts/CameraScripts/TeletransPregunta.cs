using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeletransPregunta : MonoBehaviour
{
    public GameObject canvasEntradaNivel;

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
}
