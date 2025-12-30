using UnityEngine;

public class RagdollVerticalLook : MonoBehaviour
{
    public Transform hips;

    [Header("Settings")]
    public float pointHeight = 2.5f;
    public float curveHeight = 1f;
    public float altitude = 2.5f;
    public float sensitivity = 100f;
    public float verticalLimit = 80f;

    private float verticalAngle = 0f; // accumulated rotation

    private float lockedX;

    private void Start()
    {
        lockedX = transform.localPosition.x;
    }

    private void LateUpdate()
    {
        // Lock the x position
        Vector3 l = transform.localPosition;
        l.x = lockedX;
        transform.localPosition = l;

        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        // accumulate rotation and clamp
        verticalAngle += mouseY;
        verticalAngle = Mathf.Clamp(verticalAngle, -verticalLimit, verticalLimit);

        // normalize to 0–1 for lerp
        float lerp = Mathf.InverseLerp(-verticalLimit, verticalLimit, verticalAngle);

        // compute front/back points
        Vector3 frontPos = hips.position + hips.up * pointHeight + hips.forward * altitude;
        Vector3 backPos = hips.position + hips.up * pointHeight - hips.forward * altitude;

        // interpolate position
        Vector3 targetPos = Vector3.Lerp(frontPos, backPos, lerp);
        targetPos.y += Mathf.Sin(lerp * Mathf.PI) * curveHeight;

        transform.position = targetPos;
    }

    private void OnDrawGizmos()
    {
        if (hips == null) return;

        Vector3 frontPos = hips.position + hips.up * pointHeight + hips.forward * altitude;
        Vector3 backPos = hips.position + hips.up * pointHeight - hips.forward * altitude;

        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(frontPos, 0.05f);
        Gizmos.DrawSphere(backPos, 0.05f);
    }
}
