using UnityEngine;

public class FeetGrounding : MonoBehaviour
{
    public float checkRadius = 0.2f;
    public float castRadius = 0.3f;
    public bool isGrounded;

    public LayerMask groundMask;

    RaycastHit hit;

    private void Update()
    {
        isGrounded = (Physics.CheckSphere(transform.position, checkRadius, groundMask));
        isGrounded = (Physics.Raycast(transform.position, Vector3.down, castRadius, groundMask));

        Debug.DrawRay(transform.position, Vector3.down * castRadius,
            isGrounded ? Color.green : Color.red);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
