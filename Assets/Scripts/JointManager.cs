using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Settings
{
    public string Name;

    public float strenghtMultiplier = 1f;

    public float spring = 100f;
    public float damper = 10f;

    public float twistLimit = 10f;
    public float swingLimit = 10f;
}

public class JointManager : MonoBehaviour
{
    public Settings[] bones;
    public Settings settings;

    public HeadPositioning head;

    [Header("Bools")]
    public bool AngularMotionLimited = false;
    public bool twistLimited = false;
    public bool swingLimited = false;
    public bool UpdateStrenght = false;

    [Header("Strenght")]
    public float strenghtMultiplier = 1f;
    public float maxForce = 1000f;
    public float headForce = 1000f;

    private List<ConfigurableJoint> strenghtJoints = new List<ConfigurableJoint>();

    private void Awake()
    {
        strenghtJoints.AddRange(GetComponentsInChildren<ConfigurableJoint>());
        ApplyStrenght();
        RunSettings();

        twistLimited = true;
        swingLimited = true;
        AngularMotionLimited = true;
    }

    public void Update()
    {
        RunSettings();
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

            if (settings == null)
            {
                Debug.LogWarning("No settings found for joint: " + joint.gameObject.name);
                continue;
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
            drive.positionSpring = settings.spring * settings.strenghtMultiplier;
            drive.positionDamper = settings.damper * settings.strenghtMultiplier;
            drive.maximumForce = maxForce;

            head.upwardForce = headForce * strenghtMultiplier;
            head.upwardForce = Mathf.Clamp(head.upwardForce, 0, 1000);

            joint.slerpDrive = drive;

            joint.angularXDrive = drive;
            joint.angularYZDrive = drive;
        }
    }
}
