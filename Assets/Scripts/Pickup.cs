using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("References")]
    public Transform handTransform;
    public Rigidbody handRigidbody;
    public Camera cam;
    public float originalAngularDamping;

    [Header("Reach")]
    public Transform ikTarget;
    public Transform ikHome;
    public float reachSpeed = 8f;

    [Header("Hold Springs")]
    public float holdPositionSpring = 1000f;
    public float holdPositionDamper = 50f;
    public float holdRotationSpring = 100f;
    public float holdRotationDamper = 10f;

    private Pickupable heldObject;
    public bool isReaching = false;
    public bool isHolding = false;

    private void Update()
    {
        if (isReaching && !isHolding && heldObject != null)
        {
            ikTarget.position = Vector3.Lerp(ikTarget.position, heldObject.transform.position, Time.deltaTime * reachSpeed);
            if (Vector3.Distance(ikTarget.position, heldObject.transform.position) < 0.15f)
                AttachObject();
        }

        if (isHolding && (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.E)))
            Throw();
    }

    private void FixedUpdate()
    {
        if (!isHolding || heldObject == null) return;

        Vector3 targetPos = handTransform.position +
            handTransform.TransformDirection(heldObject.positionOffset);
        Vector3 posError = targetPos - heldObject.rb.position;
        Vector3 velError = -heldObject.rb.linearVelocity;
        Vector3 force = posError * holdPositionSpring + velError * holdPositionDamper;
        heldObject.rb.AddForce(force, ForceMode.Force);

        Quaternion targetRot = handTransform.rotation * Quaternion.Euler(heldObject.rotationOffset);
        Quaternion rotError = targetRot *
            Quaternion.Inverse(heldObject.rb.rotation);
        rotError.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
        if (Mathf.Abs(angle) > 0.01f)
        {
            Vector3 torque = axis * angle * Mathf.Deg2Rad * holdRotationSpring
                - heldObject.rb.angularVelocity * holdRotationDamper;
            heldObject.rb.AddTorque(torque, ForceMode.Force);
        }
    }

    public void PickUp(Pickupable target)
    {
        if (isHolding) return;
        heldObject = target;
        isReaching = true;
        SetCollisionIgnore(heldObject, true);
    }

    private void AttachObject()
    {
        isReaching = false;
        isHolding = true;
        ikTarget.position = ikHome.position;
        originalAngularDamping = heldObject.rb.angularDamping;
        heldObject.rb.angularDamping = 10f;
    }


    private void Throw()
    {
        isHolding = false;
        heldObject.rb.angularDamping = originalAngularDamping;
        SetCollisionIgnore(heldObject, false);
        heldObject.rb.linearVelocity = handRigidbody.linearVelocity;
        heldObject.rb.AddForce(cam.transform.forward * 10f, ForceMode.Impulse);
        heldObject = null;
    }


    private void SetCollisionIgnore(Pickupable target, bool ignore)
    {
        Collider[] objectColliders = target.GetComponentsInChildren<Collider>();
        Collider[] handColliders = handRigidbody.GetComponentsInChildren<Collider>();

        foreach (Collider objCol in objectColliders)
            foreach (Collider handCol in handColliders)
                Physics.IgnoreCollision(objCol, handCol, ignore);
    }
}