using UnityEngine;

[System.Serializable]
public class JointGroup
{
    public string name;
    public ConfigurableJoint[] joints;
    [Range(0f, 10000f)] public float spring = 1000f;
    [Range(0f, 1000f)] public float damper = 100f;
    [Range(0f, 10000f)] public float maxForce = 10000f;
}

public class RagdollJointController : MonoBehaviour
{
    [Header("Bone Groups")]
    public JointGroup hips;
    public JointGroup spine;
    public JointGroup head;
    public JointGroup arms;
    public JointGroup legs;

    private void OnValidate()
    {
        ApplyAll();
    }

    public void ApplyAll()
    {
        ApplyGroup(hips);
        ApplyGroup(spine);
        ApplyGroup(head);
        ApplyGroup(arms);
        ApplyGroup(legs);
    }

    public void ApplyGroup(JointGroup group)
    {
        foreach (var joint in group.joints)
        {
            if (joint == null) continue;
            SetDrive(joint, group.spring, group.damper, group.maxForce);
        }
    }

    private void SetDrive(ConfigurableJoint joint, float spring, float damper, float maxForce)
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = spring,
            positionDamper = damper,
            maximumForce = maxForce
        };

        joint.angularXDrive = drive;
        joint.angularYZDrive = drive;
    }

    // Call this to knock out the ragdoll
    public void SetAllDead()
    {
        SetGroupStrength(hips, 0f, 0f);
        SetGroupStrength(spine, 0f, 0f);
        SetGroupStrength(head, 0f, 0f);
        SetGroupStrength(arms, 0f, 0f);
        SetGroupStrength(legs, 0f, 0f);
        ApplyAll();
    }

    // Call this to restore full control
    public void SetAllAlive()
    {
        RestoreGroup(hips);
        RestoreGroup(spine);
        RestoreGroup(head);
        RestoreGroup(arms);
        RestoreGroup(legs);
        ApplyAll();
    }

    private void SetGroupStrength(JointGroup group, float spring, float damper)
    {
        group.spring = spring;
        group.damper = damper;
    }

    private void RestoreGroup(JointGroup group)
    {
        // Just reapply whatever is in the inspector
        ApplyGroup(group);
    }
}