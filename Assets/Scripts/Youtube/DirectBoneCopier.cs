using UnityEngine;
using System.Collections.Generic;

public class DirectBoneCopier : MonoBehaviour
{
    [System.Serializable]
    public struct BonePair
    {
        public Transform source;
        public Transform target;
    }

    [Header("Rig Roots (Optional - For Auto-Mapping)")]
    public Transform sourceRoot;
    public Transform targetRoot;

    [Header("Copy Settings")]
    public bool copyRotation = true;
    public bool copyPositionOnRootOnly = true;
    public bool copyPositionOnAllBones = false;

    [Header("Bone Pair Assignments")]
    public BonePair[] bonePairs;

    private void FixedUpdate()
    {
        if (bonePairs == null || bonePairs.Length == 0) return;

        for (int i = 0; i < bonePairs.Length; i++)
        {
            Transform src = bonePairs[i].source;
            Transform tgt = bonePairs[i].target;

            if (src == null || tgt == null) continue;

            // Copy rotation in local space to maintain hierarchy relative to parents
            if (copyRotation)
            {
                tgt.localRotation = src.localRotation;
            }

            // Copy position: Root bone (index 0) usually needs position matching, 
            // while child bones usually only need rotation to prevent mesh deformation.
            if (copyPositionOnAllBones || (i == 0 && copyPositionOnRootOnly))
            {
                tgt.localPosition = src.localPosition;
            }
        }
    }

    [ContextMenu("Auto Map Bones By Name")]
    public void AutoMapByName()
    {
        if (sourceRoot == null || targetRoot == null)
        {
            Debug.LogWarning("Assign both Source Root and Target Root before running Auto-Map.");
            return;
        }

        Transform[] sourceBones = sourceRoot.GetComponentsInChildren<Transform>();
        Transform[] targetBones = targetRoot.GetComponentsInChildren<Transform>();

        List<BonePair> pairs = new List<BonePair>();

        foreach (Transform src in sourceBones)
        {
            foreach (Transform tgt in targetBones)
            {
                if (src.name == tgt.name)
                {
                    pairs.Add(new BonePair { source = src, target = tgt });
                    break;
                }
            }
        }

        bonePairs = pairs.ToArray();
        Debug.Log($"Successfully mapped {bonePairs.Length} bone pairs by matching hierarchy names.");
    }
}