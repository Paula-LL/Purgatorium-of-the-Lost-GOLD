using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoversInvertedBuffCard : BuffCards
{
    [SerializeField]
    LoversInvertedModifier loversInvertedDebuff;

    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);

        if (Player_controller.instance.currentPlayerStats.currentHealth == Player_controller.instance.currentPlayerStats.maxHealth)
        {
            collision.GetComponent<Player_controller>().AddModifier(loversInvertedDebuff, false);
            Player_controller.instance.SetCurrentHealthToMax();
        }
        else
        {
            collision.GetComponent<Player_controller>().AddModifier(loversInvertedDebuff);
            Player_controller.instance.SetCurrentHealth(Player_controller.instance.currentPlayerStats.currentHealth * 2);
        }
    }
}
