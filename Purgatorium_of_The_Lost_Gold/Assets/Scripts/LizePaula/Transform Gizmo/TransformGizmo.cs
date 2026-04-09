using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGizmo : MonoBehaviour
{
    [SerializeField] float radius = 0.25f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
