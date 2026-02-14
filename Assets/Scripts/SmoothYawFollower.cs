using UnityEngine;

/// <summary>
/// Makes an animated character smoothly match the yaw (horizontal rotation)
/// of a ragdoll reference, eliminating jitter while remaining responsive.
/// </summary>
public class SmoothYawFollower : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The ragdoll Transform whose horizontal rotation should be followed.")]
    public Transform ragdollRoot;

    [Header("Settings")]
    [Tooltip("How quickly to rotate toward the target (higher = faster response).")]
    public float followSpeed = 10f;

    [Tooltip("How much smoothing to apply to small jitter (higher = smoother, but less responsive).")]
    public float damping = 0.15f;

    private float targetYaw;
    private float currentYaw;
    private float yawVelocity;

    void Start()
    {
        if (ragdollRoot == null)
            enabled = false;

        targetYaw = ragdollRoot.eulerAngles.y;
        currentYaw = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (ragdollRoot == null) return;

        // Get target yaw from ragdoll (horizontal rotation only)
        targetYaw = ragdollRoot.eulerAngles.y;

        // SmoothDampAngle provides stable, non-oscillating motion
        currentYaw = Mathf.SmoothDampAngle(
            currentYaw,
            targetYaw,
            ref yawVelocity,
            damping,
            followSpeed * 100f,
            Time.deltaTime
        );

        // Apply yaw while preserving pitch and roll of the animator
        Vector3 euler = transform.eulerAngles;
        euler.y = currentYaw;
        transform.eulerAngles = euler;
    }
}
