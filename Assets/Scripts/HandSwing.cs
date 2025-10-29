using UnityEngine;

public class HandSwing : MonoBehaviour
{
    public SpeedTracker tracker;

    [SerializeField] Transform hips;

    public float spacing;
    public float altitude;
    public float verticalOffset;
    public float swingSpeed;

    [Range(0, 1)] public float maxSwing = 1f;
    [Range(0, 1)] public float minSwing = 0f;

    public float lerp;

    Vector3 frontPoint;
    Vector3 backPoint;

    public bool isSwinging;

    private void LateUpdate()
    {
        if (tracker.horizontalSpeed <= 0.25f)
        {
            swingSpeed = 0f;
            altitude = 0f;
        }

        swingSpeed = tracker.horizontalSpeed * 0.8f;
        swingSpeed = Mathf.Clamp(swingSpeed, 0f, 3f);
        swingSpeed = Mathf.Round(swingSpeed * 10f) / 10f;

        altitude = tracker.horizontalSpeed * 0.1f;
        altitude = Mathf.Clamp(altitude, 0f, 1.5f);
        altitude = Mathf.Round(altitude * 10f) / 10f;

        frontPoint = hips.position + hips.right * spacing + hips.forward * altitude + hips.up * verticalOffset;
        backPoint = hips.position + hips.right * spacing - hips.forward * altitude + hips.up * verticalOffset;

        Vector3 handPos = Vector3.Lerp(frontPoint, backPoint, lerp);
        lerp = Mathf.Clamp(lerp, minSwing, maxSwing);

        transform.position = handPos;

        if (isSwinging)
        {
            lerp -= Time.deltaTime * swingSpeed;
            if (lerp <= minSwing)
                isSwinging = false;
        }
        if(!isSwinging)
        {
            lerp += Time.deltaTime * swingSpeed;
            if (lerp >= maxSwing)
                isSwinging = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(frontPoint, 0.05f);
        Gizmos.DrawSphere(backPoint, 0.05f);
    }
}
