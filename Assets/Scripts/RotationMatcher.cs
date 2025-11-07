using UnityEngine;

public class RotationMatcher : MonoBehaviour
{
    public Transform Target;

    private void FixedUpdate()
    {
        if (Target == null) return;
        transform.rotation = Target.rotation;
    }
}
