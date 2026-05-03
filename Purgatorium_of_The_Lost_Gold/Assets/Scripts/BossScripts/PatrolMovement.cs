using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [Header("Puntos de Patrulla")]
    public Transform[] puntos = new Transform[4];

    [Header("Configuración de Movimiento")]
    public float velocidad = 3f;
    public float distanciaLlegada = 0.1f;
    public bool mirarHaciaElMovimiento = true;
    public float tiempoEsperaEnPunto = 0f;

    [Header("Comportamiento en Espera")]
    public bool  mirarAlPlayerEnEspera = true;
    public float velocidadGiroHaciaPlayer = 5f;
    public string etiquetaJugador = "Player";

    private int   _indicePuntoActual = 0;
    private float _temporizadorEspera = 0f;
    private bool  _estaEsperando = false;
    private Transform _jugador;

    public bool EstaEnMovimiento { get; private set; } = false;
    public static bool HayAtaqueActivo = false;
    public static int  TurnoAtaque     = 1;   // 1 = Ataque1, 2 = Ataque2
    private Animator animator;
    public GameObject boss;

    private void Start()
    {
        animator = GetComponent<Animator>();    
        if (puntos == null || puntos.Length < 2)
        {
            Debug.LogWarning($"[PatrolMovement] {gameObject.name}: necesitas al menos 2 puntos asignados.", this);
            enabled = false;
            return;
        }

        GameObject obj = GameObject.FindGameObjectWithTag(etiquetaJugador);
        if (obj != null) _jugador = obj.transform;

        if (puntos[0] != null)
            transform.position = puntos[0].position;

        
    }

    private void Update()
    {
        if (HayAtaqueActivo)
        {
            EstaEnMovimiento = false;
            return;
        }

        if (_estaEsperando)
        {
            EstaEnMovimiento = false;
            _temporizadorEspera -= Time.deltaTime;
            if (_temporizadorEspera <= 0f)
                _estaEsperando = false;

            MirarAlPlayer();
            return;
        }

        EstaEnMovimiento = true;
        animator.SetFloat("Speed", 1);
        MoverHaciaElSiguiente();
    }

    private void MoverHaciaElSiguiente()
    {
       
        Transform objetivo = ObtenerPuntoValido(_indicePuntoActual);
        if (objetivo == null && boss.GetComponent<BossHealth>().EstaMuerto == true) return;
        if(boss.GetComponent<BossHealth>().EstaMuerto == false)
        {
            transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime

        );

            if (mirarHaciaElMovimiento)
            {
                Vector3 direccion = (objetivo.position - transform.position);
                direccion.y = 0f;
                if (direccion.sqrMagnitude > 0.001f)
                {
                    Quaternion rotacion = Quaternion.LookRotation(direccion);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, 10f * Time.deltaTime);
                }
            }

            if (Vector3.Distance(transform.position, objetivo.position) <= distanciaLlegada)
            {
                animator.SetFloat("Speed", 0);
                transform.position = objetivo.position;
                AvanzarAlSiguientePunto();
            }
        }
        
    }

    private void AvanzarAlSiguientePunto()
    {
        _indicePuntoActual = (_indicePuntoActual + 1) % ObtenerCantidadPuntos();
        EstaEnMovimiento = false;

        if (tiempoEsperaEnPunto > 0f)
        {
            
            _temporizadorEspera = tiempoEsperaEnPunto;
            _estaEsperando = true;
            animator.SetFloat("Speed", 0);

        }
    }

    private int ObtenerCantidadPuntos()
    {
        int cantidad = 0;
        foreach (var punto in puntos)
            if (punto != null) cantidad++;
        return Mathf.Max(cantidad, 1);
    }

    private Transform ObtenerPuntoValido(int indice)
    {
        for (int i = 0; i < puntos.Length; i++)
        {
            int idx = (indice + i) % puntos.Length;
            if (puntos[idx] != null)
                return puntos[idx];
        }
        return null;
    }
    private void MirarAlPlayer()
    {
        if (!mirarAlPlayerEnEspera || _jugador == null || HayAtaqueActivo) return;

        Vector3 dir = _jugador.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion rotObjetivo = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotObjetivo, velocidadGiroHaciaPlayer * Time.deltaTime);
    }
}
