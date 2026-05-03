using System.Collections;
using UnityEngine;

public class Ataque1 : MonoBehaviour
{
    [Header("Deteccion del Jugador")]
    [SerializeField] private string etiquetaJugador = "Player";
    [SerializeField] private float  rangoDeteccion  = 20f;

    [Header("Collider de Ataque")]
    [SerializeField] private Collider colliderAtaque;

    [Header("Combo - Tiempos (segundos)")]
    [SerializeField] private float tiempoAviso        = 1f;
    [SerializeField] private float tiempoActivoGolpe  = 0.3f;
    [SerializeField] private float tiempoEntreGolpes  = 0.5f;
    [SerializeField] private float tiempoEntreAtaques = 3f;

    [Header("Daño")]
    [SerializeField] private float daño = 1f;

    private Transform      jugador;
    private bool           cicloEnCurso = false;
    private bool           golpeActivo  = false;
    private PatrolMovement _patrulla;
    public  GameObject     boss;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();    
        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) jugador = obj.transform;

        if (boss != null) _patrulla = boss.GetComponent<PatrolMovement>();
        if (colliderAtaque != null) colliderAtaque.enabled = false;
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

        float distancia      = Vector3.Distance(transform.position, jugador.position);
        bool  bossMoviendose = _patrulla != null && _patrulla.EstaEnMovimiento;

        if (distancia <= rangoDeteccion && !cicloEnCurso
            && !bossMoviendose && !PatrolMovement.HayAtaqueActivo
            && PatrolMovement.TurnoAtaque == 1)
            StartCoroutine(CicloAtaque());
    }

    private IEnumerator CicloAtaque()
    {
        cicloEnCurso = true;
        PatrolMovement.HayAtaqueActivo = true;

        yield return new WaitForSeconds(tiempoAviso);
        animator.SetBool("isAttackingBasic", true);
        for (int i = 0; i < 3; i++)
        {
            
            golpeActivo = true;
            if (colliderAtaque != null) colliderAtaque.enabled = true;

            yield return new WaitForSeconds(tiempoActivoGolpe);

            golpeActivo = false;
            if (colliderAtaque != null) colliderAtaque.enabled = false;

            if (i < 2) yield return new WaitForSeconds(tiempoEntreGolpes);
           
        }
        animator.SetBool("isAttackingBasic", false);
        PatrolMovement.HayAtaqueActivo = false;
        yield return new WaitForSeconds(tiempoEntreAtaques);

        PatrolMovement.TurnoAtaque = 2;
        cicloEnCurso = false;
    }

    public void ForzarCombo()
    {
        if (!cicloEnCurso)
            StartCoroutine(CicloAtaque());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!golpeActivo) return;
        if (other.CompareTag(etiquetaJugador))
        {
            Player_controller pc = other.GetComponent<Player_controller>();
            if (pc != null) pc.TakeDamage(daño);
        }
    }
}
