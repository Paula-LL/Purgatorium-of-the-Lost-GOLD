using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotNormalBuffCards : BuffCards
{
    [SerializeField]
    ChariotNormalModifier chariotNormalModifier;
    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);
        collision.GetComponent<Player_controller>().AddModifier(chariotNormalModifier);
    }
}