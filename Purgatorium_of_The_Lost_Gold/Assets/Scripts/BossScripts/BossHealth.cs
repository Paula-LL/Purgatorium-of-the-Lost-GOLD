using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Stats")]
    public BossStats stats = new BossStats();

    public float VidaActual  => stats.currentHealth;
    public float VidaMaxima  => stats.maxHealth;
    public bool  EstaMuerto  => stats.currentHealth <= 0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();    
        stats.ResetHealth();
    }

    public void RecibirDanio(float cantidad)
    {
        if (EstaMuerto) return;

        float finalDamage = stats.CalcularDanoRecibido(cantidad);
        stats.currentHealth -= finalDamage;
        ShowDamage(finalDamage);
        stats.currentHealth = Mathf.Max(stats.currentHealth, 0f);

        Debug.Log($"[BossHealth] {gameObject.name} recibio {finalDamage} dano. Vida: {stats.currentHealth}/{stats.maxHealth}");

        if (stats.currentHealth <= 0f)
            animator.SetBool("isDeath", true);
            Morir();
    }
    public void ShowDamage(float amount)
    {
        SpawnDamagePopups.Instance.DamageDone(amount, transform.position, false);
    }
    private void Morir()
    {
        GuardarEstadisticas.Instance.estadisticasAGuardar = null;
        Debug.Log($"[BossHealth] {gameObject.name} ha muerto.");
        EstadisticasJuego.RegistrarBossCaido();
    }
}