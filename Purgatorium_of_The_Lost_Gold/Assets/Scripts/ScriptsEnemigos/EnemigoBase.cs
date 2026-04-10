using System.Collections.Generic;
using UnityEngine;

public class EnemigoBase : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats = new EnemyStats();

    [Header("Follow Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float detectionRange = 10f;

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenAttacks = 3f;

    private Transform player;
    private bool playerInRange = false;
    private bool playerDetected = false;
    private float timeInRange = 0f;
    private float lastDamageTime = 0f;
    private Animator animator;

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
        stats.currentHealth -= finalDamage;
        Debug.Log($"{gameObject.name} recibio {finalDamage} dano. Vida: {stats.currentHealth}/{stats.maxHealth}");
        if (stats.currentHealth <= 0)
            Die();
    }

    void Die()
    {
        enemyList.Remove(this);
        EstadisticasJuego.RegistrarEnemigoCaido();
        if (animator != null)
            animator.SetTrigger("Die");

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