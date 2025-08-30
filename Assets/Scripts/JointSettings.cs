using UnityEngine;

[System.Serializable]
public class BoneSettings
{
    public string boneName;
    public float swingLimit = 30f;
    public float twistLimit = 10f;
    public float spring = 200f;
    public float damper = 20f;
    public float maxForce = Mathf.Infinity;
}

public class JointSettings : MonoBehaviour
{
    public BoneSettings[] bones; // Array of BoneSettings
}
