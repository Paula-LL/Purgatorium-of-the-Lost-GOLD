using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Major Arcana/ Base /The Lovers (Normal)")]

public class LoversNormalValueBuffModifier : LoversNormalModifier
{
    public int extraHealth;
    public override void ApplyLoversNormalCardModifier(PlayerStats health)
    {
         //health.maxHealth += extraHealth;
        health.maxHealth += extraHealth;
    }

}
