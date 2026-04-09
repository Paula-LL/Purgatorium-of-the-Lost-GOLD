using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Major Arcana/ Inverted /The Lovers (Inverted)")]

public class LoversInvertedValueBuffModifier : LoversInvertedModifier
{
    public int healthDrop;
    public override void ApplyLoversInvertedCardModifier(PlayerStats health)
    {
        health.maxHealth -= (healthDrop * 50)/100;
    }

}