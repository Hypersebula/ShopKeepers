using UnityEngine;

public class ActiveRagdollMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float targetSpeed = 10f;
    public Transform orientation;
    public Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    public SpeedTracker tracker;
    public float forceMultiplier = 30f;

    float errorVel;
    float prevError;

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

        float currentSpeed = tracker.Speed;
        float rawError = targetSpeed - currentSpeed;

        float smoothError = Mathf.SmoothDamp(prevError, rawError, ref errorVel, 0.08f);
        prevError = smoothError;

        float force = smoothError * forceMultiplier;
        
        // zero out up/down
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // calculate movement direction
        moveDirection = (forward * verticalInput + right * horizontalInput).normalized;

        Vector3 targetVelocity = moveDirection * (moveSpeed + force);

        rb.AddForce(targetVelocity, ForceMode.Force);
    }
}
