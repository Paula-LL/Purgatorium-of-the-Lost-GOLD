using System.Collections;
using UnityEngine;

public class Ataque3 : MonoBehaviour
{
    [Header("Deteccion del Jugador")]
    [SerializeField] private string etiquetaJugador = "Player";
    [SerializeField] private float  rangoDeteccion  = 20f;

    [Header("Proyectil (Garfio)")]
    [SerializeField] private Collider colliderGarfio;
    [SerializeField] private float distanciaMaxima    = 8f;
    [SerializeField] private float velocidadProyectil = 7f;

    [Header("Atraccion")]
    [SerializeField] private float tiempoAtraccion   = 0.6f;
    [SerializeField] private float distanciaParada   = 1.5f;

    [Header("Tiempos (segundos)")]
    [SerializeField] private float tiempoAviso        = 0.8f;
    [SerializeField] private float tiempoEntreAtaques = 3f;

    private Transform      jugador;
    private bool           cicloEnCurso     = false;
    private bool           garfioLanzado    = false;
    private bool           jugadorCapturado = false;
    private PatrolMovement _patrulla;
    private Ataque1        _ataque1;
    public  GameObject     boss;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        if (boss != null)
        {
            _patrulla = boss.GetComponent<PatrolMovement>();
            _ataque1  = boss.GetComponentInChildren<Ataque1>();
        }

        if (colliderGarfio != null) colliderGarfio.enabled = false;
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

        float distancia      = Vector3.Distance(boss.transform.position, jugador.position);
        bool  bossMoviendose = _patrulla != null && _patrulla.EstaEnMovimiento;

        if (distancia <= rangoDeteccion && !cicloEnCurso
            && !bossMoviendose && !PatrolMovement.HayAtaqueActivo
            && PatrolMovement.TurnoAtaque == 3)
            StartCoroutine(CicloAtaque());
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso     = true;
        jugadorCapturado = false;
        PatrolMovement.HayAtaqueActivo = true;

        if (colliderGarfio == null)
        {
            yield return new WaitForSeconds(tiempoEntreAtaques);
            PatrolMovement.HayAtaqueActivo = false;
            PatrolMovement.TurnoAtaque = 1;
            cicloEnCurso = false;
            yield break;
        }

        yield return new WaitForSeconds(tiempoAviso);

        // Boss mira al jugador antes de lanzar
        Vector3 dirBoss = (jugador.position - boss.transform.position);
        dirBoss.y = 0f;
        if (dirBoss.sqrMagnitude > 0.001f)
            boss.transform.rotation = Quaternion.LookRotation(dirBoss.normalized);

        Vector3    posLocalInicial = colliderGarfio.transform.localPosition;
        Quaternion rotLocalInicial = colliderGarfio.transform.localRotation;
        Vector3    destino         = boss.transform.position + boss.transform.forward * distanciaMaxima;

        colliderGarfio.enabled = true;
        garfioLanzado = true;

        while (Vector3.Distance(colliderGarfio.transform.position, destino) > 0.1f && !jugadorCapturado)
        {
            colliderGarfio.transform.position = Vector3.MoveTowards(
                colliderGarfio.transform.position, destino, velocidadProyectil * Time.deltaTime);
            yield return null;
        }

        garfioLanzado = false;
        colliderGarfio.enabled = false;

        if (jugadorCapturado && jugador != null)
        {
            animator.SetBool("isGrabAttack", true);
            Vector3 posJugadorInicio = jugador.position;
            Vector3 posDetencion     = boss.transform.position + boss.transform.forward * distanciaParada;
            float   t = 0f;
            while (t < tiempoAtraccion)
            {
                t += Time.deltaTime;
                jugador.position = Vector3.Lerp(posJugadorInicio, posDetencion, t / tiempoAtraccion);
                yield return null;
            }

            colliderGarfio.transform.localPosition = posLocalInicial;
            colliderGarfio.transform.localRotation = rotLocalInicial;

            PatrolMovement.HayAtaqueActivo = false;
            animator.SetBool("isGrabAttack", false);
            cicloEnCurso = false;
            if (_ataque1 != null) _ataque1.ForzarCombo();
            yield break;
        }

        colliderGarfio.transform.localPosition = posLocalInicial;
        colliderGarfio.transform.localRotation = rotLocalInicial;

        
        PatrolMovement.HayAtaqueActivo = false;
        yield return new WaitForSeconds(tiempoEntreAtaques);

        PatrolMovement.TurnoAtaque = 1;
        cicloEnCurso = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!garfioLanzado) return;
        if (other.CompareTag(etiquetaJugador))
            jugadorCapturado = true;
    }
}
