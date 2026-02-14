using UnityEngine;
using System.Collections;

public class ActiveRagdoll : MonoBehaviour
{
    [Header("Rotation")]
    public float angularVelocityGain = 50f;

    [Header("Position")]
    public float positionGain = 800f;
    public float positionDamping = 50f;
    public float maxPositionForce = 2000f;

    public Transform[] animated;
    public ConfigurableJoint[] joints;
    private Quaternion[] startPos;
    private Quaternion[] previousRotations;

    void Start()
    {
        startPos = new Quaternion[joints.Length];
        previousRotations = new Quaternion[joints.Length];

        for (int i = 0; i < joints.Length; i++)
        {
            startPos[i] = joints[i].transform.localRotation;
            previousRotations[i] = animated[i].localRotation;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            ConfigurableJoint joint = joints[i];
            Transform target = animated[i];
            Rigidbody body = joint.GetComponent<Rigidbody>();

            // Rotation matching
            joints[i].SetTargetRotationLocal(animated[i].localRotation, startPos[i]);

            // Compute angular velocity of animated bone manually
            Quaternion delta = target.localRotation * Quaternion.Inverse(previousRotations[i]);
            delta.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180f) angle -= 360f;

            Vector3 targetAngularVelocity = axis * angle * Mathf.Deg2Rad / Time.fixedDeltaTime;
            Vector3 velError = targetAngularVelocity - body.angularVelocity;
            body.AddTorque(velError * angularVelocityGain, ForceMode.Acceleration);

            // Position matching
            previousRotations[i] = target.localRotation;

            Vector3 positionError =
                target.position - body.position;

            Vector3 velocityError =
                -body.linearVelocity;

            Vector3 force =
                positionError * positionGain +
                velocityError * positionDamping;

            force = Vector3.ClampMagnitude(force, maxPositionForce);

            body.AddForce(force, ForceMode.Force);
        }
    }
}
