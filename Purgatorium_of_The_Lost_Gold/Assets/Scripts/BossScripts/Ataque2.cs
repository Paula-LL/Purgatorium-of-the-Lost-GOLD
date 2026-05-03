using System.Collections;
using UnityEngine;

public class Ataque2 : MonoBehaviour
{
    [Header("Deteccion del Jugador")]
    [SerializeField] private string etiquetaJugador = "Player";
    [SerializeField] private float  rangoDeteccion  = 20f;

    [Header("Dash")]
    [SerializeField] private float distanciaRetroceso = 3f;
    [SerializeField] private float duracionRetroceso  = 0.5f;
    [SerializeField] private float duracionAviso      = 1f;
    [SerializeField] private float velocidadDash      = 14f;
    [SerializeField] private float tiempoEsquive      = 1.5f;
    [SerializeField] private float duracionRetorno    = 1f;
    [SerializeField] private float tiempoEntreAtaques = 3f;

    [Header("Collider de Daño")]
    [SerializeField] private Collider colliderDash;
    [SerializeField] private float daño = 1.5f;

    private Transform      jugador;
    private bool           cicloEnCurso = false;
    private bool           dashActivo   = false;
    private bool           _yaGolpeado  = false;
    private PatrolMovement _patrulla;
    public  GameObject     boss;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        if (boss != null) _patrulla = boss.GetComponent<PatrolMovement>();
        if (colliderDash != null) colliderDash.enabled = false;
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
            && PatrolMovement.TurnoAtaque == 2)
            StartCoroutine(CicloAtaque());
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso = true;
        _yaGolpeado  = false;
        PatrolMovement.HayAtaqueActivo = true;

        Vector3 posInicial = boss.transform.position;
        Vector3 posJugador = jugador.position;
        Vector3 direccion  = posJugador - posInicial;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.001f) direccion.Normalize();

        if (direccion != Vector3.zero)
            boss.transform.rotation = Quaternion.LookRotation(direccion);

        if (_patrulla != null) _patrulla.enabled = false;

        Vector3 posRetroceso = posInicial - direccion * distanciaRetroceso;
        float t = 0f;
        animator.SetTrigger("isDashingBack");
        while (t < duracionRetroceso)
        {
            t += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(posInicial, posRetroceso, t / duracionRetroceso);
            yield return null;
        }

        yield return new WaitForSeconds(duracionAviso);

        if (colliderDash != null) colliderDash.enabled = true;
        dashActivo = true;
        animator.SetTrigger("isDashingFront");
        while (Vector3.Distance(boss.transform.position, posJugador) > 0.25f && dashActivo)
        {
            boss.transform.position = Vector3.MoveTowards(
                boss.transform.position, posJugador, velocidadDash * Time.deltaTime);
            yield return null;
        }

        dashActivo = false;
        if (colliderDash != null) colliderDash.enabled = false;

        if (!_yaGolpeado)
            yield return new WaitForSeconds(tiempoEsquive);

        Vector3 posActual = boss.transform.position;
        t = 0f;
        while (t < duracionRetorno)
        {
            t += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(posActual, posInicial, t / duracionRetorno);
            yield return null;
        }
        boss.transform.position = posInicial;
        animator.SetTrigger("DashEnd");
        if (_patrulla != null) _patrulla.enabled = true;

        PatrolMovement.HayAtaqueActivo = false;
        yield return new WaitForSeconds(tiempoEntreAtaques);

        PatrolMovement.TurnoAtaque = 3;
        cicloEnCurso = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!dashActivo || _yaGolpeado) return;
        if (other.CompareTag(etiquetaJugador))
        {
            Player_controller pc = other.GetComponent<Player_controller>();
            if (pc != null)
            {
                pc.TakeDamage(daño);
                _yaGolpeado = true;
                dashActivo  = false;
            }
        }
    }
}
