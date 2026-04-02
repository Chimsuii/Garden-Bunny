using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public GameObject collectEffect;



    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Collected!");
            Destroy(gameObject);
        }
    }
}