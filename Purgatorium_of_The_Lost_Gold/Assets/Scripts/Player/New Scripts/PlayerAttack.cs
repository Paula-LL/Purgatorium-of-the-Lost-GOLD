using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<AttackModifier> modifierAttackList = new List<AttackModifier>();

    private Player_controller player;

    public float currentAttackDamage;

    [SerializeField] private AudioSource attacksound;

    void Start()
    {
        player = Player_controller.instance;
    }
    private void Update()
    {
        currentAttackDamage = player.currentPlayerStats.attackDamage;
    }
    public void PerformAttack()
    {
        Attack attack = new Attack(player.currentPlayerStats);
        attacksound.Play();
        ApplyAttackModifiers(attack);

        float finalDamage = attack.attackDamage;

        if (UnityEngine.Random.value <= player.currentPlayerStats.critChance)
            finalDamage *= player.currentPlayerStats.critMultiplier;

        currentAttackDamage = finalDamage;

        Debug.Log("Attack prepared. Damage = " + currentAttackDamage);
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
