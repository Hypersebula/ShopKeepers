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

    [Header("Joint Snapping")]
    public float jointBrakeForce = 1500f;
    public float jointBrakeTorque = 1500f;

    private float reachAmount = 0f;

    public Rigidbody handRigidbody;
    private ConfigurableJoint grabJoint;

    public HandContact handContact;

    public bool IsGrabbing { get; private set; }
    public Vector3 GrabPoint { get; private set; }

    private Rigidbody grabbedRigidbody;

    private void Update()
    {
        UpdateReachGoal();

        float target = Input.GetKey(grabKey) ? 1f : 0f;
        reachAmount = Mathf.MoveTowards(reachAmount, target, Time.deltaTime * reachSpeed);

        ikTarget.position = Vector3.Lerp(ikTargetHome.position, reachGoal, reachAmount);

        if (Input.GetKeyDown(grabKey))
            handContact.active = true;

        if (Input.GetKeyUp(grabKey) && grabJoint != null)
        {
            handContact.active = false;
            if(grabJoint != null) 
            {
                Destroy(grabJoint);
                grabJoint = null;
                IsGrabbing = false;
                GrabPoint = Vector3.zero;
                grabbedRigidbody = null;
            }
        }
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

    public void OnHandContact(Rigidbody hitRb, Vector3 contactPoint)
    {
        if (reachAmount < 0.9f) return;
        if (hitRb == null) return;
        if (grabJoint != null) return;

        GrabPoint = contactPoint;
        IsGrabbing = true;

        grabbedRigidbody = hitRb;

        grabJoint = handRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        grabJoint.connectedBody = hitRb;
        grabJoint.xMotion = ConfigurableJointMotion.Locked;
        grabJoint.yMotion = ConfigurableJointMotion.Locked;
        grabJoint.zMotion = ConfigurableJointMotion.Locked;
        grabJoint.angularXMotion = ConfigurableJointMotion.Locked;
        grabJoint.angularYMotion = ConfigurableJointMotion.Locked;
        grabJoint.angularZMotion = ConfigurableJointMotion.Locked;
        grabJoint.breakForce = jointBrakeForce;
        grabJoint.breakTorque = jointBrakeTorque;

        Debug.Log("Grabbed: " + hitRb.gameObject.name);
    }

    private void OnJointBreak(float breakForce)
    {
        grabJoint = null;
        IsGrabbing = false;
        GrabPoint = Vector3.zero;
        handContact.active = false;

        if (grabbedRigidbody != null)
        {
            grabbedRigidbody = null;
        }
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
