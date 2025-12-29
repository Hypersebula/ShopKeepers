using UnityEngine;

public class LegTarget : MonoBehaviour
{
    [Tooltip("The base for target positions")]
    public Transform hips;

    public LayerMask Ground;

    [Header("Values")]
    public float legSpacing = 0.25f;

    Vector3 newPosition;
    Vector3 currentPosition;
    Vector3 oldPosition;

    private void LateUpdate()
    {
        transform.position = currentPosition; // Match the position to the moving target

        Ray ray = new Ray(hips.position + (hips.right * legSpacing), -hips.up);
        if (Physics.Raycast(ray, out RaycastHit info, 2, Ground))
        {
            newPosition = info.point;
        }

        currentPosition = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }
}
