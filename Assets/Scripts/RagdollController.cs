using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float moveDaping = 10f;
    public Transform orientation;

    private Rigidbody hipsRb;

    private void Start()
    {
        hipsRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 targetVelocity = (orientation.forward * v + orientation.right * h).normalized * moveSpeed;
        targetVelocity.y = hipsRb.linearVelocity.y;

        Vector3 velocityError = targetVelocity - hipsRb.linearVelocity;
        hipsRb.AddForce(velocityError * moveDaping, ForceMode.Acceleration);
    }
}
