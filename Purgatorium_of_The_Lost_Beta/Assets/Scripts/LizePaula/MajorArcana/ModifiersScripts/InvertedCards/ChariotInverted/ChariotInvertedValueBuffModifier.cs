using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Major Arcana/ Inverted /The Chariot (Inverted)")]

public class ChariotInvertedlValueBuffModifier : ChariotInvertedModifier
{
    public int dropChance;
    public float charriotInvertedSpeedDown;

    public override void ApplyChariotInvertedCardModifier(PlayerStats.Movement movement)
    {
        movement.moveSpeed -= (movement.moveSpeed * charriotInvertedSpeedDown) / 100;
    }

}
