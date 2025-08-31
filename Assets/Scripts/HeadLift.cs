using UnityEngine;

public class HeadLift : MonoBehaviour
{
    public float upwardForce = 50f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * upwardForce, ForceMode.Force);
    }
}
