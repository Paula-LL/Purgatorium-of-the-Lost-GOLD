using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 direction;

    void Start()
    {
        // Determina dirección hacia la última posición conocida del player
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);

        if (playerObj != null)
        {
            Vector3 targetPosition = playerObj.transform.position;
            direction = (targetPosition - transform.position).normalized;
        }
        else
        {
            direction = transform.forward; 
        }

        // Rotar el proyectil hacia la dirección
        transform.forward = direction;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Si choca con el player → destruir y aplicar daño
        if (other.CompareTag(playerTag))
        {
            // Aplicar daño 
            other.GetComponent<Player_controller>()?.TakeDamage(1f);
            EstadisticasJuego.RegistrarDanoRecibido(1f);

            Destroy(gameObject);
        }
        else if (!other.isTrigger) // Cualquier obstáculo sólido
        {
            Destroy(gameObject);
        }
    }
}
