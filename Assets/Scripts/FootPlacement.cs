using UnityEngine;

public class FootPlacement : MonoBehaviour
{
    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldPosition;

    [SerializeField] Transform hips;

    [SerializeField] float legSpacing;
    [SerializeField] float stepDistance;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSpeed;
    [SerializeField] float hipOffset;

    float lerp;

    public bool isStepping;

    LayerMask layerMask;

    [SerializeField] FootPlacement otherFeet;
    public SpeedTracker tracker;

    private Vector3 lastPosition;
    public Vector3 direction;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        Ray ray = new Ray(hips.position + (Vector3.right * legSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 2, layerMask))
            newPosition = info.point; transform.position = newPosition;

        lastPosition = hips.position;
    }

    private void LateUpdate()
    {
        transform.position = currentPosition;

        Vector3 displacement = hips.position - lastPosition;
        direction = displacement.normalized;

        lastPosition = hips.position;

        stepSpeed = tracker.horizontalSpeed * 2.2f;
        stepSpeed = Mathf.Clamp(stepSpeed, 3f, 15f);

        hipOffset = tracker.horizontalSpeed * 0.25f;
        hipOffset = Mathf.Clamp(hipOffset, 0.15f, 1.5f);

        stepDistance = tracker.horizontalSpeed * 0.25f;
        stepDistance = Mathf.Clamp(stepDistance, 0.5f, 0.9f);

        Ray ray = new Ray(hips.position + (hips.right * legSpacing) + (direction * hipOffset), Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit info, 2, layerMask))
        {
            if(Vector3.Distance(newPosition, info.point) > stepDistance && !otherFeet.isStepping)
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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }
}
