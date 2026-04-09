using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Player Attack de Paula
/// </summary>
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerAttack : MonoBehaviour
{
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackCooldown = 0f;

    private Renderer rend;
    private Color originalColor;
    private LineRenderer lineRenderer;
    public List<AttackModifier> modifierAttackList = new List<AttackModifier>();

    private Player_controller player;
    public Animator animator;

    void Start()
    {
        player = Player_controller.instance;

        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 31;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && !isAttacking && attackCooldown <= 0f)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            float duration = player.currentPlayerStats.attackDuration;
            attackTimer = duration;

            attackCooldown = 1f / player.currentPlayerStats.attackSpeed;

            rend.material.color = Color.red;

            Attack attack = new Attack(player.currentPlayerStats);
            ApplyAttackModifiers(attack);

            float finalDamage = attack.attackDamage;

            if (UnityEngine.Random.value <= player.currentPlayerStats.critChance)
            {
                finalDamage *= player.currentPlayerStats.critMultiplier;
            }

            attack.attackDamage = finalDamage;

            AttackEnemies(attack);
            DrawAttackCircle(attack);
            lineRenderer.enabled = true;
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
                rend.material.color = originalColor;
                lineRenderer.enabled = false;
            }
        }
    }

    void AttackEnemies(Attack attack)
    {
        Vector3 center = transform.position + transform.forward * attack.attackDistance;
        Collider[] hitColliders = Physics.OverlapSphere(center, attack.attackRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemigoDist enemigoDist = col.GetComponent<EnemigoDist>();
                EnemigoBase enemigoBase = col.GetComponent<EnemigoBase>();

                if (enemigoDist != null)
                {
                    enemigoDist.TakeDamage(attack.attackDamage);
                    EstadisticasJuego.RegistrarDanoHecho(attack.attackDamage);
                }
                else if (enemigoBase != null)
                {
                    enemigoBase.TakeDamage(attack.attackDamage);
                    EstadisticasJuego.RegistrarDanoHecho(attack.attackDamage);
                }
            }

            if (col.CompareTag("Boss"))
            {
                BossHealth bossHit = col.GetComponent<BossHealth>();
                if (bossHit != null)
                {
                    bossHit.RecibirDanio(attack.attackDamage);
                    EstadisticasJuego.RegistrarDanoHecho(attack.attackDamage);
                }
            }
        }
    }

    void DrawAttackCircle(Attack attack)
    {
        Vector3 center = transform.position + transform.forward * attack.attackDistance;
        for (int i = 0; i <= attack.circleSegments; i++)
        {
            float angle = i * Mathf.PI * 2 / attack.circleSegments;
            float x = Mathf.Cos(angle) * attack.attackRadius;
            float z = Mathf.Sin(angle) * attack.attackRadius;
            Vector3 pos = center + new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
        }
    }

    internal void AddModifier(AttackModifier cardsBuff)
    {
        modifierAttackList.Add(cardsBuff);
    }

    void ApplyAttackModifiers(Attack a)
    {
        foreach (AttackModifier modifier in modifierAttackList)
        {
            modifier.ApplyAttackModifier(a);
        }
    }
}

[System.Serializable]
public class Attack
{
    public float attackDistance;
    public float attackRadius;
    public float attackDuration;
    public float attackDamage;
    public int circleSegments = 30;

    public Attack(PlayerStats stats)
    {
        attackDistance = stats.attackDistance;
        attackRadius = stats.attackRadius;
        attackDuration = stats.attackDuration;
        attackDamage = stats.attackDamage;
    }
}