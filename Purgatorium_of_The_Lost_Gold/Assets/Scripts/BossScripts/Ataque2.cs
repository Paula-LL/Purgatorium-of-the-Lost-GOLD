using System.Collections;
using UnityEngine;

public class Ataque2 : MonoBehaviour
{
    [Header("Detección del Jugador")]
    [SerializeField] private string etiquetaJugador = "Player";
    [SerializeField] private float rangoDeteccion = 20f;

    [Header("Collider de Ataque")]
    [SerializeField] private BoxCollider colliderAtaque;

    [Header("Rotación")]
    [SerializeField] private Transform puntoGiro;
    [SerializeField] private float velocidadSeguimiento = 60f;
    [SerializeField] private Vector3 ejeGiro = Vector3.up;

    [Header("Tiempos (segundos)")]
    [SerializeField] private float duracionAviso      = 1.5f;
    [SerializeField] private float duracionAtaque     = 2f;
    [SerializeField] private float tiempoEntreAtaques = 5f;

    [Header("Daño")]
    [SerializeField] private float danio = 1;

    private static readonly Color ColorAviso  = new Color(1f, 1f, 0f, 0.35f);
    private static readonly Color ColorAtaque = new Color(1f, 0f, 0f, 0.45f);

    private Transform      jugador;
    private bool           cicloEnCurso      = false;
    private bool           faseAtaqueActiva  = false;
    private bool           siguiendoJugador  = false;
    private bool           _yaGolpeado       = false;
    private MeshRenderer   visualizador;
    private PatrolMovement _patrulla;
    private Vector3        _posInicialCollider;
    private Quaternion     _rotInicialCollider;
    public  Material       materialZona;
    public  GameObject     boss;
    public  Color          colour;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        if (boss != null) _patrulla = boss.GetComponent<PatrolMovement>();

        if (colliderAtaque != null)
        {
            _posInicialCollider = colliderAtaque.transform.localPosition;
            _rotInicialCollider = colliderAtaque.transform.localRotation;
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

        if (boss.GetComponent<BossHealth>().EstaMuerto == true) return;

        if (siguiendoJugador && colliderAtaque != null && puntoGiro != null && jugador != null)
            SeguirJugadorConOrbita();

        float distancia      = Vector3.Distance(transform.position, jugador.position);
        bool  bossMoviendose = _patrulla != null && _patrulla.EstaEnMovimiento;

        if (distancia <= rangoDeteccion && !cicloEnCurso
            && !bossMoviendose && !PatrolMovement.HayAtaqueActivo
            && PatrolMovement.TurnoAtaque == 2)
            StartCoroutine(CicloAtaque());
    }

    private void SeguirJugadorConOrbita()
    {
        // Dirección desde el pivote hacia el jugador (en el plano del eje de giro)
        Vector3 haciaJugador  = jugador.position - puntoGiro.position;
        Vector3 haciaCollider = colliderAtaque.transform.position - puntoGiro.position;

        // Proyectar sobre el plano perpendicular al eje de giro
        haciaJugador  -= Vector3.Dot(haciaJugador,  ejeGiro) * ejeGiro;
        haciaCollider -= Vector3.Dot(haciaCollider, ejeGiro) * ejeGiro;

        if (haciaJugador.sqrMagnitude < 0.001f || haciaCollider.sqrMagnitude < 0.001f) return;

        float anguloObjetivo = Vector3.SignedAngle(haciaCollider, haciaJugador, ejeGiro);
        float giro           = Mathf.MoveTowards(0f, anguloObjetivo, velocidadSeguimiento * Time.deltaTime);

        colliderAtaque.transform.RotateAround(puntoGiro.position, ejeGiro, giro);
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso               = true;
        faseAtaqueActiva           = false;
        _yaGolpeado                = false;
        PatrolMovement.HayAtaqueActivo = true;

        // Restaurar posición de inicio antes de cada ciclo
        if (colliderAtaque != null)
        {
            colliderAtaque.transform.localPosition = _posInicialCollider;
            colliderAtaque.transform.localRotation = _rotInicialCollider;
        }

        if (colliderAtaque != null) colliderAtaque.enabled = false;
        SetColor(ColorAviso);
        SetZonaVisible(true);
        siguiendoJugador = true;      // sigue al player durante el aviso

        yield return new WaitForSeconds(duracionAviso);

        siguiendoJugador = false;     // se congela al atacar
        SetColor(ColorAtaque);
        if (colliderAtaque != null) colliderAtaque.enabled = true;
        faseAtaqueActiva = true;

        yield return new WaitForSeconds(duracionAtaque);

        faseAtaqueActiva = false;
        if (colliderAtaque != null) colliderAtaque.enabled = false;
        SetZonaVisible(false);

        // Liberar el bloqueo ANTES de la espera → el boss puede mirar al player
        PatrolMovement.HayAtaqueActivo = false;

        yield return new WaitForSeconds(tiempoEntreAtaques);

        PatrolMovement.TurnoAtaque = 1;
        cicloEnCurso = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!faseAtaqueActiva || _yaGolpeado) return;

        if (other.CompareTag(etiquetaJugador))
        {
            Player_controller pc = other.GetComponent<Player_controller>();
            if (pc != null)
            {
                pc.TakeDamage(danio);
                _yaGolpeado = true;
            }
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

        // Instanciar para no modificar el asset original del proyecto
        Material matInstancia = new Material(Shader.Find("Universal Render Pipeline/Unlit") ??
                                             Shader.Find("Unlit/Color") ??
                                             Shader.Find("Standard"));
        ConfigurarMaterialTransparente(matInstancia);
        matInstancia.color    = colour;
        materialZona          = matInstancia;
        visualizador.material = matInstancia;
    }

    private static void ConfigurarMaterialTransparente(Material mat)
    {
        // URP Unlit transparente
        Shader urpUnlit = Shader.Find("Universal Render Pipeline/Unlit");
        if (urpUnlit != null)
        {
            mat.shader = urpUnlit;
            mat.SetFloat("_Surface", 1);
            mat.SetFloat("_Blend",   0);
            mat.SetInt("_SrcBlend",  (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend",  (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = 3000;
            return;
        }

        // Fallback Built-in Standard transparent
        Shader std = Shader.Find("Standard");
        if (std != null) mat.shader = std;
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend",  (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend",  (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
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
