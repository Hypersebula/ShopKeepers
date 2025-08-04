using UnityEngine;

public class SpawnerObjectScript : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefabToSpawn;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // Optional, if you want a specific position
    public Vector3 spawnOffset;

    void OnMouseDown()
    {
        if (prefabToSpawn != null)
        {
            Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position + spawnOffset;
            Instantiate(prefabToSpawn, position, Quaternion.identity);
        }
    }
}
