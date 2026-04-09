using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuffCards : BuffCards
{
    [SerializeField]
    AttackModifier attackModifier;
    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);
        collision.GetComponent<PlayerAttack>().AddModifier(attackModifier);
    }
}
