using System.Collections;
using UnityEngine;

public class Ataque4 : MonoBehaviour
{
    [Header("Deteccion del Jugador")]
    [SerializeField] private string etiquetaJugador = "Player";
    [SerializeField] private float  rangoDeteccion  = 20f;

    [Header("Collider Rotatorio")]
    [SerializeField] private Transform pivoteCollider;
    [SerializeField] private Collider  colliderDanio;
    [SerializeField] private float velocidadRotacion = 120f;
    [SerializeField] private float numRotaciones     = 2f;
    [SerializeField] private float radioOrbita       = 2f;

    [Header("Movimiento del Boss")]
    [SerializeField] private float radioMovimientoBoss = 1f;
    [SerializeField] private float velocidadGiroBoss   = 300f;
    [SerializeField] private float duracionRetorno     = 0.5f;

    [Header("Tiempos (segundos)")]
    [SerializeField] private float tiempoAviso        = 0.8f;
    [SerializeField] private float tiempoQuieto       = 2f;
    [SerializeField] private float tiempoEntreAtaques = 3f;

    [Header("Daño")]
    [SerializeField] private float daño        = 1f;
    [SerializeField] private float cooldownDanio = 0.5f;

    private Transform      jugador;
    private bool           cicloEnCurso = false;
    private bool           _danioActivo = false;
    private float          _timerDanio  = 0f;
    private PatrolMovement _patrulla;
    public  GameObject     boss;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        if (boss != null) _patrulla = boss.GetComponent<PatrolMovement>();
        if (colliderDanio != null) colliderDanio.enabled = false;
    }

    void Update()
    {
        if (jugador == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
            if (obj != null) jugador = obj.transform;
            return;
        }

        if (boss != null && boss.GetComponent<BossHealth>().EstaMuerto) return;

        _timerDanio -= Time.deltaTime;

        float distancia      = Vector3.Distance(boss.transform.position, jugador.position);
        bool  bossMoviendose = _patrulla != null && _patrulla.EstaEnMovimiento;

        if (distancia <= rangoDeteccion && !cicloEnCurso
            && !bossMoviendose && !PatrolMovement.HayAtaqueActivo
            && PatrolMovement.TurnoAtaque == 4)
            StartCoroutine(CicloAtaque());
    }

    public void RecibirColision(Collider other)
    {
        if (!_danioActivo || _timerDanio > 0f) return;
        if (other.CompareTag(etiquetaJugador))
        {
            Player_controller pc = other.GetComponent<Player_controller>();
            if (pc != null)
            {
                pc.TakeDamage(daño);
                _timerDanio = cooldownDanio;
            }
        }
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso = true;
        PatrolMovement.HayAtaqueActivo = true;

        if (_patrulla != null) _patrulla.enabled = false;

        Vector3 posBase        = boss.transform.position;
        float   anguloOrbita   = 0f;
        float   anguloBoss     = boss.transform.eulerAngles.y;

        yield return new WaitForSeconds(tiempoAviso);

        _danioActivo = true;
        if (colliderDanio != null) colliderDanio.enabled = true;

        float gradosTotales = numRotaciones * 360f;
        float gradosGirados = 0f;

        while (gradosGirados < gradosTotales)
        {
            float pasoDelta    = velocidadRotacion * Time.deltaTime;
            anguloOrbita  += pasoDelta;
            gradosGirados += pasoDelta;

            // Boss gira sobre si mismo a su propia velocidad
            anguloBoss += velocidadGiroBoss * Time.deltaTime;
            boss.transform.rotation = Quaternion.Euler(0f, anguloBoss, 0f);

            // Boss se mueve en pequeno circulo alrededor de posBase
            float   rad    = anguloOrbita * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * radioMovimientoBoss;
            boss.transform.position = posBase + offset;

            // Collider orbita a mayor radio
            if (pivoteCollider != null)
            {
                pivoteCollider.position = posBase + offset.normalized * radioOrbita;
                Vector3 mirar = posBase - pivoteCollider.position;
                if (mirar != Vector3.zero)
                    pivoteCollider.rotation = Quaternion.LookRotation(mirar.normalized);
            }

            yield return null;
        }

        _danioActivo = false;
        if (colliderDanio != null) colliderDanio.enabled = false;

        // Retorno suave a posBase
        Vector3 posActual = boss.transform.position;
        float   t = 0f;
        while (t < duracionRetorno)
        {
            t += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(posActual, posBase, t / duracionRetorno);
            yield return null;
        }
        boss.transform.position = posBase;

        if (_patrulla != null) _patrulla.enabled = true;

        PatrolMovement.HayAtaqueActivo = false;

        yield return new WaitForSeconds(tiempoQuieto);
        yield return new WaitForSeconds(tiempoEntreAtaques);

        PatrolMovement.TurnoAtaque = 1;
        cicloEnCurso = false;
    }
}
