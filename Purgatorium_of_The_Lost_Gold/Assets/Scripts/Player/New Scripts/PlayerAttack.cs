using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<AttackModifier> modifierAttackList = new List<AttackModifier>();

    private Player_controller player;

    void Start()
    {
        player = Player_controller.instance;
    }

    public void PerformAttack()
    {
        Attack attack = new Attack(player.currentPlayerStats);
        ApplyAttackModifiers(attack);

        float finalDamage = attack.attackDamage;

        if (UnityEngine.Random.value <= player.currentPlayerStats.critChance)
            finalDamage *= player.currentPlayerStats.critMultiplier;

        attack.attackDamage = finalDamage;

        AttackEnemies(attack);
    }

    void AttackEnemies(Attack attack)
    {
        Vector3 center = transform.position + transform.forward * attack.attackDistance;
        Collider[] hitColliders = Physics.OverlapSphere(center, attack.attackRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemigoDist dist = col.GetComponent<EnemigoDist>();
                EnemigoBase baseEnemy = col.GetComponent<EnemigoBase>();

                if (dist != null)
                {
                    dist.TakeDamage(attack.attackDamage);
                    EstadisticasJuego.RegistrarDanoHecho(attack.attackDamage);
                }
                else if (baseEnemy != null)
                {
                    baseEnemy.TakeDamage(attack.attackDamage);
                    EstadisticasJuego.RegistrarDanoHecho(attack.attackDamage);
                }
            }

            if (col.CompareTag("Boss"))
            {
                BossHealth boss = col.GetComponent<BossHealth>();
                if (boss != null)
                {
                    boss.RecibirDanio(attack.attackDamage);
                    EstadisticasJuego.RegistrarDanoHecho(attack.attackDamage);
                }
            }
        }
    }

    internal void AddModifier(AttackModifier cardsBuff)
    {
        modifierAttackList.Add(cardsBuff);
    }

    void ApplyAttackModifiers(Attack a)
    {
        foreach (AttackModifier modifier in modifierAttackList)
            modifier.ApplyAttackModifier(a);
    }
}

[System.Serializable]
public class Attack
{
    public float attackDistance;
    public float attackRadius;
    public float attackDuration;
    public float attackDamage;

    public Attack(PlayerStats stats)
    {
        attackDistance = stats.attackDistance;
        attackRadius = stats.attackRadius;
        attackDuration = stats.attackDuration;
        attackDamage = stats.attackDamage;
    }
}