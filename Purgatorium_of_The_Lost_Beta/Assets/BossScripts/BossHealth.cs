using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Stats")]
    public BossStats stats = new BossStats();

    public float VidaActual  => stats.currentHealth;
    public float VidaMaxima  => stats.maxHealth;
    public bool  EstaMuerto  => stats.currentHealth <= 0f;

    void Start()
    {
        stats.ResetHealth();
    }

    public void RecibirDanio(float cantidad)
    {
        if (EstaMuerto) return;

        float finalDamage = stats.CalcularDanoRecibido(cantidad);
        stats.currentHealth -= finalDamage;
        stats.currentHealth = Mathf.Max(stats.currentHealth, 0f);

        Debug.Log($"[BossHealth] {gameObject.name} recibio {finalDamage} dano. Vida: {stats.currentHealth}/{stats.maxHealth}");

        if (stats.currentHealth <= 0f)
            Morir();
    }

    private void Morir()
    {
        Debug.Log($"[BossHealth] {gameObject.name} ha muerto.");
        EstadisticasJuego.RegistrarBossCaido();
        Destroy(gameObject);
    }
}