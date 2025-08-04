using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public GameObject PlayerBody;

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
        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotateBodyToLookDirection();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    private void RotateBodyToLookDirection()
    {
        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput);

        // Only rotate if there's some input
        if (inputDir.sqrMagnitude > 0.01f)
        {
            // Get target Y rotation from orientation
            float targetY = orientation.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetY, 0f);

            // Smoothly rotate PlayerBody toward orientation
            float rotationSpeed = 720f; // degrees per second
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

}
