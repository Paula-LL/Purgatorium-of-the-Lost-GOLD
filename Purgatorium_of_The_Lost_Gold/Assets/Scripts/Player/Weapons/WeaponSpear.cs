using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpear : MonoBehaviour
{
    public float damage;

    CapsuleCollider capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss") {
            Debug.Log("Enemy hit");
        }
    }

    public void EnableTriggerCapsule() { 
        capsuleCollider.enabled = true;
    }

    public void DisableTriggerCapsule() {
        capsuleCollider.enabled = false;
    }
}
