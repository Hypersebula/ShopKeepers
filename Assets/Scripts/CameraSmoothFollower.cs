using UnityEngine;

public class CameraSpringFollower : MonoBehaviour
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
    [Tooltip("Ignore vertical movement (Y axis).")]
    public bool ignoreY = true;
    [Tooltip("Ignore horizontal rotation")]
    public bool ignoreYaw;
    [Tooltip("Ignore smoothing by speed")]
    public bool ignoreSpeedSmoothing;
    [Tooltip("How strongly to follow vertical movement (0 = off, 1 = full).")]
    [Range(0f, 1f)]
    public float verticalResponse = 0.0f;
    [Tooltip("Multiplier for smoothing. Higher = smoother but slower.")]
    [Range(0f, 2f)]
    public float smoothness = 1.0f;
    [Tooltip("How much speed contributes to extra smoothness on top of the base.")]
    [Range(0f, 1f)]
    public float speedSmoothnessScale = 0.1f;

    [Header("Distance-Based Loosening")]
    [Tooltip("Object whose distance from the Target drives spring loosening (e.g. the ragdoll root).")]
    public Transform distanceObject;
    [Tooltip("Distance at which the spring begins to loosen.")]
    public float looseningStartDistance = 0.5f;
    [Tooltip("Distance at which the spring reaches maximum looseness.")]
    public float looseningMaxDistance = 3.0f;
    [Tooltip("Frequency multiplier when at max distance. Lower = much looser spring.")]
    [Range(0.01f, 1f)]
    public float minFrequencyMultiplier = 0.2f;

    [Header("Optional External Inputs")]
    [Tooltip("Optional SpeedTracker reference for adaptive smoothing.")]
    public SpeedTracker speedTracker;

    Vector3 velocity;

    void FixedUpdate()
    {
        if (Target == null)
            return;

        if (!ignoreYaw)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                Target.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);
        }

        Vector3 targetPos = Target.position;
        Vector3 currentPos = transform.position;

        float effectiveSmoothness = smoothness;
        if (speedTracker != null && !ignoreSpeedSmoothing)
            effectiveSmoothness += speedTracker.Speed * speedSmoothnessScale;


        // Compute frequency multiplier from distance
        float freqMultiplier = 1f;
        if (distanceObject != null)
        {
            float dist = Vector3.Distance(distanceObject.position, Target.position);
            float t = Mathf.InverseLerp(looseningStartDistance, looseningMaxDistance, dist);
            freqMultiplier = Mathf.Lerp(1f, minFrequencyMultiplier, t);
        }

        float effectiveFrequency = frequency * freqMultiplier;

        transform.position = SpringTo(currentPos, targetPos, ref velocity, effectiveFrequency, damping, Time.fixedDeltaTime * speedSmoothnessScale);
    }

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