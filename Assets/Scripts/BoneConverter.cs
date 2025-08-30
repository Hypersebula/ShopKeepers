using UnityEngine;

[System.Serializable]
public class BoneConvert
{
    public string Name;
    public Transform GhostBone;
    public ConfigurableJoint Joint;
}

public class BoneConverter : MonoBehaviour
{
    public BoneConvert[] convert;

    private void Update()
    {
        
    }
}
