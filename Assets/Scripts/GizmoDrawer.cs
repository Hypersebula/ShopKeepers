using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{
    public Color gizmoColor = Color.red;
    public float gizmoSize = 0.1f;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoSize);
    }
}
