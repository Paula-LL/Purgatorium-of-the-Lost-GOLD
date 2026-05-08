using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTutorialCon : MonoBehaviour
{
    public TutorialController Tutcontroller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Tutcontroller.showDashAndAttack();
        }
    }
}
