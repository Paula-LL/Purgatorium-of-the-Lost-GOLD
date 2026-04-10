using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Major Arcana/ Base /The Chariot (Normal)")]

public class ChariotNormalValueBuffModifier : ChariotNormalModifier
{
    public int dropChance; 
    public float charriotNormalSpeedUp;
    public override void ApplyChariotNormalCardModifier(PlayerStats.Movement movement)
    {
        movement.moveSpeed *= charriotNormalSpeedUp;
        movement.dashSpeed *= charriotNormalSpeedUp;

    }
}
