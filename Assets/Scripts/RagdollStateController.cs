using UnityEngine;
using System;

public class RagdollStateController : MonoBehaviour
{
    [Serializable]
    public class JointGroup
    {
        public string name;
        public ConfigurableJoint[] joints;
        [Range(0f, 1f)] public float multiplier = 1f;
    }

    public JointGroup[] groups;
    [Range(0f, 1f)] public float globalMultiplier = 1f;

    private float[][] originalSpring;
    private float[][] originalDamper;

    public ActiveRagdoll activeRagdoll;

    private float originalPositionGain;
    private float originalPositionDamping;
    private float originalAngularVelocityGain;

    private void Start()
    {
        originalSpring = new float[groups.Length][];
        originalDamper = new float[groups.Length][];

        originalPositionGain = activeRagdoll.positionGain;
        originalPositionDamping = activeRagdoll.positionDamping;
        originalAngularVelocityGain = activeRagdoll.angularVelocityGain;

        for (int g = 0; g < groups.Length; g++)
        {
            originalSpring[g] = new float[groups[g].joints.Length];
            originalDamper[g] = new float[groups[g].joints.Length];

            for (int j = 0; j < groups[g].joints.Length; j++)
            {
                originalSpring[g][j] = groups[g].joints[j].angularXDrive.positionSpring;
                originalDamper[g][j] = groups[g].joints[j].angularXDrive.positionDamper;
            }
        }
    }

    public void ApplyMultipliers()
    {
        activeRagdoll.positionGain = originalPositionGain * globalMultiplier;
        activeRagdoll.positionDamping = originalPositionDamping * globalMultiplier;
        activeRagdoll.angularVelocityGain = originalAngularVelocityGain * globalMultiplier;

        for (int g = 0; g < groups.Length; g++)
        {
            float combined = globalMultiplier * groups[g].multiplier;

            for (int j = 0; j < groups[g].joints.Length; j++)
            {
                float spring = originalSpring[g][j] * combined;
                float damper = originalDamper[g][j] * combined;

                JointDrive xDrive = new JointDrive
                {
                    positionSpring = spring,
                    positionDamper = damper,
                    maximumForce = float.MaxValue
                };

                JointDrive yzDrive = new JointDrive
                {
                    positionSpring = spring,
                    positionDamper = damper,
                    maximumForce = float.MaxValue
                };

                groups[g].joints[j].angularXDrive = xDrive;
                groups[g].joints[j].angularYZDrive = yzDrive;
            }
        }
    }

    private void OnValidate()
    {
        if (originalSpring == null) return;
        ApplyMultipliers();
    }
}