using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BotonCreditos : MonoBehaviour
{
    [Header("Botón que cambiará la escena")]
    public Button boton;

    [Header("Nombre de la escena a cargar")]
    public string nombreEscena;

    [Header("Sonido al pulsar")]
    public AudioClip sonidoClick;
    public AudioSource audioSource;

    private void Start()
    {
        if (boton != null)
        {
            boton.onClick.AddListener(CambiarEscena);
            boton.onClick.AddListener(ReproducirSonido);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ningún botón en el Inspector.");
        }
    }

    private void ReproducirSonido()
    {
        if (sonidoClick != null)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(sonidoClick);
            else
                AudioSource.PlayClipAtPoint(sonidoClick, Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }
    }

    public void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el nombre de la escena.");
        }
    }
}

