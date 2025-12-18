using UnityEngine;

public class CapsuleFollower : MonoBehaviour
{
    public Transform Capsule;
    private Vector3 targetPos;
    public Rigidbody rb;

    public float followStrenght = 20f;

    void FixedUpdate()
    {
        targetPos = new Vector3(Capsule.position.x, Capsule.position.y, Capsule.position.z);

        Vector3 error = targetPos - rb.position;
        
        float stiffness = followStrenght;
        float damp = 2f * Mathf.Sqrt(stiffness);

        Vector3 force = error * stiffness - rb.linearVelocity * damp;

        rb.AddForce(force, ForceMode.Acceleration);
    }
}
