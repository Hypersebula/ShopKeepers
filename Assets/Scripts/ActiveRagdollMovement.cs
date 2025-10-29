using UnityEngine;

public class ActiveRagdollMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
        }
    }
}
