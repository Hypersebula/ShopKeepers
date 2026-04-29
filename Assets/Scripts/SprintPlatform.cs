using UnityEngine;

public class SprintPlatform : MonoBehaviour
{
    [Header("References")]
    public Renderer platformRenderer;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public GameObject destroyOnSuccess1;
    public GameObject destroyOnSuccess2;

    [Header("Materials")]
    public Material blueMaterial;
    public Material greenMaterial;
    public Material redMaterial;
    public Material defaultMaterial;

    private bool playerOnPlatform = false;
    private bool failed = false;
    private bool completed = false;

    private void Update()
    {
        if (!playerOnPlatform || completed) return;

        if (!Input.GetKey(sprintKey))
        {
            failed = true;
            platformRenderer.material = redMaterial;
        }
        else if (!failed)
        {
            platformRenderer.material = blueMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerOnPlatform = true;
        failed = false;
        platformRenderer.material = blueMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerOnPlatform = false;

        if (!failed && !completed)
        {
            completed = true;
            platformRenderer.material = greenMaterial;
            if (destroyOnSuccess1 != null) Destroy(destroyOnSuccess1);
            if (destroyOnSuccess2 != null) Destroy(destroyOnSuccess2);
        }
        else if (!completed)
        {
            platformRenderer.material = defaultMaterial;
            failed = false;
        }
    }
}