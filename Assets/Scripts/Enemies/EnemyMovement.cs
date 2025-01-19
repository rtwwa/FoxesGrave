using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] private float gravity = -9.81f;
    private float groundCheckDistance = 0.1f;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;

    private void Update()
    {
        isGrounded = IsGrounded();

        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            // Если игрок на земле, вертикальная скорость сбрасывается
            if (verticalVelocity < 0)
                verticalVelocity = 0f;
        }

        col.transform.position = new Vector3(col.transform.position.x, col.transform.position.y + verticalVelocity * Time.deltaTime, col.transform.position.z);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;

        // Cast a ray directly downward from the object's position
        if (Physics.Raycast(col.transform.position + Vector3.down * 1f, Vector3.down, out hit, groundCheckDistance))
        {
            Debug.Log(hit.collider.gameObject.name);
            return true;
        }

        return false;
    }
}
