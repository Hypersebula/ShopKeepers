using UnityEngine;

public class CapsuleFollower : MonoBehaviour
{
    [Header("Position")]
    public Transform Capsule;
    private Vector3 targetPos;
    public Rigidbody rb;
    public float followStrenght = 20f;

    [Header("Rotation")]
    public Transform CapsuleOrientation;
    public Transform hipsTransform;
    public ConfigurableJoint joint;
    Quaternion startLocalRotation;

    private void Start()
    {
        startLocalRotation = joint.transform.localRotation;
    }

    void FixedUpdate()
    {
        // Position
        targetPos = new Vector3(Capsule.position.x, Capsule.position.y, Capsule.position.z);

        Vector3 error = targetPos - rb.position;
        
        float stiffness = followStrenght;
        float damp = 2f * Mathf.Sqrt(stiffness);

        Vector3 force = error * stiffness - rb.linearVelocity * damp;

        rb.AddForce(force, ForceMode.Acceleration);

        // Rotation
        Quaternion capsuleYaw = Quaternion.Euler(CapsuleOrientation.eulerAngles.x, CapsuleOrientation.eulerAngles.y, CapsuleOrientation.eulerAngles.z);

        // Convert capsule yaw into joint local space
        Quaternion targetLocalRotation = Quaternion.Inverse(joint.transform.parent.rotation) * capsuleYaw;

        joint.SetTargetRotationLocal(targetLocalRotation, startLocalRotation);
    }
}
