using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCSystem : MonoBehaviour
{
    public GameObject dialogueTemplate;
    public GameObject canvas;

    bool player_detection = false;

    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.E) && !PlayerController.dialogue)
        {
            Debug.Log("E PRESSED - START DIALOGUE");

            PlayerController.dialogue = true;

            canvas.SetActive(true);

            NewDialogue("Hi!");
            NewDialogue("I am Bill!");
            NewDialogue("Nice to see new faces around these parts!");
            NewDialogue("Make sure to stop by to chat sometime!");
            NewDialogue("Press E to close.");
        }
        else if (PlayerController.dialogue && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E PRESSED - END DIALOGUE");

            PlayerController.dialogue = false;

            // Clear dialogue
            foreach (Transform child in canvas.transform)
            {
                Destroy(child.gameObject);
            }

            canvas.SetActive(false);
        }
    }

    void NewDialogue(string text)
    {
        GameObject clone = Instantiate(dialogueTemplate, canvas.transform);

        RectTransform rect = clone.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI tmp = clone.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = text;

        clone.SetActive(true);
    }

    // 🔥 DEBUG VERSION
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SOMETHING ENTERED: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER IN RANGE");
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("SOMETHING LEFT: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER LEFT RANGE");
            player_detection = false;
        }
    }
}