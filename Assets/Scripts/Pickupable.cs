using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [Header("Hold Settings")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public string interactionText = "Pick up [E]";

    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}