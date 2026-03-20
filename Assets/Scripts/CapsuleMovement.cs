using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CapsuleMovement : MonoBehaviour
{
    public float groundDrag;

    public float jumpForce;
    public float airMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Ragdoll Authority")]
    public Rigidbody ragdollHips;
    public bool ragdollDriving = false;

    [Header("Grab Constraint")]
    public Grabbing leftHand;
    public Grabbing rightHand;
    public float maxGrabDistance = 1.5f;

    [Header("Sprinting")]
    private float moveSpeed;
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Leg IK")]
    public LegTarget leftLeg;
    public LegTarget rightLeg;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.SphereCast(transform.position, 0.25f, Vector3.down, out RaycastHit hit,playerHeight * 0.5f + 0f, whatIsGround);
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        if (ragdollDriving)
        {
            rb.MovePosition(ragdollHips.position);
            return;
        }

        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(sprintKey))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

        if (Input.GetKeyDown(jumpKey) && grounded)
            Jump();

        bool sprinting = Input.GetKey(sprintKey);
        moveSpeed = sprinting ? sprintSpeed : walkSpeed;
        leftLeg.isSprinting = sprinting;
        rightLeg.isSprinting = sprinting;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void ApplyGrabConstaint()
    {
        ConstraintToHand(leftHand);
        ConstraintToHand(rightHand);
    }

    private void ConstraintToHand(Grabbing hand)
    {
       
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
