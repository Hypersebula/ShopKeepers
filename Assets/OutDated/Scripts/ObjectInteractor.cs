using UnityEngine;
using System.Collections.Generic;

public class ObjectInteractor : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform holdPoint;
    public LayerMask interactableLayer;

    [Header("Settings")]
    public float pickUpRange = 5f;
    public float followSpeed = 25f;
    public float throwForce = 10f;

    [Header("Keybinds")]
    public KeyCode selectKey = KeyCode.Mouse1; // Right Click
    public KeyCode interactKey = KeyCode.Mouse0; // Left Click

    [Header("Held Object Settings")]
    public float objectRotationPower = 10f; // Not used directly but kept for your logic

    [Header("Hold Distance Settings")]
    public float holdDistance = 2f; // Start distance
    public float minHoldDistance = 0.5f; // Hand-held state if reached
    public float maxHoldDistance = 5f;
    public float scrollSensitivity = 0.5f;

    [Header("Spawner Settings")]
    public List<SpawnerEntry> spawners = new List<SpawnerEntry>();

    private GameObject heldObject;
    private Rigidbody heldRb;
    private CharacterController characterController;
    private bool isSelectMode = false;
    private float lastClickTime;
    private float doubleClickThreshold = 0.25f;

    [System.Serializable]
    public class SpawnerEntry
    {
        public GameObject spawnerObject;
        public GameObject itemToSpawn;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;

            if (cameraTransform == null)
            {
                Debug.LogWarning("ObjectInteractor: Could not find cameraTransform at Start.");
            }
        }
    }

    void Update()
    {
        HandleInput();

        if (heldObject != null)
        {
            HandleScroll();
            MoveHeldObject();
        }
    }

    void HandleInput()
    {
        bool rightHeld = Input.GetKey(selectKey);

        if (rightHeld && !isSelectMode)
        {
            isSelectMode = true;
        }
        else if (!rightHeld && isSelectMode)
        {
            isSelectMode = false;
        }

        if (isSelectMode && Input.GetKeyDown(interactKey))
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            lastClickTime = Time.time;

            if (heldObject == null)
            {
                if (TrySpawnFromSpawner()) return;
                TryPickUp();
            }
            else
            {
                if (timeSinceLastClick <= doubleClickThreshold)
                {
                    ThrowHeldObject();
                }
                else
                {
                    PlaceHeldObject();
                }
            }
        }
    }

    void HandleScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            holdDistance += scroll * scrollSensitivity; // Inverted: add instead of subtract
            holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
        }
    }

    bool TrySpawnFromSpawner()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
        {
            foreach (var entry in spawners)
            {
                if (hit.collider.gameObject == entry.spawnerObject && entry.itemToSpawn != null)
                {
                    GameObject spawned = Instantiate(entry.itemToSpawn, holdPoint.position, Quaternion.identity);
                    PickUpObject(spawned);
                    return true;
                }
            }
        }
        return false;
    }

    void TryPickUp()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange, interactableLayer))
        {
            if (heldObject != null) return;

            foreach (var entry in spawners)
            {
                if (hit.collider.gameObject == entry.spawnerObject)
                {
                    GameObject spawned = Instantiate(entry.itemToSpawn, holdPoint.position, Quaternion.identity);
                    PickUpObject(spawned);
                    return;
                }
            }

            if (hit.rigidbody != null)
            {
                PickUpObject(hit.rigidbody.gameObject);
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldRb = obj.GetComponent<Rigidbody>();

        heldRb.useGravity = false;
        heldRb.isKinematic = false;

        Collider[] playerColliders = characterController.GetComponentsInChildren<Collider>();
        Collider[] objColliders = heldObject.GetComponentsInChildren<Collider>();

        foreach (var objCol in objColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(objCol, playerCol, true);
            }
        }

        holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
    }

    void MoveHeldObject()
    {
        if (heldRb == null) return;

        Vector3 targetPos = cameraTransform.position + cameraTransform.forward * holdDistance;

        if (holdDistance <= minHoldDistance + 0.01f)
        {
            // HAND STATE: instant snap, lock rotation
            heldObject.transform.position = targetPos;
            heldObject.transform.rotation = Quaternion.identity;
            heldRb.linearVelocity = Vector3.zero;
            heldRb.angularVelocity = Vector3.zero;
        }
        else
        {
            // TELEKINESIS: smooth follow & free rotation
            Vector3 forceDir = (targetPos - heldObject.transform.position);
            heldRb.linearVelocity = Vector3.Lerp(heldRb.linearVelocity, forceDir * followSpeed, Time.deltaTime * 10f);

            Vector3 currentEuler = heldObject.transform.rotation.eulerAngles;
            currentEuler.x = ClampAngle(currentEuler.x, -10f, 10f);
            currentEuler.y = ClampAngle(currentEuler.y, -20f, 20f);
            currentEuler.z = ClampAngle(currentEuler.z, -10f, 10f);
            heldObject.transform.rotation = Quaternion.Euler(currentEuler);
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return Mathf.Clamp(angle, min, max);
    }

    void PlaceHeldObject()
    {
        RestoreCollisions();
        heldRb.useGravity = true;
        heldObject = null;
        heldRb = null;
    }

    void ThrowHeldObject()
    {
        RestoreCollisions();
        heldRb.useGravity = true;
        heldRb.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);
        heldObject = null;
        heldRb = null;
    }

    void RestoreCollisions()
    {
        if (heldObject == null || characterController == null) return;

        Collider[] playerColliders = characterController.GetComponentsInChildren<Collider>();
        Collider[] objColliders = heldObject.GetComponentsInChildren<Collider>();

        foreach (var objCol in objColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(objCol, playerCol, false);
            }
        }
    }
}
