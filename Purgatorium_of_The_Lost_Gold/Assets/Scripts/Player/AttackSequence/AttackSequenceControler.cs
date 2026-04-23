using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSequenceControler : MonoBehaviour
{
    private Animator anim;
    private PlayerAttack playerAttack;

    [SerializeField] private float nextFireTime = .4f;
    private float timeSinceLastAttack = 0;
    private float lastAttackTime = 1f;

    enum AttackStates { MOVE, AT1, AT2, AT3, AT4 };
    AttackStates currentState;

    private void Start()
    {
        currentState = AttackStates.MOVE;
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case AttackStates.MOVE:
                MoveUpdate();
                break;
            case AttackStates.AT1:
                At1Update();
                break;
            case AttackStates.AT2:
                At2Update();
                break;
            case AttackStates.AT3:
                At3Update();
                break;
            case AttackStates.AT4:
                At4Update();
                break;
        }
    }

    void MoveUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("A_seq1");
            timeSinceLastAttack = 0;
            currentState = AttackStates.AT1;
        }
    }

    void At1Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > nextFireTime)
        {
            anim.SetTrigger("CancelAttack");
            currentState = AttackStates.MOVE;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("A_seq2");
            timeSinceLastAttack = 0;
            currentState = AttackStates.AT2;
        }
    }

    void At2Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > nextFireTime)
        {
            anim.SetTrigger("CancelAttack");
            currentState = AttackStates.MOVE;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("A_seq3");
            timeSinceLastAttack = 0;
            currentState = AttackStates.AT3;
        }
    }

    public void ResetTimeSinceLastAttack()
    {
        timeSinceLastAttack = 0;
    }

    void At3Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > nextFireTime)
        {
            anim.SetTrigger("CancelAttack");
            currentState = AttackStates.MOVE;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("A_seq4");
            timeSinceLastAttack = 0;
            currentState = AttackStates.AT4;
        }
    }

    void At4Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > lastAttackTime)
        {
            currentState = AttackStates.MOVE;
        }
    }
    public void AnimationEvent_PerformAttack()
    {
        if (playerAttack != null)
            playerAttack.PerformAttack();
    }
}


/*private Animator anim;
    public float coolDownTime;
    private float nextFireTime = 2.5f;
    public static int noOfClicks = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1;
    private float timeSinceLastAttack = 1f; 

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
            //noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            anim.SetBool("A_seq1", false);
            //noOfClicks = 0;
        }//variable cooldown float to calc time since last attack
        if (timeSinceLastAttack > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastClickedTime = Time.time;
                timeSinceLastAttack = 0f;
                OnClick();
            }
        }
        else
            timeSinceLastAttack += Time.deltaTime;
    }*/

/*private void SetClicks() { 
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

}*/
//attack stuck in loop after A_seq1 is done
/*private void OnClick()
{
    lastClickedTime = Time.time;
    noOfClicks++;

    if (noOfClicks == 1) {
        anim.SetBool("A_seq1", true);
    }

    noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);

    if (noOfClicks >= 2) // && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("A_seq1"))
    {
        //anim.SetBool("A_seq1", false);
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

}*/