using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    int index = 0;

    void Update()
    {
        // Click or press E to go next
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && transform.childCount > 0)
        {
            if (PlayerController.dialogue)
            {
                // Hide all first (optional but cleaner)
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                // Show current line
                if (index < transform.childCount)
                {
                    transform.GetChild(index).gameObject.SetActive(true);
                    index++;
                }

                // End dialogue when finished
                if (index >= transform.childCount)
                {
                    index = 0;
                    PlayerController.dialogue = false;

                    gameObject.SetActive(false); // hide whole dialogue UI
                }
            }
        }
    }
}