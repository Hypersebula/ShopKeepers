using UnityEngine;
using TMPro;

public class InteractionDetector : MonoBehaviour
{
    [Header("Raycast")]
    public float interactDistance = 3f;
    public LayerMask interactMask;
    public Camera cam;

    [Header("UI")]
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    [Header("References")]
    public Pickup rightHand;
    private Pickupable currentTarget;

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            Pickupable pickupable = hit.collider.GetComponent<Pickupable>();
            if (pickupable != null)
            {
                currentTarget = pickupable;
                interactionUI.SetActive(true);
                interactionText.text = "Pick up [E]";

                if (Input.GetKeyDown(KeyCode.E) && !rightHand.isHolding)
                    rightHand.PickUp(pickupable);

                return;
            }
        }

        currentTarget = null;
        interactionUI.SetActive(false);
    }
}