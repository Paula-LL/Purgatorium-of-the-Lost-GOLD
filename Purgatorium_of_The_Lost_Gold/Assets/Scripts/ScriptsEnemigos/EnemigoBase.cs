using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemigoBase : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats = new EnemyStats();

    [Header("Follow Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float detectionRange = 10f;

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenAttacks = 3f;

    [Header("Hit Feedback")]
    [SerializeField] private float knockbackDistance = 1.5f;
    [SerializeField] private float blinkDuration = 0.15f;

    private Transform player;
    private bool playerInRange = false;
    private bool playerDetected = false;
    private float timeInRange = 0f;
    private float lastDamageTime = 0f;
    private Animator animator;

    private Coroutine hitEffectCoroutine;
    private Renderer[] renderers;
    private Color[] coloresOriginales;

    public static List<EnemigoBase> enemyList = new List<EnemigoBase>();

    void Awake()
    {
        enemyList.Add(this);
    }

    void Start()
    {
        stats.ResetHealth();
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (stats.currentHealth <= 0) return;

        CheckPlayerDetection();

        float speed = 0f;
        if (playerDetected)
            speed = FollowPlayer();

        if (playerInRange)
            ProcessDamage();

        if (transform.position.y < 10)
        {
            Die();
        }

        UpdateAnimation(speed);
    }

    void CheckPlayerDetection()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);
        playerDetected = distance <= detectionRange;
    }

    float FollowPlayer()
    {
        if (player == null) return 0f;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        if (!playerInRange && distance > 0.1f)
        {
            Vector3 move = direction.normalized * stats.moveSpeed * Time.deltaTime;
            transform.position += move;
            transform.forward = direction.normalized;
            return move.magnitude / Time.deltaTime;
        }

        return 0f;
    }

    void ProcessDamage()
    {
        timeInRange += Time.deltaTime;

        if (timeInRange >= lastDamageTime + timeBetweenAttacks)
        {
            DealDamageToPlayer();
            lastDamageTime = timeInRange;
        }
    }

    void DealDamageToPlayer()
    {
        if (player == null) return;
        Player_controller playerScript = player.GetComponent<Player_controller>();
        if (playerScript == null) return;

        float damage = stats.CalcularDanoAtaque();
        playerScript.TakeDamage(damage);
    }

    void UpdateAnimation(float speed)
    {
        if (animator == null) return;
        animator.SetFloat("Speed", speed);
        animator.SetBool("isAttacking", playerInRange);
    }

    public void TakeDamage(float amount)
    {
        float finalDamage = stats.CalcularDanoRecibido(amount);
        stats.currentHealth -= (float)finalDamage;
        Debug.Log($"{gameObject.name} recibio {finalDamage} dano. Vida: {stats.currentHealth}/{stats.maxHealth}");
        
        if (hitEffectCoroutine != null) StopCoroutine(hitEffectCoroutine);
        hitEffectCoroutine = StartCoroutine(EfectoDano());

        if (stats.currentHealth <= 0) {
            stats.currentHealth = 0;
            Die();
        }  
    }

    private System.Collections.IEnumerator EfectoDano()
    {
        if (renderers == null) {
            renderers = GetComponentsInChildren<Renderer>();
            coloresOriginales = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++) {
                if (renderers[i].material.HasProperty("_Color")) {
                    coloresOriginales[i] = renderers[i].material.color;
                }
            }
        }

        Vector3 retroceso = -transform.forward * knockbackDistance;
        if (player != null) {
            retroceso = (transform.position - player.position).normalized * knockbackDistance;
            retroceso.y = 0;
        }
        
        float duracion = blinkDuration;
        float elapsed = 0f;
        Vector3 posInicial = transform.position;
        Vector3 posFinal = transform.position + retroceso;
        
        for (int i = 0; i < renderers.Length; i++) {
            if (renderers[i].material.HasProperty("_Color")) {
                renderers[i].material.color = Color.red;
            }
        }
        
        while (elapsed < duracion) {
            transform.position = Vector3.Lerp(posInicial, posFinal, elapsed / duracion);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        for (int i = 0; i < renderers.Length; i++) {
            if (renderers[i].material.HasProperty("_Color")) {
                renderers[i].material.color = coloresOriginales[i];
            }
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        enemyList.Remove(this);
        EstadisticasJuego.RegistrarEnemigoCaido();
        
        if (DungeonGenerator.s.enemigosRestantesEnSala == 1)
        {
            DungeonGenerator.s.SpawnCardInTheRoom(GetComponentInParent<Room>().GetActualRoom());
        }
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            timeInRange = 0f;
            lastDamageTime = timeBetweenAttacks;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            timeInRange = 0f;
        }
    }
    
}