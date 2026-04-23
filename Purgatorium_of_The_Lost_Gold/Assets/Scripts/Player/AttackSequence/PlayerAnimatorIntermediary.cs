using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorIntermediary : MonoBehaviour
{
    [SerializeField] AttackSequenceControler attackSequenceControler;
    public void OnAttackAnimationBegun()
    {
        attackSequenceControler.ResetTimeSinceLastAttack();
    }
}
