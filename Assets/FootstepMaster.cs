using UnityEngine;

public class FootstepMaster : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource source;
    public AudioClip[] solidSounds;
    public AudioClip[] grassSounds;

    [Header("Settings")]
    public float stepInterval = 0.5f;
    private float timer;
    
    private Rigidbody rb;
    private string currentSurface = "None";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (source == null) source = GetComponent<AudioSource>();
    }

    void Update()
    {
      
        if (rb.linearVelocity.magnitude > 0.1f && currentSurface != "None")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                PlayStep();
                timer = stepInterval;
            }
        }
        else
        {
            timer = 0; 
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("SolidGround"))
        {
            currentSurface = "SolidGround";
        }
        else if (col.gameObject.CompareTag("Grass"))
        {
            currentSurface = "Grass";
        }
    }

    void OnCollisionExit(Collision col)
    {
        currentSurface = "None";
    }

    void PlayStep()
    {
        AudioClip clip = null;

        if (currentSurface == "SolidGround")
        {
            if (solidSounds.Length > 0)
                clip = solidSounds[Random.Range(0, solidSounds.Length)];
        }
        else if (currentSurface == "Grass")
        {
            if (grassSounds.Length > 0)
                clip = grassSounds[Random.Range(0, grassSounds.Length)];
        }

        if (clip != null)
        {
            source.pitch = Random.Range(0.85f, 1.15f); 
            source.PlayOneShot(clip, 0.6f);
            Debug.Log("Playing: " + currentSurface + " sound");
        }
    }
}