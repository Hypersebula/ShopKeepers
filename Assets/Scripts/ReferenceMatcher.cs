using UnityEngine;

public class ReferenceMatcher : MonoBehaviour
{
    public Transform[] ragdollBones;
    public ConfigurableJoint[] joints;

    public Transform[] animatedBones;

    private void Start()
    {
        joints = new ConfigurableJoint[ragdollBones.Length];
        for (int i = 0; i < ragdollBones.Length; i++)
        {
            joints[i] = ragdollBones[i].GetComponent<ConfigurableJoint>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < ragdollBones.Length; i++)
        {
            if (joints[i] == null || animatedBones[i] == null) continue;

            Quaternion targetRot =
                Quaternion.Inverse(ragdollBones[i].parent.rotation) * animatedBones[i].rotation;

            joints[i].targetRotation = Quaternion.Inverse(targetRot);
        }
    }
}
