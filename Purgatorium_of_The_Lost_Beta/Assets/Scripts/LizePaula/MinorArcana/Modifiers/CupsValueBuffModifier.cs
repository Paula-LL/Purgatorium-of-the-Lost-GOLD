using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minor Arcana/Cups")]


public class CupsValueBuffModifier : CupsModifier
{
    public int cupsHealing;
    public override void ApplyCupsCardModifier(Player_controller player)
    {
        player.HealHealth(cupsHealing);
    }
}
