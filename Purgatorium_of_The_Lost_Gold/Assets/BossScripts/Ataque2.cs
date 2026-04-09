using System.Collections;
using UnityEngine;

public class Ataque2 : MonoBehaviour
{
    [Header("DetecciÃ³n del Jugador")]
    [Tooltip("Tag del objeto jugador.")]
    [SerializeField] private string etiquetaJugador = "Player";
    [Tooltip("Distancia mÃ¡xima para que el ataque se active.")]
    [SerializeField] private float rangoDeteccion = 20f;

    [Header("Collider de Ataque")]
    [Tooltip("BoxCollider con Is Trigger activado que define la zona de daÃ±o.")]
    [SerializeField] private BoxCollider colliderAtaque;

    [Header("Pivote y Posicionamiento")]
    [Tooltip("Transform central alrededor del cual gira la zona de ataque.")]
    [SerializeField] private Transform puntoGiro;
    [Tooltip("Distancia fija desde el pivote hasta la zona de ataque.")]
    [SerializeField] private float radioGiro = 2f;
    [Tooltip("Velocidad mÃ¡xima de giro para apuntar al jugador (grados/segundo).")]
    [SerializeField] private float velocidadGiro = 180f;

    [Header("Tiempos (segundos)")]
    [Tooltip("Segundos en AMARILLO (aviso, el objeto sigue girando).")]
    [SerializeField] private float duracionAviso      = 1.5f;
    [Tooltip("Segundos en ROJO y activo (objeto congelado).")]
    [SerializeField] private float duracionAtaque     = 2f;
    [Tooltip("Espera entre ataques.")]
    [SerializeField] private float tiempoEntreAtaques = 5f;

    [Header("DaÃ±o")]
    [SerializeField] private float danio = 1;

    private static readonly Color ColorAviso  = new Color(1f, 1f, 0f, 0.35f);
    private static readonly Color ColorAtaque = new Color(1f, 0f, 0f, 0.45f);

    private Transform    jugador;
    private bool         cicloEnCurso     = false;
    private bool         faseAtaqueActiva = false;
    private bool         siguiendoJugador = false;
    private float        anguloActual     = 0f;
    private MeshRenderer visualizador;
    public  Material     materialZona;
    public Color colour;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        if (puntoGiro != null)
        {
            Vector3 desplazamiento = transform.position - puntoGiro.position;
            desplazamiento.y = 0f;
            if (desplazamiento.sqrMagnitude > 0.001f)
                anguloActual = Mathf.Atan2(desplazamiento.x, desplazamiento.z) * Mathf.Rad2Deg;
        }

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

        Vector3 origen    = puntoGiro != null ? puntoGiro.position : transform.position;
        float   distancia = Vector3.Distance(origen, jugador.position);

        if (distancia <= rangoDeteccion && !cicloEnCurso)
            StartCoroutine(CicloAtaque());

        if (siguiendoJugador && puntoGiro != null && jugador != null)
            SeguirJugador();
    }

    private void SeguirJugador()
    {
        Vector3 haciaJugador = jugador.position - puntoGiro.position;
        haciaJugador.y = 0f;
        if (haciaJugador.sqrMagnitude < 0.001f) return;

        float anguloObjetivo = Mathf.Atan2(haciaJugador.x, haciaJugador.z) * Mathf.Rad2Deg;
        anguloActual = Mathf.MoveTowardsAngle(anguloActual, anguloObjetivo, velocidadGiro * Time.deltaTime);
        AplicarPosicionOrbita();
    }

    private void AplicarPosicionOrbita()
    {
        float   rad  = anguloActual * Mathf.Deg2Rad;
        Vector3 dir  = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
        transform.position = puntoGiro.position + dir * radioGiro;

        Vector3 mirarHacia = puntoGiro.position - transform.position;
        if (mirarHacia != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(mirarHacia.normalized);
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso     = true;
        faseAtaqueActiva = false;
        siguiendoJugador = true;

        if (colliderAtaque != null) colliderAtaque.enabled = false;

        SetColor(ColorAviso);
        SetZonaVisible(true);

        yield return new WaitForSeconds(duracionAviso);

        siguiendoJugador = false;

        SetColor(ColorAtaque);
        if (colliderAtaque != null) colliderAtaque.enabled = true;
        faseAtaqueActiva = true;

        yield return new WaitForSeconds(duracionAtaque);

        if (colliderAtaque != null) colliderAtaque.enabled = false;
        faseAtaqueActiva = false;
        SetZonaVisible(false);

        siguiendoJugador = true;
        yield return new WaitForSeconds(tiempoEntreAtaques);

        siguiendoJugador = false;
        cicloEnCurso     = false;
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

    private void CrearVisualizador()
    {
        if (colliderAtaque == null) return;

        GameObject vis = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vis.name = "ZonaAtaque2_Visual";
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
        Vector3 origen = puntoGiro != null ? puntoGiro.position : transform.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(origen, rangoDeteccion);

        if (puntoGiro != null)
        {
            Gizmos.color = Color.yellow;
            int   segmentos = 32;
            float paso      = 360f / segmentos;
            for (int i = 0; i < segmentos; i++)
            {
                float a0 = i       * paso * Mathf.Deg2Rad;
                float a1 = (i + 1) * paso * Mathf.Deg2Rad;
                Vector3 p0 = origen + new Vector3(Mathf.Sin(a0), 0f, Mathf.Cos(a0)) * radioGiro;
                Vector3 p1 = origen + new Vector3(Mathf.Sin(a1), 0f, Mathf.Cos(a1)) * radioGiro;
                Gizmos.DrawLine(p0, p1);
            }
        }
    }
}
