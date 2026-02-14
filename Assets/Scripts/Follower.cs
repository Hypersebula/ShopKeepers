using UnityEngine;

public class FollowHips : MonoBehaviour
{
    public Transform target; // The hips transform to follow
    public Vector3 offset;       // Local offset from hips

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + target.rotation * offset;
    }
}
