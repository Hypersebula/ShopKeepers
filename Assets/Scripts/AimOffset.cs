using UnityEngine;

public class AimOffset : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Transform OffsetTarget;
    [SerializeField] Transform Aimer;
    [SerializeField] float offset;

    private void LateUpdate()
    {
        Vector3 dir = (Target.position - Aimer.position).normalized;
        OffsetTarget.position = Aimer.position + dir * offset;
    }
}
