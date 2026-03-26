using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SlideshowManager : MonoBehaviour
{
    public Image slideImage;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    public Sprite[] slides;
    public string[] dialogues;

    private int currentIndex = 0;
    private bool showingDialogue = false;

    void Start()
    {
        ShowImageOnly();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            NextStep();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed - skipping");
            SkipSlideshow();
        }
    }

    public void NextStep()
    {
        if (!showingDialogue)
        {
            StartCoroutine(ShowDialogue());
        }
        else
        {
            currentIndex++;

            if (currentIndex >= slides.Length)
            {
                StartGame();
            }
            else
            {
                ShowImageOnly();
            }
        }
    }

    void ShowImageOnly()
    {
        slideImage.sprite = slides[currentIndex];
        dialogueBox.SetActive(false);
        showingDialogue = false;
    }

    IEnumerator ShowDialogue()
    {
        dialogueBox.SetActive(true);
        dialogueText.text = "";

        string fullText = dialogues[currentIndex];

        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueText.text += fullText[i];
            yield return new WaitForSeconds(0.08f); // typing speed
        }

        showingDialogue = true;
    }

    public void SkipSlideshow()
    {
        StartGame();
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}