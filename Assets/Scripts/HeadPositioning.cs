using UnityEngine;

public class HeadPositioning : MonoBehaviour
{
    public float upwardForce = 50f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * upwardForce, ForceMode.Force);
    }
}
