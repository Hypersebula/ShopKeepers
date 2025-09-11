using UnityEngine;

public class HeadTargetPositioner : MonoBehaviour
{
    public Transform hips;

    public float headHeight = 1.5f;

    private void LateUpdate()
    {
        if (hips == null) return;

        Vector3 pos = hips.position;

        pos.y += headHeight;

        transform.position = pos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
