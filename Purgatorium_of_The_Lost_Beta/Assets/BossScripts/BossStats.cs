using System;
using UnityEngine;

/// <summary>
/// Stats del Boss. Funciona igual que PlayerStats.
/// Asignado y usado por BossHealth.
/// </summary>
[Serializable]
public class BossStats
{
    [Header("Vida")]
    public float baseHealth    = 100f;
    public float maxHealth     = 100f;
    public float currentHealth;

    [Header("Defensa")]
    public float defense       = 0f;

    [Header("Ataque")]
    public float baseDamage    = 5f;
    public float attackDamage  = 5f;
    public float critChance    = 0.1f;
    public float critMultiplier = 2f;

    public BossStats()
    {
        baseHealth     = 100f;
        maxHealth      = baseHealth;
        currentHealth  = maxHealth;
        defense        = 0f;
        baseDamage     = 5f;
        attackDamage   = baseDamage;
        critChance     = 0.1f;
        critMultiplier = 2f;
    }

    public BossStats(BossStats other)
    {
        baseHealth     = other.baseHealth;
        maxHealth      = other.maxHealth;
        currentHealth  = other.currentHealth;
        defense        = other.defense;
        baseDamage     = other.baseDamage;
        attackDamage   = other.attackDamage;
        critChance     = other.critChance;
        critMultiplier = other.critMultiplier;
    }

    /// <summary>Calcula el danio final tras restar la defensa.</summary>
    public float CalcularDanoRecibido(float cantidad)
    {
        return Mathf.Max(cantidad - defense, 0f);
    }

    /// <summary>Calcula el danio de ataque, aplicando critico si procede.</summary>
    public float CalcularDanoAtaque()
    {
        float dmg = attackDamage;
        if (UnityEngine.Random.value <= critChance)
            dmg *= critMultiplier;
        return dmg;
    }

    /// <summary>Recarga la vida al maximo.</summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}