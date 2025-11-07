using UnityEngine;

/// <summary>
/// Smoothly follows a physics-based target (ragdoll hips) with a spring filter.
/// Designed for IK-driven animated characters.
/// </summary>
public class SpringFollower : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Transform of the ragdoll's hips to follow.")]
    public Transform Target;

    [Header("Tuning")]
    [Tooltip("How quickly the follower moves to the target. Higher = snappier.")]
    public float frequency = 6.0f;

    [Tooltip("How much damping to apply. 1 = critically damped. Lower = looser, higher = overshoots.")]
    [Range(0.1f, 2f)]
    public float damping = 1.0f;

    [Tooltip("Ignore vertical movement (Y axis). Useful for IK-based characters that handle height separately.")]
    public bool ignoreY = true;

    [Tooltip("Ignore smoothing by speed")]
    public bool ignoreSpeedSmoothing;

    [Tooltip("How strongly to follow vertical movement (0 = off, 1 = full).")]
    [Range(0f, 1f)]
    public float verticalResponse = 0.0f;

    [Tooltip("Multiplier for smoothing. Higher = smoother but slower.")]
    [Range(0f, 2f)]
    public float smoothness = 1.0f;

    [Header("Optional External Inputs")]
    [Tooltip("Optional SpeedTracker reference for adaptive smoothing.")]
    public SpeedTracker speedTracker;

    Vector3 velocity;

    void FixedUpdate()
    {
        //transform.rotation = Quaternion.Euler(
        //    transform.rotation.eulerAngles.x,
        //    ragdollHips.rotation.eulerAngles.y,
        //    transform.rotation.eulerAngles.z
        //);

        if (Target == null)
            return;

        Vector3 targetPos = Target.position;
        Vector3 currentPos = transform.position;

        // Handle vertical filtering
        if (ignoreY)
            targetPos.y = currentPos.y;
        else if (verticalResponse < 1f)
            targetPos.y = Mathf.Lerp(currentPos.y, targetPos.y, verticalResponse);

        if (speedTracker != null && !ignoreSpeedSmoothing)
            smoothness = speedTracker.horizontalSpeed * 0.1f;

        // Apply spring smoothing
        transform.position = SpringTo(currentPos, targetPos, ref velocity, frequency, damping, Time.fixedDeltaTime * smoothness);
    }

    /// <summary>
    /// Critically damped spring smoothing step.
    /// </summary>
    Vector3 SpringTo(Vector3 current, Vector3 target, ref Vector3 vel, float freq, float damping, float dt)
    {
        float f = freq * 2 * Mathf.PI;
        float g = 1f / (1f + 2f * dt * damping * f + dt * dt * f * f);
        Vector3 diff = current - target;
        Vector3 accel = (vel + f * f * dt * diff) * (2f * damping * f * dt);
        vel = (vel - accel) * g;
        return target + (diff + vel * dt) * g;
    }
}
