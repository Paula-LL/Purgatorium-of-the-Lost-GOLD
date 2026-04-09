using System.Collections;
using UnityEngine;

public class Ataque1 : MonoBehaviour
{
    [Header("DetecciÃ³n del Jugador")]
    [Tooltip("Tag del objeto jugador.")]
    [SerializeField] private string etiquetaJugador = "Player";
    [Tooltip("Distancia mÃ¡xima para que el ataque se active.")]
    [SerializeField] private float rangoDeteccion = 20f;

    [Header("Collider de Ataque")]
    [Tooltip("BoxCollider con Is Trigger activado que define la zona de daÃ±o.")]
    [SerializeField] private BoxCollider colliderAtaque;

    [Header("Tiempos (segundos)")]
    [Tooltip("Segundos en AMARILLO (aviso) antes de atacar.")]
    [SerializeField] private float duracionAviso      = 1.5f;
    [Tooltip("Segundos en ROJO y activo (ataque).")]
    [SerializeField] private float duracionAtaque     = 1.5f;
    [Tooltip("Espera entre ataques.")]
    [SerializeField] private float tiempoEntreAtaques = 4f;

    [Header("DaÃ±o")]
    [SerializeField] private float danio = 1;

    private static readonly Color ColorAviso  = new Color(1f, 1f, 0f, 0.35f);
    private static readonly Color ColorAtaque = new Color(1f, 0f, 0f, 0.45f);

    private Transform    jugador;
    private bool         cicloEnCurso     = false;
    private bool         faseAtaqueActiva = false;
    private MeshRenderer visualizador;
    public Material     materialZona;
    public Color colour;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        CrearVisualizador();

        if (colliderAtaque != null) colliderAtaque.enabled = false;
        SetZonaVisible(false);
    }

    void Update()
    {
        if (jugador == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
            if (obj != null) jugador = obj.transform;
            return;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion && !cicloEnCurso)
            StartCoroutine(CicloAtaque());
    }

    private void CrearVisualizador()
    {
        if (colliderAtaque == null) return;

        GameObject vis = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vis.name = "ZonaAtaque1_Visual";
        vis.transform.SetParent(colliderAtaque.transform, false);
        vis.transform.localPosition = colliderAtaque.center;
        vis.transform.localRotation = Quaternion.identity;
        vis.transform.localScale    = colliderAtaque.size;

        Destroy(vis.GetComponent<Collider>());

        visualizador = vis.GetComponent<MeshRenderer>();
        visualizador.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        visualizador.receiveShadows    = false;

        Shader shaderTransp = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        if (shaderTransp == null) shaderTransp = Shader.Find("Transparent/Diffuse");
        if (shaderTransp == null) shaderTransp = Shader.Find("Standard");


        materialZona.color = colour;
        visualizador.material = materialZona;
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso     = true;
        faseAtaqueActiva = false;

        SetColor(ColorAviso);
        SetZonaVisible(true);
        if (colliderAtaque != null) colliderAtaque.enabled = false;

        yield return new WaitForSeconds(duracionAviso);

        SetColor(ColorAtaque);
        if (colliderAtaque != null) colliderAtaque.enabled = true;
        faseAtaqueActiva = true;

        yield return new WaitForSeconds(duracionAtaque);

        if (colliderAtaque != null) colliderAtaque.enabled = false;
        faseAtaqueActiva = false;
        SetZonaVisible(false);

        yield return new WaitForSeconds(tiempoEntreAtaques);

        cicloEnCurso = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!faseAtaqueActiva) return;

        if (other.CompareTag(etiquetaJugador))
        {
            Player_controller pc = other.GetComponent<Player_controller>();
            if (pc != null) pc.TakeDamage(danio);
        }
    }

    private void SetColor(Color color)
    {
        if (materialZona != null) materialZona.color = color;
    }

    private void SetZonaVisible(bool visible)
    {
        if (visualizador != null) visualizador.enabled = visible;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
