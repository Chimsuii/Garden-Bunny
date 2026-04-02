using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float runMultiplier = 1.8f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;

    [Header("Mouse Look")]
    public Transform cameraPivot;
    public float mouseSensitivity = 2f;

    [Header("Push Settings")]
    public float pushStrength = 3f;
    public float maxPushSpeed = 5f;

    // ✅ Dialogue flag (GLOBAL)
    public static bool dialogue = false;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float inputX;
    private float inputY;

    private bool isGrounded;
    private bool jumpRequested;

    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 🛑 STOP EVERYTHING during dialogue
        if (dialogue)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // 🔒 Lock cursor normally
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }

        // Mouse look
        HandleMouseLook();

        // ESC toggle (optional)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // 🛑 STOP MOVEMENT during dialogue
        if (dialogue)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        inputX = 0f;
        inputY = 0f;

        // Movement input
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputY = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputY = -1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputX = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputX = 1f;

        Vector2 inputVec = new Vector2(inputX, inputY);
        inputVec = Vector2.ClampMagnitude(inputVec, 1f);

        moveDirection = transform.forward * inputVec.y + transform.right * inputVec.x;

        // ✅ Sprint check (Shift)
        bool isRunning = (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
                         && moveDirection.sqrMagnitude > 0.01f;

        float currentSpeed = isRunning ? speed * runMultiplier : speed;

        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);

        // Jump
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        jumpRequested = false;

        // Better falling
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        }
    }

    private void HandleMouseLook()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseDelta.x, 0f));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
            }
        }

        Rigidbody hitRb = collision.rigidbody;

        if (hitRb != null && !hitRb.isKinematic)
        {
            if (moveDirection.sqrMagnitude > 0.001f)
            {
                Vector3 pushDir = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
                Vector3 desiredVelocity = pushDir * pushStrength;

                if (hitRb.linearVelocity.magnitude < maxPushSpeed)
                {
                    hitRb.linearVelocity += desiredVelocity * Time.fixedDeltaTime;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}