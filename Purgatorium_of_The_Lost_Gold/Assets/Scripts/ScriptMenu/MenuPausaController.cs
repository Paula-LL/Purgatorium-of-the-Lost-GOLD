using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

public class MenuPausaController : MonoBehaviour
{
    [Header("CANVAS CONFIGURATION")]
    public GameObject opcionesCanvas;
    public GameObject hpBar;

    [Header("AUDIO REFERENCES")]
    public AudioMixer audioMixer;

    [Header("AUDIO MIXER PARAMETERS")]
    [SerializeField] string parametroMusica = "MusicaFondo";
    [SerializeField] string parametroEfectos = "MusicaGeneral";

    [Header("AUDIO")]
    public Slider volumenSliderMusica;
    public Slider volumenEfectosSlider;

    [Header("VIDEO")]
    public Toggle pantallaCompletaToggle;
    public TMP_Dropdown calidadVideo;

    [Header("EXIT BUTTONS")]
    public Button cambiarEscenaButton;
    public Button salirButton;

    [Header("SCENE MANAGEMENT")]
    public string escenaMenu = "main menu";
    public string escenaVictoria = "Victoria";

    private static MenuPausaController instance;
    private bool ajustesCargados = false;



    void Awake()
    {
        if (SceneManager.GetActiveScene().name == escenaMenu)
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name == escenaVictoria)
        {
            Destroy(gameObject);
            return;
        }

        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == escenaMenu)
        {
            Destroy(gameObject);
            return;
        }

        ReconfigurarEnNuevaEscena();
    }

    void Start()
    {
        if (!ajustesCargados)
        {
            ConfigurarMenuPausa();
            ajustesCargados = true;
        }
    }

    void ConfigurarMenuPausa()
    {
        if (opcionesCanvas != null)
            opcionesCanvas.gameObject.SetActive(false);

        ConfigurarEventos();
        CargarAjustesGuardados();
    }

    void ConfigurarEventos()
    {
        if (volumenSliderMusica != null)
            volumenSliderMusica.onValueChanged.RemoveAllListeners();

        if (volumenEfectosSlider != null)
            volumenEfectosSlider.onValueChanged.RemoveAllListeners();

        if (pantallaCompletaToggle != null)
            pantallaCompletaToggle.onValueChanged.RemoveAllListeners();

        if (calidadVideo != null)
            calidadVideo.onValueChanged.RemoveAllListeners();

        if (cambiarEscenaButton != null)
            cambiarEscenaButton.onClick.RemoveAllListeners();

        if (salirButton != null)
            salirButton.onClick.RemoveAllListeners();

        if (volumenSliderMusica != null)
            volumenSliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);

        if (volumenEfectosSlider != null)
            volumenEfectosSlider.onValueChanged.AddListener(CambiarVolumenEfectos);

        if (pantallaCompletaToggle != null)
            pantallaCompletaToggle.onValueChanged.AddListener(CambiarPantallaCompleta);

        if (calidadVideo != null)
        {
            ConfigurarDropdownResoluciones();
            calidadVideo.onValueChanged.AddListener(CambiarResolucion);
        }

        if (cambiarEscenaButton != null)
            cambiarEscenaButton.onClick.AddListener(CambiarEscena);

        if (salirButton != null)
            salirButton.onClick.AddListener(SalirJuego);
    }

    void ReconfigurarEnNuevaEscena()
    {
        if (opcionesCanvas != null)
            opcionesCanvas.gameObject.SetActive(false);

        ConfigurarEventos();
        CargarAjustesGuardados();
    }

    void CargarAjustesGuardados()
    {
        if (volumenSliderMusica != null)
        {
            float volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica", 0.8f);
            volumenSliderMusica.value = volumenGuardado;
            AplicarVolumenMusica(volumenGuardado);
        }

        if (volumenEfectosSlider != null)
        {
            float volumenGuardado = PlayerPrefs.GetFloat("VolumenEfectos", 0.8f);
            volumenEfectosSlider.value = volumenGuardado;
            AplicarVolumenEfectos(volumenGuardado);
        }

        if (pantallaCompletaToggle != null)
        {
            bool pantallaCompletaGuardada = PlayerPrefs.GetInt("PantallaCompleta", Screen.fullScreen ? 1 : 0) == 1;
            pantallaCompletaToggle.isOn = pantallaCompletaGuardada;
        }

        if (calidadVideo != null)
        {
            ConfigurarDropdownResoluciones();
            int resolucionGuardada = PlayerPrefs.GetInt("ResolucionIndex", -1);
            if (resolucionGuardada >= 0 && resolucionGuardada < calidadVideo.options.Count)
            {
                calidadVideo.value = resolucionGuardada;
            }
        }
    }

    void AplicarAjustesGuardados()
    {
        if (volumenSliderMusica != null)
            AplicarVolumenMusica(volumenSliderMusica.value);

        if (volumenEfectosSlider != null)
            AplicarVolumenEfectos(volumenEfectosSlider.value);
    }

    private void Update()
    {
        detectarteclado();
    }

    public void detectarteclado()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            bool activar = !opcionesCanvas.gameObject.activeSelf;
            opcionesCanvas.gameObject.SetActive(true);
            hpBar.gameObject.SetActive(false);

            if (activar)
            {
                PausarFisicas();
            }
            else
            {
                ReanudarFisicas();
                
                opcionesCanvas.gameObject.SetActive(false);
                hpBar.gameObject.SetActive(true);
            }
        }
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ------------------------------------------------------------------
    //                   CONTROL DE FISICAS
    // ------------------------------------------------------------------

    void PausarFisicas()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    void ReanudarFisicas()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    // ------------------------------------------------------------------
    //                   CONTROL DE AUDIO
    // ------------------------------------------------------------------

    void CambiarVolumenMusica(float valor)
    {
        AplicarVolumenMusica(valor);
        PlayerPrefs.SetFloat("VolumenMusica", valor);
        PlayerPrefs.Save();
    }

    void AplicarVolumenMusica(float valor)
    {
        if (audioMixer != null)
        {
            float volumenDB = Mathf.Log10(valor <= 0 ? 0.0001f : valor) * 20;
            audioMixer.SetFloat(parametroMusica, volumenDB);
        }
    }

    void CambiarVolumenEfectos(float valor)
    {
        AplicarVolumenEfectos(valor);
        PlayerPrefs.SetFloat("VolumenEfectos", valor);
        PlayerPrefs.Save();
    }

    void AplicarVolumenEfectos(float valor)
    {
        if (audioMixer != null)
        {
            float volumenDB = Mathf.Log10(valor <= 0 ? 0.0001f : valor) * 20;
            audioMixer.SetFloat(parametroEfectos, volumenDB);
        }
    }

    // ------------------------------------------------------------------
    //                   OPCIONES DE VIDEO
    // ------------------------------------------------------------------

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
            string resString = res.width + "x" + res.height;
            if (!resolucionesUnicas.Contains(resString))
            {
                resolucionesUnicas.Add(resString);
                opciones.Add(resString);
            }
        }

        calidadVideo.AddOptions(opciones);

        int resolucionGuardada = PlayerPrefs.GetInt("ResolucionIndex", -1);
        if (resolucionGuardada >= 0 && resolucionGuardada < opciones.Count)
        {
            calidadVideo.value = resolucionGuardada;
        }
        else
        {
            calidadVideo.value = 0;
        }
    }

    void CambiarResolucion(int indice)
    {
        AplicarResolucionGuardada(indice);
        PlayerPrefs.SetInt("ResolucionIndex", indice);
        PlayerPrefs.Save();
    }

    void AplicarResolucionGuardada(int indice)
    {
        List<Resolution> resolucionesUnicas = new List<Resolution>();
        HashSet<string> resSet = new HashSet<string>();

        foreach (Resolution res in Screen.resolutions)
        {
            string resString = res.width + "x" + res.height;
            if (!resSet.Contains(resString))
            {
                resSet.Add(resString);
                resolucionesUnicas.Add(res);
            }
        }

        if (indice < resolucionesUnicas.Count)
        {
            Resolution res = resolucionesUnicas[indice];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
    }

    // ------------------------------------------------------------------
    //                   BOTONES
    // ------------------------------------------------------------------

    void CambiarEscena()
    {
        ReanudarFisicas();
        PlayerPrefs.Save();
        SceneManager.LoadScene(escenaMenu);
    }

    void SalirJuego()
    {
        Application.Quit();
    }
}