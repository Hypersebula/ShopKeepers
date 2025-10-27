using UnityEngine;

public class FollowHips : MonoBehaviour
{
    public Transform targetHips; // The hips transform to follow
    public Vector3 offset;       // Local offset from hips

    void LateUpdate()
    {
        if (targetHips == null) return;

        transform.position = targetHips.position + targetHips.rotation * offset;
    }
}
