using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla el Canvas de Estadisticas accesible desde el Menu Principal.
/// Conecta el boton de apertura directamente a Abrir() desde el Inspector del boton.
/// Solo necesita un unico TMP_Text para mostrar todas las estadisticas.
/// </summary>
public class EstadisticasUI : MonoBehaviour
{
    [Header("Canvas Menu Principal")]
    [Tooltip("Canvas raiz del menu principal. Se oculta al abrir estadisticas y se muestra al cerrar.")]
    [SerializeField] private GameObject canvasMenuPrincipal;

    [Header("Boton de cierre")]
    [SerializeField] private Button botonCerrar;

    [Header("Texto de estadisticas")]
    [Tooltip("Un unico TMP_Text donde se mostraran todas las estadisticas.")]
    [SerializeField] private TMP_Text textoEstadisticas;

    private void Awake()
    {
        // Desactivarse al iniciar sin depender de Start
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Cada vez que el canvas se activa, refrescar los datos
        ActualizarUI();

        if (botonCerrar != null)
        {
            botonCerrar.onClick.RemoveAllListeners();
            botonCerrar.onClick.AddListener(Cerrar);
        }
    }


    /// <summary>Cierra el panel y reactiva el menu principal.</summary>
    public void Cerrar()
    {
        gameObject.SetActive(false);

        if (canvasMenuPrincipal != null)
            canvasMenuPrincipal.SetActive(true);
    }

    /// <summary>Escribe todas las estadisticas en el unico TMP_Text.</summary>
    public void ActualizarUI()
    {
        if (textoEstadisticas == null) return;

        EstadisticasJuego s = EstadisticasJuego.Instancia;

        if (s == null)
        {
            textoEstadisticas.text =
                "Partidas jugadas: 0\n" +
                "Victorias: 0\n" +
                "Enemigos eliminados: 0\n" +
                "Bosses derrotados: 0\n" +
                "Veces que has muerto: 0\n" +
                "Dano total infligido: 0\n" +
                "Dano total recibido: 0\n" +
                "Tiempo total jugado: 00:00:00";
            return;
        }

        textoEstadisticas.text =
            $"Partidas jugadas: {s.PartidasJugadas}\n" +
            $"Victorias: {s.Victorias}\n" +
            $"Enemigos eliminados: {s.EnemigosMatados}\n" +
            $"Bosses derrotados: {s.BossesMatados}\n" +
            $"Veces que has muerto: {s.MuerteJugador}\n" +
            $"Dano total infligido: {s.DanoHecho:F0}\n" +
            $"Dano total recibido: {s.DanoRecibido:F0}\n" +
            $"Tiempo total jugado: {s.TiempoTotalFormateado()}";
    }
}