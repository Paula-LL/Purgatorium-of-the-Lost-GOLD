using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSequenceControler : MonoBehaviour
{
    private Animator anim;
    private float coolDownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if (noOfClicks == 1) {
            anim.SetBool("A_seq1", true);
        }

        noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);
    }
}
