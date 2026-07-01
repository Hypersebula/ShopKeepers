using UnityEngine;

public class WeightPlatform : MonoBehaviour
{
    [Header("Settings")]
    public int requiredObjects = 3;

    [Header("Actions")]
    public GameObject destroyObject;
    public GameObject materialObject;
    public Material activatedMaterial;

    private int objectCount = 0;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            objectCount++;
            CheckActivation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            objectCount--;
            objectCount = Mathf.Max(0, objectCount);
        }
    }

    private void CheckActivation()
    {
        if (activated || objectCount < requiredObjects) return;
        activated = true;

        if (materialObject != null)
            materialObject.GetComponent<Renderer>().material = activatedMaterial;

        if (destroyObject != null)
            Destroy(destroyObject);
    }
}