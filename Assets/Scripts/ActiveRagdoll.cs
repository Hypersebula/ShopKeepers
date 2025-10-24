using UnityEngine;
using System.Collections.Generic;

public class RagdollFollower : MonoBehaviour
{
    public Transform animatedRoot;
    public Transform ragdollRoot;

    // Each animated bone -> joint and its bind-pose offset
    private class BoneData
    {
        public ConfigurableJoint joint;
        public Quaternion rotationOffset;
    }

    private Dictionary<Transform, BoneData> boneMap = new Dictionary<Transform, BoneData>();

    void Start()
    {
        // Build map and cache offsets in the bind pose
        var ragdollJoints = ragdollRoot.GetComponentsInChildren<ConfigurableJoint>();
        foreach (var joint in ragdollJoints)
        {
            Transform animatedBone = FindChildByName(animatedRoot, joint.transform.name);
            if (animatedBone == null) continue;

            BoneData data = new BoneData();
            data.joint = joint;

            // Capture offset between animated bone and ragdoll joint at rest pose
            // joint.connectedBody is the parent bone
            if (joint.connectedBody != null)
            {
                Quaternion animatedLocal = Quaternion.Inverse(joint.connectedBody.transform.rotation) * animatedBone.rotation;
                Quaternion ragdollLocal = Quaternion.Inverse(joint.connectedBody.transform.rotation) * joint.transform.rotation;
                data.rotationOffset = Quaternion.Inverse(animatedLocal) * ragdollLocal;
            }
            else
            {
                data.rotationOffset = Quaternion.identity;
            }

            boneMap[animatedBone] = data;
        }
    }

    void LateUpdate()
    {
        foreach (var kvp in boneMap)
        {
            Transform animatedBone = kvp.Key;
            BoneData data = kvp.Value;
            ConfigurableJoint joint = data.joint;

            if (joint.connectedBody == null) continue;

            // Calculate desired local rotation relative to parent (connected body)
            Quaternion targetLocalRotation =
                Quaternion.Inverse(joint.connectedBody.transform.rotation) * animatedBone.rotation;

            // Apply the cached offset so ragdoll matches animated orientation
            joint.targetRotation = targetLocalRotation * data.rotationOffset;
        }
    }

    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform t in parent.GetComponentsInChildren<Transform>())
            if (t.name == name) return t;
        return null;
    }
}
