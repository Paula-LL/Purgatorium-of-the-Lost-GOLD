using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffCards : MonoBehaviour
{
    public virtual void PickUpCard(Collider collision)
    {
        Destroy(gameObject);
        //collision.GetComponent<PlayerAttack>().AddModifier(cardsBuff);
        //collision.GetComponent<Player_controller>().AddModifier(cardsBuff2);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickUpCard(collision);
        }
    }
}
