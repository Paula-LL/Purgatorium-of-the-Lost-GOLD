using UnityEngine;
using System.Collections;

public class TravelingCamara : MonoBehaviour
{
    [Header("Destino")]
    public Transform puntoFinalLibro;

    [Header("Configuración")]
    public float delayAntesDeMover = 2.0f;
    public float velocidadMover = 2.0f;

    [Header("UI Final")]
    public CanvasGroup canvasMenuLibro; // Arrastra aquí el Canvas Group
    public float velocidadFade = 1.5f;

    private bool puedeMover = false;
    private float contadorTiempo = 0f;
    private Vector3 posicionInicial;
    private Quaternion rotInicial;

    public void EmpezarViaje()
    {
        StartCoroutine(EsperarYViajar());
    }

    IEnumerator EsperarYViajar()
    {
        yield return new WaitForSeconds(delayAntesDeMover);

        posicionInicial = transform.position;
        rotInicial = transform.rotation;
        contadorTiempo = 0f;
        puedeMover = true;
    }

    void Update()
    {
        if (puedeMover)
        {
            contadorTiempo += Time.deltaTime * (velocidadMover / 5f);
            float suavizado = Mathf.SmoothStep(0f, 1f, contadorTiempo);

            transform.position = Vector3.Lerp(posicionInicial, puntoFinalLibro.position, suavizado);
            transform.rotation = Quaternion.Slerp(rotInicial, puntoFinalLibro.rotation, suavizado);

            if (contadorTiempo >= 1f)
            {
                puedeMover = false;
                // En lugar de un SetActive, lanzamos el Fade In
                StartCoroutine(FadeInMenu());
            }
        }
    }

    IEnumerator FadeInMenu()
    {
        while (canvasMenuLibro.alpha < 1f)
        {
            canvasMenuLibro.alpha += Time.deltaTime * velocidadFade;
            yield return null;
        }

        // Aseguramos que los botones sean interactuables al terminar
        canvasMenuLibro.interactable = true;
        canvasMenuLibro.blocksRaycasts = true;
    }
}