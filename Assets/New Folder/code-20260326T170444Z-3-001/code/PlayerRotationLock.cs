using UnityEngine;

/// <summary>
/// Prevents player Rigidbody from rotating uncontrollably on Y due to collisions,
/// but allows manual rotation from input.
/// Attach this alongside your PlayerController.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerRotationLock : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        // Keep the rotation strictly as it was after input
        Vector3 euler = transform.eulerAngles;

        // Freeze X and Z, keep Y as it is (manual rotation only)
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
}