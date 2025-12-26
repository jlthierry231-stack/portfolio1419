using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public float cameraSmoothSpeed = 5f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
    }

    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * vertical + right * horizontal).normalized;

        // Apply gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // Move the character
        Vector3 movement = moveDirection * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(movement * Time.deltaTime);

        // Rotate character to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleCamera()
    {
        // Calculate desired camera position
        Vector3 desiredPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;

        // Smooth camera movement
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);

        // Make camera look at player
        cameraTransform.LookAt(transform.position + Vector3.up * cameraHeight * 0.5f);
    }
}