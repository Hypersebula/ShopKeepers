using UnityEngine;

public class PalmAimOffset : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Transform OffsetTarget;
    [SerializeField] Transform Palm;
    [SerializeField] float offset;

    private void LateUpdate()
    {
        Vector3 dir = (Target.position - Palm.position).normalized;
        OffsetTarget.position = Palm.position + dir * offset;
    }
}
