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

    public HeadPositioning head;

    [Header("Bools")]
    public bool AngularMotionLimited = false;
    public bool twistLimited = false;
    public bool swingLimited = false;
    public bool UpdateStrenght = false;

    [Header("Strenght")]
    public float strenghtMultiplier = 1f;
    public float maxForce = 10000000f;
    public float headForce = 1000f;

    private List<ConfigurableJoint> strenghtJoints = new List<ConfigurableJoint>();

    private void Awake()
    {
        strenghtJoints.AddRange(GetComponentsInChildren<ConfigurableJoint>());
        ApplyStrenght();
        RunSettings();
    }

    public void Update()
    {
        if(UpdateStrenght)
        {
            RunSettings();
            ApplyStrenght();
        }
    }

    public void RunSettings()
    {
        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>();

        foreach (var joint in joints)
        {
            // Try to find a Settings with the same name as joint gameObject
            Settings boneSettings = null;
            foreach (var b in bones)
            {
                if (b.Name == joint.gameObject.name)
                {
                    boneSettings = b;
                    break;
                }
            }

            if (boneSettings == null)
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
            else
            {
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
                joint.angularZMotion = ConfigurableJointMotion.Free;
            }
            // Apply twist Limits
            if (twistLimited)
            {
                SoftJointLimit lowX = new SoftJointLimit { limit = -boneSettings.twistLimit };
                SoftJointLimit highX = new SoftJointLimit { limit = boneSettings.twistLimit };
                joint.lowAngularXLimit = lowX;
                joint.highAngularXLimit = highX;
            }
            // Apply swing Limits
            if (swingLimited)
            {
                SoftJointLimit swingY = new SoftJointLimit { limit = boneSettings.swingLimit };
                SoftJointLimit swingZ = new SoftJointLimit { limit = boneSettings.swingLimit };
                joint.angularYLimit = swingY;
                joint.angularZLimit = swingZ;
            }
        }
    }

    public void ApplyStrenght()
    {
        foreach (var joint in strenghtJoints)
        {
            Settings boneSettings = null;
            foreach (var b in bones)
            {
                if(b.Name == joint.gameObject.name)
                {
                    boneSettings = b;
                    break;
                }
            }

            if (boneSettings == null)
            {
                Debug.LogWarning("No settings found for joint (strenght);" + joint.gameObject.name);
                continue;
            }

            JointDrive drive = new JointDrive
            {
                positionSpring = boneSettings.spring * boneSettings.strenghtMultiplier * strenghtMultiplier,
                positionDamper = boneSettings.damper * boneSettings.strenghtMultiplier * strenghtMultiplier,
                maximumForce = maxForce
            };

            joint.slerpDrive = drive;
            joint.angularXDrive = drive;
            joint.angularYZDrive = drive;
        }

        // Apply head upward force once (not per-joint)
        if (head != null)
        {
            head.upwardForce = Mathf.Clamp(headForce * strenghtMultiplier, 0f, 1000f);
        }
    }
}
