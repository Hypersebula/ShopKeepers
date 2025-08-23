using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;

    public float walkSpeed = 5f;
    public float sprintSpeed = 7.5f;
    public float Stamina = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float groundDistance = 0.4f;

    public Transform groundCheck;
    public Transform head;
    
    public LayerMask groundMask;

    Vector3 velocity;

    public bool isGrounded;
    public bool isSprinting;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // WASD Input
        float horizontal = Input.GetAxis("Horizontal"); // Left Right
        float vertical = Input.GetAxis("Vertical"); // Up Down

        // Use Head Orientation
        Vector3 forward = head.forward;
        Vector3 right = head.right;

        // Flatten movement to XZ
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Combine into movement Vector (relative to player orientation)
        Vector3 move = right * horizontal + forward * vertical;

        //Sprinting
        float currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
        {
            currentSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            currentSpeed = walkSpeed;
            isSprinting = false;
        }

        // Stamina Drain
        if(isSprinting)
        {
            Stamina = Stamina - Time.deltaTime;
        }
        else if(!isSprinting)
        {
            Stamina = Stamina + Time.deltaTime;
        }

        Stamina = Mathf.Clamp(Stamina, 0, 10);

        // Apply movement through CharacterController
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
