using UnityEngine;

public class PlayerMovementOld : MonoBehaviour
{
    public CharacterController controller;

    // Movement Values
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    // Stamina
    public float maxStamina = 10f;
    public float stamina;
    public float staminaDrainRate = 3f;
    public float staminaRegenRate = 2f;

    // Ground Check
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    // Crouching
    public float crouchHeight = 1f;
    private float originalHeight;
    private bool isCrouching;

    // Sprint regen delay
    public float sprintRegenDelay = 1f;
    private float sprintRegenCooldownTimer;

    // Smooth crouch transition
    public float crouchTransitionSpeed = 6f;
    private float targetHeight;

    // Velocity & Speed for Sound
    public float currentSpeed;

    Vector3 velocity;

    private void Start()
    {
        originalHeight = controller.height;
        stamina = maxStamina;
        targetHeight = controller.height;
    }

    private void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Sprint Logic
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching && move.magnitude > 0.1f && stamina > 0f;
        currentSpeed = walkSpeed;

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            stamina -= staminaDrainRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            sprintRegenCooldownTimer = sprintRegenDelay; // reset cooldown timer
        }
        else
        {
            if (sprintRegenCooldownTimer > 0)
            {
                sprintRegenCooldownTimer -= Time.deltaTime;
            }
            else
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
        }

        // Crouch Logic (toggle)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchHeight : originalHeight;
        }

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        // Move the character
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply smooth crouch height transition
        if (isCrouching)
        {
            // Smooth down transition for crouching
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        }
        else
        {
            // Faster transition when standing up (original height)
            controller.height = Mathf.MoveTowards(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed * 3); // increase speed when going back up
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
