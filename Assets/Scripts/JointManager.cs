using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Settings
{
    public string Name;

    public float localStrenghtMultiplier;

    public float localSpring = 0f;
    public float localDamper = 0f;

    public float twistLimit = 0f;
    public float swingLimit = 0f;
}

public class JointManager : MonoBehaviour
{
    public Settings[] bones;
    public Settings settings;

    [Header("Bools")]
    public bool AngularMotionLimited = false;
    public bool twistLimited = false;
    public bool swingLimited = false;
    public bool UpdateStrenght = false;

    [Header("Strenght")]
    public float globalStrenghtMultiplier = 1f;

    public float globalSpring = 100f;
    public float globalDamper = 10f;
    public float maxForce = 1000f;

    private List<ConfigurableJoint> strenghtJoints = new List<ConfigurableJoint>();

    private void Awake()
    {
        strenghtJoints.AddRange(GetComponentsInChildren<ConfigurableJoint>());
        ApplyStrenght();
    }

    public void RunSettings()
    {
        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>();

        foreach (var joint in joints)
        {
            // Try to find a Settings with the same name as joint gameObject
            foreach (var b in bones)
            {
                if (b.Name == joint.gameObject.name)
                {
                    settings = b;
                    break;
                }
            }

            // Apply angular Limits
            if (AngularMotionLimited)
            {
                joint.angularXMotion = ConfigurableJointMotion.Limited;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
                joint.angularZMotion = ConfigurableJointMotion.Limited;
            }
            // Apply twist Limits
            if (twistLimited)
            {
                SoftJointLimit lowX = new SoftJointLimit { limit = -settings.twistLimit };
                SoftJointLimit highX = new SoftJointLimit { limit = settings.twistLimit };
                joint.lowAngularXLimit = lowX;
                joint.highAngularXLimit = highX;
            }
            // Apply swing Limits
            if (swingLimited)
            {
                SoftJointLimit swingY = new SoftJointLimit { limit = settings.swingLimit };
                SoftJointLimit swingZ = new SoftJointLimit { limit = settings.swingLimit };
                joint.angularYLimit = swingY;
                joint.angularZLimit = swingZ;
            }
        }
    }

    public void ApplyStrenght()
    {
        foreach (var joint in strenghtJoints)
        {
            JointDrive drive = new JointDrive();
            drive.positionSpring = globalSpring * globalStrenghtMultiplier;
            drive.positionDamper = globalDamper * globalStrenghtMultiplier;
            drive.maximumForce = maxForce;

            joint.slerpDrive = drive;

            joint.angularXDrive = drive;
            joint.angularYZDrive = drive;
        }
    }
}
