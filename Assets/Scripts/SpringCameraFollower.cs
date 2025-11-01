using UnityEngine;

public class SpringCameraFollower : MonoBehaviour
{
    public Transform head;
    public Transform headTarget;

    Vector3 velocity;
    Vector3 angularVelocity;

    public float frequency = 6f;
    [Range(0.1f, 2f)] public float damping = 1f;
    [Range(0f, 2f)] public float smoothness = 1f;

    private void LateUpdate()
    {
        Vector3 targetPos = head.position;
        Vector3 currentPos = transform.position;

        // Apply spring smoothing
        transform.position = SpringTo(currentPos, targetPos, ref velocity, frequency, damping, Time.deltaTime * smoothness);

        //Vector3 cameraPosition = transform.position;
        //Vector3 targetForward = (headTarget.position - cameraPosition).normalized;
        //Quaternion targetRot = Quaternion.LookRotation(targetForward, Vector3.right);
        //transform.rotation = RotateTo(transform.rotation, targetRot, ref angularVelocity, frequency, damping, Time.deltaTime * smoothness);
    }

    public static Vector3 SpringTo(Vector3 current, Vector3 target, ref Vector3 velocity, float frequency, float damping, float dt)
    {
        float f = frequency * 2f * Mathf.PI;
        float g = 1f / (1f + 2f * dt * damping * f + dt * dt * f * f);
        Vector3 diff = current - target;
        Vector3 accel = (velocity + f * f * dt * diff) * (2f * damping * f * dt);
        velocity = (velocity - accel) * g;
        return target + (diff + velocity * dt) * g;
    }

    //public static Quaternion RotateTo(Quaternion current, Quaternion target, ref Vector3 angularVelocity, float frequency, float damping, float dt)
    //{
    //    // Convert quaternion difference to axis-angle
    //    Quaternion deltaRot = target * Quaternion.Inverse(current);
    //    deltaRot.ToAngleAxis(out float angleDeg, out Vector3 axis);

    //    if (angleDeg > 180f) angleDeg -= 360f;  // normalize angle

    //    // Angular acceleration
    //    float f = frequency * 2f * Mathf.PI;
    //    float g = 1f / (1f + 2f * dt * damping * f + dt * dt * f * f);
    //    Vector3 accel = (angularVelocity + f * f * dt * axis * angleDeg * Mathf.Deg2Rad) * (2f * damping * f * dt);
    //    angularVelocity = (angularVelocity - accel) * g;

    //    // Apply rotation step
    //    Quaternion step = Quaternion.AngleAxis(angularVelocity.magnitude * dt * Mathf.Rad2Deg, angularVelocity.normalized);
    //    return step * current;
    //}
}
