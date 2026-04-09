using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscenaVictoria : MonoBehaviour
{
    [SerializeField] private Button botonMenu;

    [SerializeField] private string nombreEscena = "Menu";

    void Start()
    {
        // Asignar los métodos a los botones
        if (botonMenu != null)
            botonMenu.onClick.AddListener(CambiarEscena);

    }
    public void CambiarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
