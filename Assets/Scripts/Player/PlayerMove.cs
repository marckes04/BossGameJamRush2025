using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerController playerAsset;
    private InputAction move;

    private Vector3 forceDirection; 

    private Rigidbody rb;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private float movementForce = 1f;

    [SerializeField]
    private float maxSpeed = 5f;

    private Animator animator;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing.");
        }
        playerAsset = new PlayerController();
  
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component is missing.");
        }
    }

    void OnEnable()
    {
       move =  playerAsset.PlayerControls.Move;
       playerAsset.PlayerControls.Enable();
    }

    void OnDisable()
    {
        playerAsset.PlayerControls.Disable();
    }

    void FixedUpdate()
    {
        Vector2 input = move.ReadValue<Vector2>();

        // Check for snap input (e.g., a button press)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SnapTo(new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0));
            return; // Skip further updates for this frame
        }

        // Normal movement logic
        forceDirection += input.x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += input.y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        UpdateAnimations(horizontalVelocity);
        LookAt();
    }


    private void UpdateAnimations(Vector3 horizontalVelocity)
    {
        // Calcula la velocidad horizontal
        float speed = horizontalVelocity.magnitude;

        // Actualiza el parámetro 'Speed' en el Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
    }


    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }


    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    // Method to snap the player's body to a specific position and/or rotation
    public void SnapTo(Vector3 targetPosition, Quaternion targetRotation)
    {
        // Snap the Rigidbody's position
        rb.position = targetPosition;

        // Snap the Rigidbody's rotation
        rb.rotation = targetRotation;

        // Optionally reset velocity to ensure no unintended movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Update the animator if needed
        if (animator != null)
        {
            animator.SetFloat("Speed", 0); // Stop animation since we're snapping to idle
        }
    }

}
