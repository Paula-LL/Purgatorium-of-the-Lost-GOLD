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
    [SerializeField] private float duracionAviso      = 1.5f;
    [SerializeField] private float duracionAtaque     = 1.5f;
    [SerializeField] private float tiempoEntreAtaques = 4f;

    [Header("Rotación del Ataque")]
    [SerializeField] private float anguloBarrido = 90f;
    [SerializeField] private Transform pivoteRotacion;

    [Header("DaÃ±o")]
    [SerializeField] private float danio = 1;

    private static readonly Color ColorAviso  = new Color(1f, 1f, 0f, 0.35f);
    private static readonly Color ColorAtaque = new Color(1f, 0f, 0f, 0.45f);

    private Transform    jugador;
    private bool         cicloEnCurso     = false;
    private bool         faseAtaqueActiva = false;
    private bool         _yaGolpeado      = false;
    private MeshRenderer visualizador;
    private PatrolMovement _patrulla;
    private Vector3    _posInicialCollider;
    private Quaternion _rotInicialCollider;
    public Material     materialZona;
    public GameObject boss;
    public Color colour;

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
        if (boss.GetComponent<BossHealth>().EstaMuerto == true)
        {
            return;
        }
        float distancia = Vector3.Distance(transform.position, jugador.position);

        bool bossMoviendose = _patrulla != null && _patrulla.EstaEnMovimiento;

        if (distancia <= rangoDeteccion && !cicloEnCurso
            && !bossMoviendose && !PatrolMovement.HayAtaqueActivo
            && PatrolMovement.TurnoAtaque == 1)
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
            mat.SetFloat("_Surface", 1);           // 1 = Transparent
            mat.SetFloat("_Blend",   0);           // 0 = Alpha
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

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso               = true;
        faseAtaqueActiva           = false;
        _yaGolpeado                = false;
        PatrolMovement.HayAtaqueActivo = true;

        // Resetear posición antes de cada ataque
        if (colliderAtaque != null)
        {
            colliderAtaque.transform.localPosition = _posInicialCollider;
            colliderAtaque.transform.localRotation = _rotInicialCollider;
        }

        SetColor(ColorAviso);
        SetZonaVisible(true);
        if (colliderAtaque != null) colliderAtaque.enabled = false;

        yield return new WaitForSeconds(duracionAviso);

        SetColor(ColorAtaque);
        if (colliderAtaque != null) colliderAtaque.enabled = true;
        faseAtaqueActiva = true;

        yield return BarrerAngulo();

        if (colliderAtaque != null) colliderAtaque.enabled = false;
        faseAtaqueActiva = false;
        SetZonaVisible(false);

        PatrolMovement.HayAtaqueActivo = false;

        yield return new WaitForSeconds(tiempoEntreAtaques);

        PatrolMovement.TurnoAtaque = 2;
        cicloEnCurso = false;
    }

    private IEnumerator BarrerAngulo()
    {
        if (colliderAtaque == null || duracionAtaque <= 0f)
        {
            yield return new WaitForSeconds(duracionAtaque);
            yield break;
        }

        Transform pivote = pivoteRotacion != null ? pivoteRotacion : transform;

        // Posición y ángulo inicial tomados del collider en este momento
        Vector3 posOrigen = colliderAtaque.transform.position;
        Vector3 offset    = posOrigen - pivote.position;
        float   radio     = new Vector2(offset.x, offset.z).magnitude;
        float   anguloInicio = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        float   anguloFin    = anguloInicio + anguloBarrido;

        float tiempo = 0f;
        while (tiempo < duracionAtaque)
        {
            tiempo += Time.deltaTime;
            float t       = Mathf.Clamp01(tiempo / duracionAtaque);
            float angulo  = Mathf.LerpAngle(anguloInicio, anguloFin, t);
            float rad     = angulo * Mathf.Deg2Rad;
            Vector3 nuevaPos = pivote.position + new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * radio;
            nuevaPos.y = posOrigen.y;
            colliderAtaque.transform.position = nuevaPos;

            Vector3 mirar = nuevaPos - pivote.position;
            if (mirar != Vector3.zero)
                colliderAtaque.transform.rotation = Quaternion.LookRotation(mirar.normalized);

            yield return null;
        }
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
