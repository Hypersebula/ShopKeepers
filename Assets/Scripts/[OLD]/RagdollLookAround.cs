using UnityEngine;

public class RagdollLookAround : MonoBehaviour
{
    public float sensitivity = 1f;

    public float minLook = -90f;
    public float maxLook = 90f;
    public float currentLook = 0f;

    public ConfigurableJoint headBone;
    Quaternion headDown;
    Quaternion headUp;
    public ConfigurableJoint neckBone;
    Quaternion neckDown;
    Quaternion neckUp;
    public ConfigurableJoint chestBone;
    Quaternion chestDown;
    Quaternion chestUp;
    public ConfigurableJoint spineBone;
    Quaternion spineDown;
    Quaternion spineUp;

    private void Start()
    {
        Quaternion headBase = headBone.transform.localRotation;
        headDown = ToJointSpace(headBone, headBase * Quaternion.Euler(90f, 0f, 0f));
        headUp = ToJointSpace(headBone, headBase * Quaternion.Euler(-90f, 0f, 0f));

        Quaternion neckBase = neckBone.transform.localRotation;
        neckDown = ToJointSpace(neckBone, neckBase * Quaternion.Euler(90f, 0f, 0f));
        neckUp = ToJointSpace(neckBone, neckBase * Quaternion.Euler(-90f, 0f, 0f));

        Quaternion chestBase = chestBone.transform.localRotation;
        chestDown = ToJointSpace(chestBone, chestBase * Quaternion.Euler(45f, 0f, 0f));
        chestUp = ToJointSpace(chestBone, chestBase * Quaternion.Euler(-45f, 0f, 0f));

        Quaternion spineBase = spineBone.transform.localRotation;
        spineDown = ToJointSpace(spineBone, spineBase * Quaternion.Euler(15f, 0f, 0f));
        spineUp = ToJointSpace(spineBone, spineBase * Quaternion.Euler(-15f, 0f, 0f));
    }

    private void Update()
    {
        // Mouse movement
        float mouseY = Input.GetAxis("Mouse Y");
        currentLook -= mouseY * sensitivity;
        currentLook = Mathf.Clamp(currentLook, minLook, maxLook);

        float t = Mathf.InverseLerp(minLook, maxLook, currentLook);

        headBone.targetRotation = Quaternion.Slerp(headDown, headUp, t);

        neckBone.targetRotation = Quaternion.Slerp(neckDown, neckUp, t);

        chestBone.targetRotation = Quaternion.Slerp(chestDown, chestUp, t);

        spineBone.targetRotation = Quaternion.Slerp(spineDown, spineUp, t);
    }

    Quaternion ToJointSpace(ConfigurableJoint joint, Quaternion localRotation)
    {
        // converts a desired bone rotation into joint.targetRotation space
        return Quaternion.Inverse(joint.transform.localRotation) * localRotation;
    }
}
