using System;
using UnityEngine;

/// <summary>
/// Stats de un enemigo normal. Funciona igual que PlayerStats.
/// Asignado y usado por EnemigoBase y EnemigoDist.
/// </summary>
[Serializable]
public class EnemyStats
{
    [Header("Vida")]
    public int baseHealth = 3;
    public int maxHealth = 3;
    public float currentHealth;

    [Header("Defensa")]
    public float defense      = 0f;

    [Header("Ataque")]
    public float baseDamage   = 1f;
    public float attackDamage = 1f;
    public float critChance   = 0f;
    public float critMultiplier = 2f;

    [Header("Movimiento")]
    public float moveSpeed    = 3f;

    public EnemyStats()
    {
        baseHealth    = 3;
        maxHealth     = baseHealth;
        currentHealth = maxHealth;
        defense       = 0f;
        baseDamage    = 1f;
        attackDamage  = baseDamage;
        critChance    = 0f;
        critMultiplier = 2f;
        moveSpeed     = 3f;
    }

    public EnemyStats(EnemyStats other)
    {
        baseHealth     = other.baseHealth;
        maxHealth      = other.maxHealth;
        currentHealth  = other.currentHealth;
        defense        = other.defense;
        baseDamage     = other.baseDamage;
        attackDamage   = other.attackDamage;
        critChance     = other.critChance;
        critMultiplier = other.critMultiplier;
        moveSpeed      = other.moveSpeed;
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