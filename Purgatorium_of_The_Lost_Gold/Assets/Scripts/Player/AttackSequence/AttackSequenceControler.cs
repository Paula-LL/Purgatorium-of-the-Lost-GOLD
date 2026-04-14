using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSequenceControler : MonoBehaviour
{
    private Animator anim;
    public float coolDownTime;
    private float nextFireTime = 0.5f;
    public static int noOfClicks = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq1"))
        {
            anim.SetBool("A_seq1", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq2"))
        {
            anim.SetBool("A_seq2", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq3"))
        {
            anim.SetBool("A_seq3", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq4"))
        {
            anim.SetBool("A_seq4", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    private void SetClicks() { 
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq1"))
        {
            anim.SetBool("A_seq1", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq2"))
        {
            anim.SetBool("A_seq2", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq3"))
        {
            anim.SetBool("A_seq3", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq4"))
        {
            anim.SetBool("A_seq4", false);
            noOfClicks = 0; 
        }

        if (Time.time - lastClickedTime > maxComboDelay) {
            noOfClicks = 0; 
        }
        if (Time.time > nextFireTime) {
            if (Input.GetMouseButtonDown(0)) {
                OnClick(); 
            }
        }

    }
    //attack stuck in loop after A_seq1 is done
    private void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if (noOfClicks == 1) {
            anim.SetBool("A_seq1", true);
        }

        noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);

        if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq1")) {
            anim.SetBool("A_seq1", false);
            anim.SetBool("A_seq2", true);
        }

        if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq1"))
        {
            anim.SetBool("A_seq2", false);
            anim.SetBool("A_seq3", true);
        }

        if (noOfClicks >= 4 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq1"))
        {
            anim.SetBool("A_seq3", false);
            anim.SetBool("A_seq4", true);
        }

    }
}
