using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoversNormalBuffCard : BuffCards
{
    [SerializeField]
    LoversNormalModifier loversNormalBuff;

    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);

        if (Player_controller.instance.currentPlayerStats.currentHealth == Player_controller.instance.currentPlayerStats.maxHealth)
        {
            collision.GetComponent<Player_controller>().AddModifier(loversNormalBuff, false);
            Player_controller.instance.SetCurrentHealthToMax();
        }
        else
        {
            collision.GetComponent<Player_controller>().AddModifier(loversNormalBuff);
            //Hacer método incrementar vida actual en PlayerController
            Player_controller.instance.SetCurrentHealth(Player_controller.instance.currentPlayerStats.currentHealth * 2);
        }
        Debug.Log("Picked up: The Lovers");
    }
}
