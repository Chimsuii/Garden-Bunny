using UnityEngine;

public class itemCollection : MonoBehaviour
{
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private Collider itemCollider;
    private bool isCollected = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        itemCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // 1. Play the sound FIRST
            if (audioSource != null && audioSource.isActiveAndEnabled)
            {
                audioSource.Play();
            }

            // 2. Hide only the VISUALS and the HITBOX
            // We do NOT disable the whole GameObject or the AudioSource
            if (meshRenderer != null) meshRenderer.enabled = false;
            if (itemCollider != null) itemCollider.enabled = false;

            // 3. Wait for the sound to finish (e.g., 2 seconds) before destroying
            Destroy(gameObject, 2.0f);
        }
    }
}