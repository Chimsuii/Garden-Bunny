using System.Collections;
using UnityEngine;
using TMPro;

public class NPC_Dialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public TextMeshProUGUI dialogue_text;
    public string[] dialogue_lines;
    public float text_speed = 0.05f;

    private int dialogue_index;
    private bool is_typing = false;

    [Header("Player Detection")]
    public bool player_detection = false;

    void Start()
    {
        dialogue_text.text = "";
        gameObject.SetActive(false); // hidden at start
    }

    void Update()
    {
        // Press E to interact (change if you want)
        if (player_detection && Input.GetKeyDown(KeyCode.E))
        {
            if (!gameObject.activeSelf)
            {
                StartDialogue();
            }
            else
            {
                HandleNextLine();
            }
        }
    }

    void StartDialogue()
    {
        gameObject.SetActive(true);
        dialogue_index = 0;
        dialogue_text.text = "";
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        is_typing = true;

        foreach (char c in dialogue_lines[dialogue_index].ToCharArray())
        {
            dialogue_text.text += c;
            yield return new WaitForSeconds(text_speed);
        }

        is_typing = false;
    }

    void HandleNextLine()
    {
        if (is_typing)
        {
            StopAllCoroutines();
            dialogue_text.text = dialogue_lines[dialogue_index];
            is_typing = false;
        }
        else
        {
            NextLine();
        }
    }

    void NextLine()
    {
        if (dialogue_index < dialogue_lines.Length - 1)
        {
            dialogue_index++;
            dialogue_text.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        gameObject.SetActive(false);
    }

    // 🔥 YOUR DETECTION (from your code)
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
        if (other.CompareTag("Player"))
        {
            player_detection = false;
        }
    }
}