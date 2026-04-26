using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerMenuPrincipal : MonoBehaviour
{
    [Header("Configuracion de Botones")]
    [SerializeField] private Button botonInicio;
    [SerializeField] private Button botonAjustes;
    [SerializeField] private Button botonSalir;

    [Header("Botones Extra para Canvas")]
    [SerializeField] private Button botonAyuda;

    [Header("Configuracion de Escena")]
    [SerializeField] private string nombreEscena = "Mapa";

    [Header("Configuracion de Canvas Ajustes")]
    [SerializeField] private Canvas canvasActivar;
    [SerializeField] private Canvas canvasDesactivar;

    [Header("Configuracion de Canvas Ayuda")]
    [SerializeField] private Canvas canvasActivarAyuda;
    [SerializeField] private Canvas canvasDesactivarMenu1;

    [Header("Configuracion de Canvas Estadisticas")]
    [SerializeField] private Button botonEstadisticasCanvas;
    [SerializeField] private Canvas canvasActivarEstadis;
    [SerializeField] private Canvas canvasDesactivarMenu;

    [Header("Sonido de Botones")]
    [SerializeField] private AudioClip sonidoBoton;
    [SerializeField] private AudioSource audioSourceBoton;

    void Start()
    {
        if (botonInicio != null)
        {
            botonInicio.onClick.AddListener(CambiarEscena);
            botonInicio.onClick.AddListener(ReproducirSonido);
        }

        if (botonAjustes != null)
        {
            botonAjustes.onClick.AddListener(AlternarCanvas);
            botonAjustes.onClick.AddListener(ReproducirSonido);
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.AddListener(SalirDelJuego);
            botonSalir.onClick.AddListener(ReproducirSonido);
        }

        if (botonAyuda != null)
        {
            botonAyuda.onClick.AddListener(AlternarCanvasAyuda);
            botonAyuda.onClick.AddListener(ReproducirSonido);
        }

        if (botonEstadisticasCanvas != null)
        {
            botonEstadisticasCanvas.onClick.AddListener(AlternarCanvasNuevo);
            botonEstadisticasCanvas.onClick.AddListener(ReproducirSonido);
        }

    }

    private void ReproducirSonido()
    {
        if (sonidoBoton != null)
        {
            if (audioSourceBoton != null)
                audioSourceBoton.PlayOneShot(sonidoBoton);
            else
                AudioSource.PlayClipAtPoint(sonidoBoton, Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }
    }

    public void CambiarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void AlternarCanvas()
    {
        if (canvasActivar != null)
            canvasActivar.gameObject.SetActive(true);

        if (canvasDesactivar != null)
            canvasDesactivar.gameObject.SetActive(false);
    }

    public void AlternarCanvasAyuda()
    {
        if (canvasActivarAyuda != null)
            canvasActivarAyuda.gameObject.SetActive(true);

        if (canvasDesactivarMenu1 != null)
            canvasDesactivarMenu1.gameObject.SetActive(false);
    }

    public void AlternarCanvasNuevo()
    {
        if (canvasActivarEstadis != null)
            canvasActivarEstadis.gameObject.SetActive(true);

        if (canvasDesactivarMenu != null)
            canvasDesactivarMenu.gameObject.SetActive(false);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}