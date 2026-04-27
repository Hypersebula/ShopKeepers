using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [Header("Button Settings")]
    public Transform buttonTop;
    private float upY = 0f;
    private float downY = -0.05f;
    public float springStrength = 10f;
    public float pressThreshold = 0.8f;
    public float travelDistance = 0.05f;

    [Header("Actions")]
    public GameObject destroyObject1;
    public GameObject destroyObject2;
    public GameObject unhideObject;
    public GameObject capsule;
    public GameObject ragdoll;
    public Transform targetLocation;
    public float EnableTime = 1f;
    public RagdollStateController ragdollStateController;

    [Header("Custom Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    public bool isPressed = false;
    private float currentY;

    private void Start()
    {
        upY = buttonTop.localPosition.y;
        downY = upY - travelDistance;
        currentY = upY;
    }

    private void FixedUpdate()
    {
        currentY = Mathf.Lerp(currentY, upY, Time.fixedDeltaTime * springStrength);
        Vector3 pos = buttonTop.localPosition;
        pos.y = currentY;
        buttonTop.localPosition = pos;

        float pressAmount = 1f - Mathf.InverseLerp(downY, upY, currentY);

        if (!isPressed && pressAmount >= pressThreshold)
        {
            isPressed = true;
            OnButtonPressed();
        }
        else if (isPressed && pressAmount < pressThreshold * 0.5f)
        {
            isPressed = false;
            onReleased.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        float force = rb.mass * Physics.gravity.magnitude;
        currentY = Mathf.MoveTowards(currentY, downY, force * Time.fixedDeltaTime * 0.1f);
    }

    private void OnButtonPressed()
    {
        onPressed.Invoke();
        if (destroyObject1 != null) Destroy(destroyObject1);
        if (destroyObject2 != null) Destroy(destroyObject2);
        if (unhideObject != null) unhideObject.SetActive(true);
        if (capsule != null && ragdoll != null && targetLocation != null)
            StartCoroutine(Teleport());
    }

    private System.Collections.IEnumerator Teleport()
    {
        ragdollStateController.globalMultiplier = 0f;
        ragdollStateController.ApplyMultipliers();

        foreach (Collider col in ragdoll.GetComponentsInChildren<Collider>())
            col.enabled = false;

        capsule.SetActive(false);

        yield return null;

        Vector3 offset = targetLocation.position - ragdoll.transform.position;
        capsule.transform.position = targetLocation.position;

        foreach (Rigidbody rb in ragdoll.GetComponentsInChildren<Rigidbody>())
        {
            rb.position += offset;
            rb.linearVelocity = Vector3.zero;
        }

        yield return new WaitForSeconds(0.2f);

        capsule.SetActive(true);

        foreach (Collider col in ragdoll.GetComponentsInChildren<Collider>())
            col.enabled = true;

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(ragdollStateController.LerpMultiplier(1f, 0.5f));
    }
}