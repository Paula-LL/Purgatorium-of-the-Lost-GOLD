using UnityEngine;
using UnityEngine.UI;

public class ActivarDesactivarCanvas : MonoBehaviour
{
    [Header("Configuración de Canvas")]
    public Canvas canvasActivar;
    public Canvas canvasDesactivar;

    [Header("Configuración del Botón")]
    public Button botonActivar;

    [Header("Sonido de Botón")]
    public AudioClip sonidoClick;
    public AudioSource audioSource;

    private void Start()
    {
        if (botonActivar != null)
        {
            botonActivar.onClick.AddListener(AlternarCanvas);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ningún botón en el Inspector.", this);
        }
    }

    public void AlternarCanvas()
    {
        if (canvasActivar != null)
        {
            canvasActivar.gameObject.SetActive(true);
        }

        if (canvasDesactivar != null)
        {
            canvasDesactivar.gameObject.SetActive(false);
        }

        ReproducirSonido();
    }

    private void ReproducirSonido()
    {
        if (sonidoClick != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(sonidoClick);
            }
            else
            {
                AudioSource.PlayClipAtPoint(sonidoClick, Camera.main != null ? Camera.main.transform.position : Vector3.zero);
            }
        }
    }
}
