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

    private Transform player;
    private float nextShootTime;
    private bool playerDetected = false;

    void Start()
    {
        stats.ResetHealth();

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

        if (distance <= stopDistance && Time.time >= nextShootTime)
        {
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
        stats.currentHealth -= finalDamage;
        Debug.Log($"{gameObject.name} recibio {finalDamage} dano. Vida: {stats.currentHealth}/{stats.maxHealth}");
        if (stats.currentHealth <= 0)
        {
            EstadisticasJuego.RegistrarEnemigoCaido();
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}