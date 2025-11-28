using UnityEngine;

public class HipsTargetPosition : MonoBehaviour
{
    public Transform TargetPosition;
    public ConfigurableJoint hips;

    private void Update()
    {
        hips.targetPosition = new Vector3(TargetPosition.transform.position.x, 0, TargetPosition.transform.position.z);
    }
}
