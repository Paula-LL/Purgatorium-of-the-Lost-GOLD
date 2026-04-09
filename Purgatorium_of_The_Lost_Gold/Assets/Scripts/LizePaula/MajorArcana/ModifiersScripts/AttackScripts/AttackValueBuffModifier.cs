using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Major Arcana/ Base /The Hanged Man (Normal)")]

public class AttackValueBuffModifier : AttackModifier
{
    public int dropChance; 
    public int hangedManAttackUp;
    public override void ApplyAttackModifier(Attack attack)
    {
        hangedManAttackUp = Random.Range(1, 11); 
        attack.attackDamage *= hangedManAttackUp;
    }
}
