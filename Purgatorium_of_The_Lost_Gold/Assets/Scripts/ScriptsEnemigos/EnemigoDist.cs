using UnityEngine;

public class EnemigoDist : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats = new EnemyStats();

    [Header("Follow Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float detectionRange = 15f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown = 2f;

    [Header("Hit Feedback")]
    [SerializeField] private float knockbackDistance = 1.5f;
    [SerializeField] private float blinkDuration = 0.15f;

    private Transform player;
    private float nextShootTime;
    private bool playerDetected = false;
    private Animator animator;

    private Coroutine hitEffectCoroutine;
    private Renderer[] renderers;
    private Color[] coloresOriginales;

    void Start()
    {
        stats.ResetHealth();
        animator = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        playerDetected = distance <= detectionRange;

        if (!playerDetected) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        transform.forward = dir.normalized;

        if (distance > stopDistance)
            transform.position += transform.forward * stats.moveSpeed * Time.deltaTime;
            animator.SetFloat("Speed", 1);

        if (distance <= stopDistance && Time.time >= nextShootTime)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("isAttacking", true);
            SpawnProjectile();
            nextShootTime = Time.time + shootCooldown;
        }
        
    }

    void SpawnProjectile()
    {
        if (projectilePrefab == null || shootPoint == null) return;
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = stats.CalcularDanoRecibido(damage);
        stats.currentHealth -= (float)finalDamage;
        Debug.Log($"{gameObject.name} recibio {finalDamage} dano. Vida: {stats.currentHealth}/{stats.maxHealth}");

        if (hitEffectCoroutine != null) StopCoroutine(hitEffectCoroutine);
        hitEffectCoroutine = StartCoroutine(EfectoDano());

        if (stats.currentHealth <= 0 || transform.position.y< 10) 
        {
            animator.SetBool("isDead", true );
            EstadisticasJuego.RegistrarEnemigoCaido();
            Invoke("DestruirEnemigo", 3f);
            if (DungeonGenerator.s.enemigosRestantesEnSala == 1)
            {
                DungeonGenerator.s.SpawnCardInTheRoom(GetComponentInParent<Room>().GetActualRoom());
            }
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
    public void DestruirEnemigo()
    {
        Destroy(gameObject);    
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}