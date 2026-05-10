using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CardPickUp : MonoBehaviour
{
    public Animator anim;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.Play("marco|PlaneAction");
            anim.Play("carta|PlaneAction");
            Destroy(gameObject, 5f);
        }
    }
}