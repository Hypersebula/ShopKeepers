//using UnityEngine;

//public class RagdollBone : MonoBehaviour
//{
//    [Header("References")]
//    public HealthManager healthManager;
//    public RagdollStateController ragdollStateController;

//    [Header("Impact Settings")]
//    public float boneMultiplier = 1f;
//    public float minImpactThreshold = 5f;
//    public float mediumImpactThreshold = 20f;
//    public float heavyImpactThreshold = 50f;

//    [Header("Strength Response")]
//    public float mediumStrengthMultiplier = 0.6f;
//    public float heavyStrengthMultiplier = 0.2f;
//    public float recoveryDuration = 1f;

//    [Header("Capsule Response")]
//    public CapsuleMovement capsuleMovement;
//    public float capsuleSlowDuration = 0.5f;

//    private float lastImpactTime;
//    public float impactCooldown = 0.2f;

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ragdoll")) return;

//        if (Time.time - lastImpactTime < impactCooldown) return;

//        Vector3 relVel = collision.relativeVelocity;
//        // reduce vertical impact contribution so landing doesnt count as a hit
//        relVel.y *= 0.2f;
//        float impactValue = relVel.magnitude *
//            (collision.rigidbody != null ? collision.rigidbody.mass : 1f) * boneMultiplier;

//        if (impactValue < minImpactThreshold) return;

//        lastImpactTime = Time.time;

//        float damage = impactValue * 0.5f;
//        healthManager.TakeDamage(damage);

//        if (impactValue >= heavyImpactThreshold)
//            StartCoroutine(StrengthResponse(heavyStrengthMultiplier, recoveryDuration));
//        else if (impactValue >= mediumImpactThreshold)
//            StartCoroutine(StrengthResponse(mediumStrengthMultiplier, recoveryDuration));
//    }

//    private System.Collections.IEnumerator StrengthResponse(float targetMultiplier, float duration)
//    {
//        float current = ragdollStateController.globalMultiplier;
//        float newMultiplier = Mathf.Min(current, targetMultiplier);

//        // slow capsule on medium+ impact
//        if (capsuleMovement != null)
//            StartCoroutine(SlowCapsule());

//        yield return StartCoroutine(ragdollStateController.LerpMultiplier(newMultiplier, 0.1f));
//        yield return new WaitForSeconds(duration);
//        yield return StartCoroutine(ragdollStateController.LerpMultiplier(1f, recoveryDuration));
//    }

//    private System.Collections.IEnumerator SlowCapsule()
//    {
//        float originalWalk = capsuleMovement.walkSpeed;
//        float originalSprint = capsuleMovement.sprintSpeed;
//        float originalFollow = ragdollStateController.capsuleFollower.followStrenght;

//        capsuleMovement.walkSpeed = 0f;
//        capsuleMovement.sprintSpeed = 0f;
//        ragdollStateController.capsuleFollower.followStrenght = 0f;

//        yield return new WaitForSeconds(capsuleSlowDuration);

//        capsuleMovement.walkSpeed = originalWalk;
//        capsuleMovement.sprintSpeed = originalSprint;
//        ragdollStateController.capsuleFollower.followStrenght = originalFollow;
//    }
//}