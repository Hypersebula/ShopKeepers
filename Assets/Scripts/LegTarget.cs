using UnityEngine;

public class LegTarget : MonoBehaviour
{
    [Tooltip("The base for target positions")]
    public Transform hips;

    public LayerMask Ground;

    [Tooltip("The rigidbody for velocity measuring")]
    public Rigidbody rb;

    [Header("Scripts")]
    public LegTarget otherFeet;
    public SpeedTracker tracker;

    [Header("Values")]
    public float legSpacing = 0.25f;

    public float stepDistance = 0.5f;
    public float minStepDistance = 0.1f;
    public float maxStepDistance = 0.75f;

    [Range(0, 1)] float lerp;
    public float stepHeight = 0.25f;

    public float stepSpeed = 1f;
    public float minStepSpeed = 2f;
    public float maxStepSpeed = 5f;

    public float dirSpacing;
    public float minDirSpacing;
    public float maxDirSpacing;

    public float tweakSpeed = 2f;

    public bool isStepping = false;

    Vector3 constantPosition;
    Vector3 newPosition;
    Vector3 currentPosition;
    Vector3 oldPosition;

    private void LateUpdate()
    {
        transform.position = currentPosition; // Match the position to the moving target

        stepSpeed = Mathf.Clamp(stepSpeed, minStepSpeed, maxStepSpeed);
        stepDistance = Mathf.Clamp(stepDistance, minStepDistance, maxStepDistance);
        dirSpacing = Mathf.Clamp(dirSpacing, minDirSpacing, maxDirSpacing);

        if (tracker.Speed > 0.1f)
        {
            stepSpeed = stepSpeed + Time.deltaTime * tweakSpeed;
            stepDistance = stepDistance + Time.deltaTime * tweakSpeed;
            dirSpacing = dirSpacing + Time.deltaTime * tweakSpeed;
        }
        else
        {
            stepSpeed = stepSpeed - Time.deltaTime * tweakSpeed;
            stepDistance = stepDistance - Time.deltaTime * tweakSpeed;
            dirSpacing = dirSpacing - Time.deltaTime * tweakSpeed;
        }

        Vector3 moveDir = rb.linearVelocity.normalized;

        Ray ray = new Ray(hips.position + (hips.right * legSpacing) + (moveDir * dirSpacing), -Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit info, 2, Ground))
        {
            if(Vector3.Distance(newPosition, info.point) > stepDistance && !otherFeet.isStepping && !isStepping)
            {
                lerp = 0;
                newPosition = info.point;
            }
        }
        if(lerp < 1)
        {
            isStepping = true;

            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            isStepping = false;
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(constantPosition, 0.05f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }
}
