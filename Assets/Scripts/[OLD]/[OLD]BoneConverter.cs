using UnityEngine;

[System.Serializable]
public class BoneConvert
{
    public string Name;
    public Transform GhostBone;
    public ConfigurableJoint Joint;
    [HideInInspector] public Quaternion initialLocalRotation;
}

public class BoneConverter : MonoBehaviour
{
    public BoneConvert[] convert;

    private void Awake()
    {
        foreach (var b in convert)
        {
            if (b.Joint)
            {
                // Get relative rotation at bind pose
                Quaternion jointSpaceRot = Quaternion.Inverse(b.Joint.connectedBody.transform.rotation) * b.Joint.transform.rotation;
                Quaternion ghostSpaceRot = Quaternion.Inverse(b.Joint.connectedBody.transform.rotation) * b.GhostBone.rotation;

                b.initialLocalRotation = Quaternion.Inverse(ghostSpaceRot) * jointSpaceRot;
            }
        }
    }

    private void LateUpdate()
    {
        foreach (var b in convert)
        {
            if (b.Joint == null || b.GhostBone == null) continue;

            // Ghost bone rotation in joint-space
            Quaternion targetInJointSpace =
                Quaternion.Inverse(b.Joint.connectedBody.transform.rotation) * b.GhostBone.rotation;

            // Apply calibration offset
            b.Joint.targetRotation = targetInJointSpace * b.initialLocalRotation;
        }
    }
}
