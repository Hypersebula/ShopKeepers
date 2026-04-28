using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public bool startAtA = true;

    private Vector3 target;
    private Vector3 lastPosition;
    private Rigidbody playerRb;

    public LegTarget leftLeg;
    public LegTarget rightLeg;

    private void Start()
    {
        target = startAtA ? pointB.position : pointA.position;
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
            target = target == pointA.position ? pointB.position : pointA.position;

        if (playerRb != null)
        {
            Vector3 delta = transform.position - lastPosition;
            playerRb.MovePosition(playerRb.position + delta);

            if (leftLeg != null) leftLeg.transform.position += delta;
            if (rightLeg != null) rightLeg.transform.position += delta;
        }

        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerRb = other.rigidbody;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerRb = null;
    }
}