 using UnityEngine;

public class Grabbing : MonoBehaviour
{
    [Header("Ray Settings")]
    public Transform shoulder;
    public Vector3 rayOriginOffset;
    public float maxDistance = 2f;
    public string grabbableTag = "Grabbable";
    public LayerMask raycastMask;

    [Header("References")]
    public Transform aimer;
    public Camera cam;

    [Header("Debug")]
    public bool drawGizmos = true;

    [Header("IK")]
    public Transform ikTarget;
    public Transform ikTargetHome;

    private Vector3 reachGoal;

    [Header("Activation")]
    public KeyCode grabKey = KeyCode.Mouse0;
    public float reachSpeed = 5f;

    private float reachAmount = 0f;

    private void Update()
    {
        UpdateReachGoal();

        float target = Input.GetKey(grabKey) ? 1f : 0f;
        reachAmount = Mathf.MoveTowards(reachAmount, target, Time.deltaTime * reachSpeed);

        ikTarget.position = Vector3.Lerp(ikTargetHome.position, reachGoal, reachAmount);
    }

    private Ray GetGrabRay()
    {
        Vector3 origin = shoulder.position + cam.transform.TransformDirection(rayOriginOffset);
        Vector3 direction = aimer.forward;
        return new Ray(origin, direction);
    }

    private void UpdateReachGoal()
    {
        Ray ray = GetGrabRay();

        if(Physics.Raycast(ray, out RaycastHit hit, maxDistance, raycastMask))
        {
            reachGoal = hit.point;
        }
        else
        {
            reachGoal = ray.origin + ray.direction * maxDistance;
        }
    }

    public void OnHandContact(Rigidbody hitRb, Vector3 constactPoint)
    {
        if (reachAmount < 0.9f) return;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos || cam == null || shoulder == null || aimer == null) return;
        Ray ray = GetGrabRay();
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray.origin, ray.direction * maxDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(reachGoal, 0.05f);
    }
}
