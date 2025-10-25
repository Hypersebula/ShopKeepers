using UnityEngine;
using System.Collections;

public class ActiveRagdoll : MonoBehaviour
{
    public Transform[] animated;
    public ConfigurableJoint[] joints;
    private Quaternion[] startPos;

    void Start()
    {
        startPos = new Quaternion[joints.Length];

        for (int i = 0; i < joints.Length; i++)
        {
            startPos[i] = joints[i].transform.localRotation;
        }
    }

    void Update()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].SetTargetRotationLocal(animated[i].localRotation, startPos[i]);
        }
    }
}
