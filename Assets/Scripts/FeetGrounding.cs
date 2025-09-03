using UnityEngine;

public class FeetGrounding : MonoBehaviour
{
    public float castDistance = 0.2f;
    public bool isGrounded;

    public LayerMask groundMask;

    RaycastHit hit;

    private void Update()
    {
        isGrounded = (Physics.Raycast(transform.position, Vector3.down, castDistance, groundMask));

        Debug.DrawRay(transform.position, Vector3.down * castDistance,
                      isGrounded ? Color.green : Color.red);
    }
}
