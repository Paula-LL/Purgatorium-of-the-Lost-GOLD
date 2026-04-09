using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ControllerAjustes : MonoBehaviour
{
    [Header("CANVAS CONFIGURATION")]
    public Canvas canvasParaActivar;
    public Canvas canvasParaDesactivar;

    [Header("BUTTON REFERENCES")]
    public Button botonAlternarCanvas;

    [Header("BUTTON SOUND REFERENCES")]
    public AudioClip sonidoBoton;
    public AudioSource audioSourceBoton;


    [Header("AUDIO REFERENCES")]
    public AudioMixer audioMixer;

    [Header("AUDIO MIXER PARAMETERS")]
    [SerializeField] string parametroMusica = "MusicaFondo";
    [SerializeField] string parametroEfectos = "MusicaGeneral";

    [Header("AUDIO SETTINGS")]
    public Slider volumenSliderMusica;
    public Slider volumenEfectosSlider;

    [Header("VIDEO SETTINGS")]
    public Toggle pantallaCompletaToggle;
    public TMP_Dropdown calidadVideo;

    void Start()
    {
        CargarAjustesGuardados();
        ConfigurarEventos();
    }

    void ConfigurarEventos()
    {
        // Configurar botón de alternar canvas
        if (botonAlternarCanvas != null)
        {
            botonAlternarCanvas.onClick.AddListener(AlternarCanvas);
            botonAlternarCanvas.onClick.AddListener(ReproducirSonido);
        }

        // Configurar sliders de audio
        if (volumenSliderMusica != null)
        {
            volumenSliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        }

        if (volumenEfectosSlider != null)
        {
            volumenEfectosSlider.onValueChanged.AddListener(CambiarVolumenEfectos);
        }

        // Configurar pantalla completa
        if (pantallaCompletaToggle != null)
        {
            pantallaCompletaToggle.onValueChanged.AddListener(CambiarPantallaCompleta);
        }

        // Configurar resoluciones
        if (calidadVideo != null)
        {
            calidadVideo.onValueChanged.AddListener(CambiarResolucion);
        }
    }

    void CargarAjustesGuardados()
    {
        // Cargar y aplicar ajustes de audio
        if (volumenSliderMusica != null)
        {
            float volumenGuardadoMusica = PlayerPrefs.GetFloat("VolumenMusica", 0.8f);
            volumenSliderMusica.value = volumenGuardadoMusica;
            CambiarVolumenMusica(volumenGuardadoMusica);
        }

        if (volumenEfectosSlider != null)
        {
            float volumenGuardadoEfectos = PlayerPrefs.GetFloat("VolumenEfectos", 0.8f);
            volumenEfectosSlider.value = volumenGuardadoEfectos;
            CambiarVolumenEfectos(volumenGuardadoEfectos);
        }

        // Cargar y aplicar ajustes de video
        if (pantallaCompletaToggle != null)
        {
            bool pantallaCompleta = PlayerPrefs.GetInt("PantallaCompleta", Screen.fullScreen ? 1 : 0) == 1;
            pantallaCompletaToggle.isOn = pantallaCompleta;
            // No aplicar Screen.fullScreen aquí para evitar conflictos
        }

        // Cargar y aplicar resolución
        if (calidadVideo != null)
        {
            ConfigurarDropdownResoluciones();
            int resolucionIndex = PlayerPrefs.GetInt("ResolucionIndex", -1);
            if (resolucionIndex >= 0 && resolucionIndex < calidadVideo.options.Count)
            {
                calidadVideo.value = resolucionIndex;
                // No aplicar resolución automáticamente al cargar
            }
        }
    }

    public void AlternarCanvas()
    {
        if (canvasParaActivar != null)
        {
            canvasParaActivar.gameObject.SetActive(true);
        }

        if (canvasParaDesactivar != null)
        {
            canvasParaDesactivar.gameObject.SetActive(false);
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

    void CambiarVolumenMusica(float valor)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(parametroMusica, Mathf.Log10(valor <= 0 ? 0.0001f : valor) * 20);
            // Guardar preferencia
            PlayerPrefs.SetFloat("VolumenMusica", valor);
            PlayerPrefs.Save();
        }
    }

    void CambiarVolumenEfectos(float valor)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(parametroEfectos, Mathf.Log10(valor <= 0 ? 0.0001f : valor) * 20);
            // Guardar preferencia
            PlayerPrefs.SetFloat("VolumenEfectos", valor);
            PlayerPrefs.Save();
        }
    }

    void CambiarPantallaCompleta(bool esCompleta)
    {
        Screen.fullScreen = esCompleta;
        PlayerPrefs.SetInt("PantallaCompleta", esCompleta ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ConfigurarDropdownResoluciones()
    {
        calidadVideo.ClearOptions();
        List<string> opciones = new List<string>();
        HashSet<string> resolucionesUnicas = new HashSet<string>();

        foreach (Resolution res in Screen.resolutions)
        {
            string resString = res.width + "x" + res.height ;
            if (!resolucionesUnicas.Contains(resString))
            {
                resolucionesUnicas.Add(resString);
                opciones.Add(resString);
            }
        }

        calidadVideo.AddOptions(opciones);

        // Cargar resolución guardada o establecer la actual
        int resolucionGuardada = PlayerPrefs.GetInt("ResolucionIndex", -1);
        if (resolucionGuardada >= 0 && resolucionGuardada < opciones.Count)
        {
            calidadVideo.value = resolucionGuardada;
        }
        else
        {
            // Establecer resolución actual como seleccionada
            string resActual = Screen.currentResolution.width + "x" + Screen.currentResolution.height ;
            int indiceActual = opciones.IndexOf(resActual);
            if (indiceActual >= 0)
            {
                calidadVideo.value = indiceActual;
            }
        }
    }

    void CambiarResolucion(int indice)
    {
        AplicarResolucion(indice);

        // Guardar preferencia
        PlayerPrefs.SetInt("ResolucionIndex", indice);
        PlayerPrefs.Save();
    }

    void AplicarResolucion(int indice)
    {
        List<Resolution> resolucionesUnicas = new List<Resolution>();
        HashSet<string> resSet = new HashSet<string>();

        foreach (Resolution res in Screen.resolutions)
        {
            string resString = res.width + "x" + res.height ;
            if (!resSet.Contains(resString))
            {
                resSet.Add(resString);
                resolucionesUnicas.Add(res);
            }
        }

        if (indice < resolucionesUnicas.Count)
        {
            Resolution resSeleccionada = resolucionesUnicas[indice];
            Screen.SetResolution(resSeleccionada.width, resSeleccionada.height, Screen.fullScreen);
        }
    }
}
