using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsMinorArcanaBuffCards : BuffCards
{
    [SerializeField]
    CupsModifier cupsHealing;

    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);

        if (Player_controller.instance.currentPlayerStats.currentHealth < Player_controller.instance.currentPlayerStats.maxHealth)
        {
            cupsHealing.ApplyCupsCardModifier(Player_controller.instance);
        }
        Debug.Log("Picked up: Cups");
    }
}
