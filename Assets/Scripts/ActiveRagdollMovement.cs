using UnityEngine;

public class ActiveRagdollMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;

        // zero out up/down
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // calculate movement direction
        moveDirection = (forward * verticalInput + right * horizontalInput).normalized;

        Vector3 targetVelocity = moveDirection * moveSpeed;

        rb.AddForce(targetVelocity, ForceMode.Force);
    }
}
