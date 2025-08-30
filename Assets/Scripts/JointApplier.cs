using UnityEngine;

public class JointApplier : MonoBehaviour
{
    [Header("Reference to your MuscleProfile")]
    public JointSettings profile;

    [Header("Apply automatically on Start")]
    public bool apply = true;

    [Header("ApplyValueToAll")]
    public bool applyAll;
    public string Name;
    public float Value;

    void Start()
    {
        if (apply)
            ApplyJointSettings();
        apply = false;
    }

    private void Update()
    {
        if (applyAll)
        {
            ChangeValueOnAllBones(Name, Value);
        }

        if (apply)
        {
            ApplyJointSettings();
        }
    }

    /// <summary>
    /// Finds all joints under this armature and applies the settings from MuscleProfile
    /// </summary>
    public void ApplyJointSettings()
    {
        if (profile == null || profile.bones.Length == 0)
        {
            Debug.LogWarning("No MuscleProfile or bones found!");
            return;
        }

        // Get all ConfigurableJoints in children
        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>();

        foreach (var joint in joints)
        {
            // Try to find a BoneSettings with the same name as the joint's GameObject
            BoneSettings settings = null;
            foreach (var b in profile.bones)
            {
                if (b.boneName == joint.gameObject.name)
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

            // Apply angular limits
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit lowX = new SoftJointLimit { limit = -settings.twistLimit };
            SoftJointLimit highX = new SoftJointLimit { limit = settings.twistLimit };
            joint.lowAngularXLimit = lowX;
            joint.highAngularXLimit = highX;

            SoftJointLimit swingY = new SoftJointLimit { limit = settings.swingLimit };
            SoftJointLimit swingZ = new SoftJointLimit { limit = settings.swingLimit };
            joint.angularYLimit = swingY;
            joint.angularZLimit = swingZ;

            // Apply drive with maximumForce
            JointDrive drive = new JointDrive
            {
                positionSpring = settings.spring,
                positionDamper = settings.damper,
                maximumForce = settings.maxForce
            };
            joint.angularXDrive = drive;
            joint.angularYZDrive = drive;
        }

        Debug.Log("Joint settings applied to " + joints.Length + " joints.");
    }
    public void ChangeValueOnAllBones(string valueName, float value)
    {
        if (profile == null || profile.bones.Length == 0) return;

        foreach (var b in profile.bones)
        {
            switch (valueName.ToLower())
            {
                case "spring":
                    b.spring = value;
                    break;
                case "damper":
                    b.damper = value;
                    break;
                case "swinglimit":
                    b.swingLimit = value;
                    break;
                case "twistlimit":
                    b.twistLimit = value;
                    break;
                case "maxforce":
                    b.maxForce = value;
                    break;
                default:
                    Debug.LogWarning("Unknown value name: " + valueName);
                    break;
            }
        }

        // Re-apply to all joints immediately
        ApplyJointSettings();
    }
}
