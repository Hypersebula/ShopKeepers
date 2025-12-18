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

        //// Rotation
        //Vector3 capsuleForward = CapsuleOrientation.forward;
        //capsuleForward.y = 0f;
        //capsuleForward.Normalize();

        //Vector3 hipsForward = hipsTransform.forward;
        //hipsForward.y = 0f;
        //hipsForward.Normalize();

        //float yawError = Vector3.SignedAngle(hipsForward, capsuleForward, Vector3.up);

        //Quaternion yawOffset = Quaternion.Euler(0f, 0f, yawError);
        //joint.SetTargetRotationLocal(
        //    yawOffset * startLocalRotation, startLocalRotation
        //);
    }
}
