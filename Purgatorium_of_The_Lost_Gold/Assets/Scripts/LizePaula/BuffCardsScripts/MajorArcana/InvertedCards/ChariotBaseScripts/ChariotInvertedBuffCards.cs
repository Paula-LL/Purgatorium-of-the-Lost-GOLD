using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotInvertedBuffCards : BuffCards
{
    [SerializeField]
    ChariotInvertedModifier chariotInvertedModifier;
    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);
        collision.GetComponent<Player_controller>().AddModifier(chariotInvertedModifier);
    }
}
