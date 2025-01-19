using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private void FixedUpdate()
    {
        Debug.Log($"Velocity: {rb.velocity}");
    }
}
