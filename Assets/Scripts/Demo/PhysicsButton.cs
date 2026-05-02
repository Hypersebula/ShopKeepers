using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [Header("Button Settings")]
    public Transform buttonTop;
    private float upY = 0f;
    private float downY = -0.05f;
    public float springStrength = 10f;
    public float pressThreshold = 0.8f;
    public float travelDistance = 0.05f;

    [Header("Actions")]
    public GameObject destroyObject1;
    public GameObject destroyObject2;
    public GameObject unhideObject;
    public GameObject capsule;
    public GameObject ragdoll;
    public Transform targetLocation;
    public float EnableTime = 1f;
    public RagdollStateController ragdollStateController;
    public HealthManager healthManager;

    [Header("Custom Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    [Header("Spawn")]
    public GameObject spawnPrefab;
    public Transform spawnLocation;

    [Header("Player Only")]
    public bool playerOnly = false;

    [Header("Material Change")]
    public Renderer materialRenderer;
    public Material pressedMaterial;
    private Material originalMaterial;

    [Header("Multi Button Condition")]
    public PhysicsButton conditionButton1;
    public PhysicsButton conditionButton2;
    public PhysicsButton conditionButton3;
    public GameObject conditionDisable1;
    public GameObject conditionDisable2;
    public GameObject conditionDisable3;

    public bool isPressed = false;
    private float currentY;

    public Vector3 pressDirection = Vector3.down;


    private float upTravel = 0f;
    private float downTravel;
    private float currentTravel;

    [Header("Pressable")]
    public bool isPressable = true;
    public PhysicsButton button;

    private Vector3 originalLocalPosition;

    private void Start()
    {
        originalLocalPosition = buttonTop.localPosition;
        downTravel = travelDistance;
        currentTravel = upTravel;
        if (materialRenderer != null)
            originalMaterial = materialRenderer.material;
    }

    private void FixedUpdate()
    {
        currentTravel = Mathf.Lerp(currentTravel, upTravel, Time.fixedDeltaTime * springStrength);
        buttonTop.localPosition = originalLocalPosition + pressDirection.normalized * currentTravel;

        float pressAmount = Mathf.InverseLerp(upTravel, downTravel, currentTravel);

        if (!isPressed && pressAmount >= pressThreshold)
        {
            isPressed = true;
            OnButtonPressed();
        }
        else if (isPressed && pressAmount < pressThreshold * 0.5f)
        {
            isPressed = false;
            if (materialRenderer != null && originalMaterial != null)
                materialRenderer.material = originalMaterial;
            onReleased.Invoke();
            button.SetPressable(false);
        }

        if (conditionButton1 != null && conditionButton2 != null && conditionButton3 != null)
        {
            bool allPressed = conditionButton1.isPressable && conditionButton2.isPressable && conditionButton3.isPressable;
            Debug.Log($"allPressed: {allPressed} isPressed: {isPressed} b1: {conditionButton1.isPressed} b2: {conditionButton2.isPressed} b3: {conditionButton3.isPressed}");

            bool shouldDisable = allPressed && isPressed;

            if (conditionDisable1 != null) conditionDisable1.SetActive(!shouldDisable);
            if (conditionDisable2 != null) conditionDisable2.SetActive(!shouldDisable);
            if (conditionDisable3 != null) conditionDisable3.SetActive(!shouldDisable);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger hit by: " + other.gameObject.name + " isPressable: " + isPressable);
        if (!isPressable) return;
        if (playerOnly && !other.CompareTag("Player")) return;
        Rigidbody rb = other.attachedRigidbody;
        Debug.Log("Rigidbody: " + rb);
        if (rb == null) return;
        float force = rb.mass * Physics.gravity.magnitude;
        currentTravel = Mathf.MoveTowards(currentTravel, downTravel, force * Time.fixedDeltaTime * 0.1f);
    }

    private void OnButtonPressed()
    {
        onPressed.Invoke();
        if (destroyObject1 != null) Destroy(destroyObject1);
        if (destroyObject2 != null) Destroy(destroyObject2);
        if (unhideObject != null) unhideObject.SetActive(true);
        if (capsule != null && ragdoll != null && targetLocation != null)
            StartCoroutine(Teleport());
        if (spawnPrefab != null && spawnLocation != null)
            Instantiate(spawnPrefab, spawnLocation.position, spawnLocation.rotation);
        if (materialRenderer != null && pressedMaterial != null)
            materialRenderer.material = pressedMaterial;
        if (button != null)
            button.SetPressable(true);
    }

    public System.Collections.IEnumerator Teleport()
    {
        ragdollStateController.globalMultiplier = 0f;
        ragdollStateController.ApplyMultipliers();

        foreach (Collider col in ragdoll.GetComponentsInChildren<Collider>())
            col.enabled = false;

        capsule.SetActive(false);

        yield return null;

        capsule.transform.position = targetLocation.position;

        capsule.SetActive(true);

        if (healthManager != null)
        {
            healthManager.isDead = false;
            healthManager.currentHealth = healthManager.maxHealth;
            ragdollStateController.globalMultiplier = 1f;
        }

        StartCoroutine(ragdollStateController.LerpMultiplier(1f, 0.5f));

        yield return new WaitForSeconds(2f);

        foreach (Collider col in ragdoll.GetComponentsInChildren<Collider>())
            col.enabled = true;
    }

    public void SetPressable(bool value)
    {
        isPressable = value;
    }
}