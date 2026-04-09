using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton persistente que registra y persiste todas las estadísticas globales de la partida.
/// Se mantiene vivo entre escenas. Llama a sus métodos estáticos desde cualquier script.
/// </summary>
public class EstadisticasJuego : MonoBehaviour
{
    public static EstadisticasJuego Instancia { get; private set; }

    // -------------------------------------------------------
    //  CLAVES PLAYERPREFS
    // -------------------------------------------------------
    private const string KEY_ENEMIGOS      = "stat_enemigos";
    private const string KEY_BOSSES        = "stat_bosses";
    private const string KEY_MUERTES       = "stat_muertes";
    private const string KEY_VICTORIAS     = "stat_victorias";
    private const string KEY_TIEMPO_TOTAL  = "stat_tiempo_total";
    private const string KEY_DANO_HECHO    = "stat_dano_hecho";
    private const string KEY_DANO_RECIBIDO = "stat_dano_recibido";
    private const string KEY_PARTIDAS      = "stat_partidas";
    private const string KEY_CARTAS        = "stat_cartas";

    // -------------------------------------------------------
    //  STATS EN MEMORIA (sesión actual + acumulado)
    // -------------------------------------------------------
    public int EnemigosMatados      { get; private set; }
    public int BossesMatados        { get; private set; }
    public int MuerteJugador        { get; private set; }
    public int Victorias            { get; private set; }
    public float TiempoTotalJugado  { get; private set; }  // segundos
    public float DanoHecho          { get; private set; }
    public float DanoRecibido       { get; private set; }
    public int PartidasJugadas      { get; private set; }
    public int CartasRecogidas      { get; private set; }

    // Tiempo de la sesión actual (se acumula en Update)
    private float tiempoSesionActual = 0f;
    private bool contandoTiempo = true;

    // -------------------------------------------------------
    //  CICLO DE VIDA
    // -------------------------------------------------------
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);

        CargarEstadisticas();
        SceneManager.sceneLoaded += OnEscenaCargada;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnEscenaCargada;
    }

    private void OnEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        // Cuando se carga una nueva partida, incrementar partidas jugadas
        if (escena.name == "lujuria" || escena.name == "Level" || escena.name == "Game")
        {
            PartidasJugadas++;
            tiempoSesionActual = 0f;
            GuardarEstadisticas();
        }
    }

    private void Update()
    {
        // Acumular tiempo de juego (no cuenta cuando Time.timeScale == 0, es decir, en pausa)
        if (contandoTiempo && Time.timeScale > 0f)
        {
            tiempoSesionActual += Time.deltaTime;
            TiempoTotalJugado  += Time.deltaTime;
        }
    }

    private void OnApplicationQuit()
    {
        GuardarEstadisticas();
    }

    // -------------------------------------------------------
    //  API PÚBLICA — llamar desde otros scripts
    // -------------------------------------------------------

    public static void RegistrarEnemigoCaido()
    {
        if (Instancia == null) return;
        Instancia.EnemigosMatados++;
        Instancia.GuardarEstadisticas();
    }

    public static void RegistrarBossCaido()
    {
        if (Instancia == null) return;
        Instancia.BossesMatados++;
        Instancia.EnemigosMatados++; // El boss también cuenta como enemigo
        Instancia.GuardarEstadisticas();
    }

    public static void RegistrarMuerteJugador()
    {
        if (Instancia == null) return;
        Instancia.MuerteJugador++;
        Instancia.GuardarEstadisticas();
    }

    public static void RegistrarVictoria()
    {
        if (Instancia == null) return;
        Instancia.Victorias++;
        Instancia.GuardarEstadisticas();
    }

    public static void RegistrarDanoHecho(float cantidad)
    {
        if (Instancia == null) return;
        Instancia.DanoHecho += cantidad;
    }

    public static void RegistrarDanoRecibido(float cantidad)
    {
        if (Instancia == null) return;
        Instancia.DanoRecibido += cantidad;
    }

    public static void RegistrarCartaRecogida()
    {
        if (Instancia == null) return;
        Instancia.CartasRecogidas++;
        Instancia.GuardarEstadisticas();
    }

    /// <summary>Tiempo de la sesión actual en formato legible HH:MM:SS.</summary>
    public string TiempoSesionFormateado()
    {
        return FormatearTiempo(tiempoSesionActual);
    }

    /// <summary>Tiempo total acumulado en formato legible.</summary>
    public string TiempoTotalFormateado()
    {
        return FormatearTiempo(TiempoTotalJugado);
    }

    public static string FormatearTiempo(float segundos)
    {
        int h = Mathf.FloorToInt(segundos / 3600);
        int m = Mathf.FloorToInt((segundos % 3600) / 60);
        int s = Mathf.FloorToInt(segundos % 60);
        return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
    }

    /// <summary>Resetea TODOS los contadores y limpia PlayerPrefs.</summary>
    public static void ResetearTodo()
    {
        if (Instancia == null) return;
        Instancia.EnemigosMatados     = 0;
        Instancia.BossesMatados       = 0;
        Instancia.MuerteJugador       = 0;
        Instancia.Victorias           = 0;
        Instancia.TiempoTotalJugado   = 0f;
        Instancia.DanoHecho           = 0f;
        Instancia.DanoRecibido        = 0f;
        Instancia.PartidasJugadas     = 0;
        Instancia.CartasRecogidas     = 0;
        Instancia.tiempoSesionActual  = 0f;
        Instancia.GuardarEstadisticas();
    }

    // -------------------------------------------------------
    //  PERSISTENCIA
    // -------------------------------------------------------
    private void GuardarEstadisticas()
    {
        PlayerPrefs.SetInt(KEY_ENEMIGOS,      EnemigosMatados);
        PlayerPrefs.SetInt(KEY_BOSSES,        BossesMatados);
        PlayerPrefs.SetInt(KEY_MUERTES,       MuerteJugador);
        PlayerPrefs.SetInt(KEY_VICTORIAS,     Victorias);
        PlayerPrefs.SetFloat(KEY_TIEMPO_TOTAL, TiempoTotalJugado);
        PlayerPrefs.SetFloat(KEY_DANO_HECHO,  DanoHecho);
        PlayerPrefs.SetFloat(KEY_DANO_RECIBIDO, DanoRecibido);
        PlayerPrefs.SetInt(KEY_PARTIDAS,      PartidasJugadas);
        PlayerPrefs.SetInt(KEY_CARTAS,        CartasRecogidas);
        PlayerPrefs.Save();
    }

    private void CargarEstadisticas()
    {
        EnemigosMatados    = PlayerPrefs.GetInt(KEY_ENEMIGOS, 0);
        BossesMatados      = PlayerPrefs.GetInt(KEY_BOSSES, 0);
        MuerteJugador      = PlayerPrefs.GetInt(KEY_MUERTES, 0);
        Victorias          = PlayerPrefs.GetInt(KEY_VICTORIAS, 0);
        TiempoTotalJugado  = PlayerPrefs.GetFloat(KEY_TIEMPO_TOTAL, 0f);
        DanoHecho          = PlayerPrefs.GetFloat(KEY_DANO_HECHO, 0f);
        DanoRecibido       = PlayerPrefs.GetFloat(KEY_DANO_RECIBIDO, 0f);
        PartidasJugadas    = PlayerPrefs.GetInt(KEY_PARTIDAS, 0);
        CartasRecogidas    = PlayerPrefs.GetInt(KEY_CARTAS, 0);
    }
}
