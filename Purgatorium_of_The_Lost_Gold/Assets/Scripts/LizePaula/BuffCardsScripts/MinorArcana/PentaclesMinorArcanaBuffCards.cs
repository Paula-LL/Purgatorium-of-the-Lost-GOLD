using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaclesMinorArcanaBuffCards : BuffCards
{
    [SerializeField]
    PentacleModifier pentaclesCoins;

    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);

        collision.GetComponent<InventoryController>().AddModifier(pentaclesCoins);

        Debug.Log("Picked up: Pentacles");
    }
}
