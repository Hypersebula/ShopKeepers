using UnityEngine;

public class ForwardStateMachine : MonoBehaviour
{
    public Rigidbody rb;

    public bool movingForward = false;
    public bool movingBackward = false;
    public bool idle = false;

    public float treshold = 0.2f;

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;
        Vector3 moveDir = velocity.normalized;

        float forwardDot = Vector3.Dot(transform.forward, moveDir);

        movingForward = forwardDot > treshold;
        movingBackward = forwardDot < -treshold;
        idle = forwardDot < treshold && forwardDot > -treshold;
    }
}
