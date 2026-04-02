using UnityEngine;

/// <summary>
/// Keeps pushable objects grounded on floor surfaces while allowing normal physics interaction with walls and other objects.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GroundedPushable : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Ground Check Settings")]
    [Tooltip("Distance to check below the object for the floor.")]
    public float groundCheckDistance = 0.1f;

    [Tooltip("Force applied to keep object grounded.")]
    public float groundingStrength = 20f;

    [Tooltip("Layer(s) considered as floor.")]
    public LayerMask floorLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        // Raycast straight down from object center
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance + 0.5f, floorLayer))
        {
            float distanceToGround = hit.distance - GetComponent<Collider>().bounds.extents.y;

            if (distanceToGround > 0.001f)
            {
                // Apply gentle downward acceleration to keep it grounded
                rb.AddForce(Vector3.down * groundingStrength * distanceToGround, ForceMode.Acceleration);
            }
        }

        // Otherwise do nothing, let gravity act naturally
    }
}