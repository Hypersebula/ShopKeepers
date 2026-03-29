using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [Header("Points")]
    public Transform capsulePoint;
    public Transform ragdollHead;

    [Header("Blend")]
    [Range(0f, 1f)]
    public float blendAmount = 0f;
    public float blendSpeed = 5f;
    private float currentBlend = 0f;
    public AnimationCurve distanceToCurve;
    public float maxDistance = 1f;

    private void Update()
    {
        float distance = Vector3.Distance(capsulePoint.position, ragdollHead.position);
        float normalized = Mathf.Clamp01(distance / maxDistance);
        float target = distanceToCurve.Evaluate(normalized);

        currentBlend = Mathf.MoveTowards(currentBlend, target, Time.deltaTime * blendSpeed);

        transform.position = Vector3.Lerp(capsulePoint.position, ragdollHead.position, currentBlend);
        transform.rotation = Quaternion.Slerp(capsulePoint.rotation, ragdollHead.rotation, currentBlend);
    }
}
