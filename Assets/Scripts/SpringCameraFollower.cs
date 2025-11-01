using UnityEngine;

/// <summary>
/// Smoothly follows a ragdoll head using spring-damped motion.
/// Filters pitch/yaw to prevent flipping, ignores roll, supports axis-specific damping.
/// </summary>
[RequireComponent(typeof(Camera))]
public class SpringCameraFollower : MonoBehaviour
{
    [Header("References")]
    public Transform targetHead;

    [Header("Position Spring")]
    public float frequency = 8f;
    [Range(0.1f, 2f)] public float damping = 1f;
    public Vector3 offset = new Vector3(0f, 0.1f, -0.15f);

    [Header("Rotation Follow")]
    [Tooltip("How strongly to follow pitch (X axis).")]
    [Range(0f, 1f)] public float pitchFollow = 1f;

    [Tooltip("How strongly to follow yaw (Y axis).")]
    [Range(0f, 1f)] public float yawFollow = 0.5f; // make this looser

    [Tooltip("How quickly to interpolate rotation changes.")]
    public float rotationLerpSpeed = 8f;

    [Tooltip("Enable for debug lines.")]
    public bool debugDraw = false;

    private Vector3 velocity;
    private float smoothedPitch;
    private float smoothedYaw;

    void LateUpdate()
    {
        if (!targetHead) return;

        // --- POSITION ---
        Vector3 targetPos = targetHead.TransformPoint(offset);
        transform.position = SpringTo(transform.position, targetPos, ref velocity, frequency, damping, Time.deltaTime);

        // --- ROTATION ---
        Quaternion headRot = targetHead.rotation;
        Vector3 headEuler = headRot.eulerAngles;

        // Convert to -180..180 range to avoid flipping
        headEuler.x = NormalizeAngle(headEuler.x);
        headEuler.y = NormalizeAngle(headEuler.y);

        // Smooth individual axes
        smoothedPitch = Mathf.LerpAngle(smoothedPitch, headEuler.x, pitchFollow * rotationLerpSpeed * Time.deltaTime);
        smoothedYaw = Mathf.LerpAngle(smoothedYaw, headEuler.y, yawFollow * rotationLerpSpeed * Time.deltaTime);

        // Apply smoothed rotation, ignore roll
        Quaternion targetRotation = Quaternion.Euler(smoothedPitch, smoothedYaw, 0f);
        transform.rotation = targetRotation;

        if (debugDraw)
        {
            Debug.DrawLine(transform.position, targetHead.position, Color.yellow);
            Debug.DrawRay(transform.position, transform.forward * 0.2f, Color.cyan);
        }
    }

    // Spring smoothing
    private Vector3 SpringTo(Vector3 current, Vector3 target, ref Vector3 vel, float freq, float damping, float dt)
    {
        float f = freq * 2f * Mathf.PI;
        float g = 1f / (1f + 2f * dt * damping * f + dt * dt * f * f);
        Vector3 diff = current - target;
        Vector3 accel = (vel + f * f * dt * diff) * (2f * damping * f * dt);
        vel = (vel - accel) * g;
        return target + (diff + vel * dt) * g;
    }

    // Convert any angle to -180..180
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
