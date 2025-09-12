using UnityEngine;

public class FeetGrounding : MonoBehaviour
{
    public float checkRadius = 0.2f;
    public bool isGrounded;

    public LayerMask groundMask;

    RaycastHit hit;

    private void Update()
    {
        isGrounded = (Physics.CheckSphere(transform.position, checkRadius, groundMask));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
