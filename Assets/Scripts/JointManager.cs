using UnityEngine;

[System.Serializable]
public class Settings
{
    public string Name;

    public float Strenght = 0;
    public float spring = 0;
    public float damper = 0;

    public float twistLimit = 0;
    public float swingLimit = 0;
}

public class JointManager : MonoBehaviour
{
    public Settings[] bones;
    public Settings settings;

    [Header("Bools")]
    public bool AngularMotionLimited = false;
    public bool twistLimited = false;
    public bool swingLimited = false;
    public bool ApplyStrenght = false;

    public void RunSettings()
    {
        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>();

        foreach (var joint in joints)
        {
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

            // Apply Drive
            JointDrive drive = new JointDrive
            {
                positionSpring = settings.spring,
                positionDamper = settings.damper,
            };
            joint.rotationDriveMode = RotationDriveMode.XYAndZ;
            joint.angularXDrive = drive;
            joint.angularYZDrive = drive;

            // Apply Strenght
            

        }
    }
}
