using UnityEngine;
using System.Collections;

public class ActiveRagdoll : MonoBehaviour
{
    public float angularVelocityGain = 50f;

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

            joints[i].SetTargetRotationLocal(animated[i].localRotation, startPos[i]);

            // Compute angular velocity of animated bone manually
            Quaternion delta = target.localRotation * Quaternion.Inverse(previousRotations[i]);
            delta.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180f) angle -= 360f;

            Vector3 targetAngularVelocity = axis * angle * Mathf.Deg2Rad / Time.fixedDeltaTime;
            Vector3 velError = targetAngularVelocity - body.angularVelocity;
            body.AddTorque(velError * angularVelocityGain, ForceMode.Acceleration);

            previousRotations[i] = target.localRotation;
        }
    }
}
