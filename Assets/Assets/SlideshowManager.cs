using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SlideshowManager : MonoBehaviour
{
    [Header("Slides & Dialogue")]
    public Image slideImage;
    public TMP_Text dialogueText;
    public GameObject dialogueBox;

    public Sprite[] slides;
    public string[] dialogues;

    [Header("Settings")]
    public float imageFadeDuration = 0.5f;
    public AudioSource typeSound;

    private int currentIndex = 0;
    private bool showingDialogue = false;

    // Dialogue paging
    private string[] currentDialogueParts;
    private int dialoguePartIndex = 0;

    // Typing
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    // Fade
    private CanvasGroup slideCanvasGroup;
    private CanvasGroup dialogueCanvasGroup;

    void Start()
    {
        // Setup slide fade
        slideCanvasGroup = slideImage.GetComponent<CanvasGroup>();
        if (slideCanvasGroup == null)
            slideCanvasGroup = slideImage.gameObject.AddComponent<CanvasGroup>();

        slideCanvasGroup.alpha = 1;

        // Setup dialogue fade
        dialogueCanvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        if (dialogueCanvasGroup == null)
            dialogueCanvasGroup = dialogueBox.AddComponent<CanvasGroup>();

        dialogueCanvasGroup.alpha = 0;

        dialogueBox.SetActive(false);
        ShowImage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextStep();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SkipSlideshow();
        }
    }

    public void NextStep()
    {
        // If typing → instantly finish
        if (isTyping)
        {
            dialogueText.text = currentDialogueParts[dialoguePartIndex];
            isTyping = false;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            return;
        }

        // If more dialogue parts → go next part
        if (showingDialogue && dialoguePartIndex < currentDialogueParts.Length - 1)
        {
            dialoguePartIndex++;
            StartTyping();
            return;
        }

        // If not yet showing dialogue → show it
        if (!showingDialogue)
        {
            ShowDialogue();
        }
        else
        {
            currentIndex++;

            if (currentIndex >= slides.Length)
            {
                StartGame();
                return;
            }

            ShowImage();
        }
    }

    void ShowImage()
    {
        StartCoroutine(FadeImage(slides[currentIndex]));
        dialogueBox.SetActive(false);
        showingDialogue = false;
    }

    IEnumerator FadeImage(Sprite newSprite)
    {
        // Fade out
        for (float t = 1; t >= 0; t -= Time.deltaTime / imageFadeDuration)
        {
            slideCanvasGroup.alpha = t;
            yield return null;
        }

        slideImage.sprite = newSprite;

        // Fade in
        for (float t = 0; t <= 1; t += Time.deltaTime / imageFadeDuration)
        {
            slideCanvasGroup.alpha = t;
            yield return null;
        }
    }

    void ShowDialogue()
    {
        dialogueBox.SetActive(true);
        StartCoroutine(FadeDialogueIn());

        // Split dialogue using |
        currentDialogueParts = dialogues[currentIndex].Split('|');
        dialoguePartIndex = 0;

        StartTyping();
        showingDialogue = true;
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentDialogueParts[dialoguePartIndex]));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;

            if (typeSound != null)
                typeSound.Play();

            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    IEnumerator FadeDialogueIn()
    {
        for (float t = 0; t <= 1; t += Time.deltaTime / 0.3f)
        {
            dialogueCanvasGroup.alpha = t;
            yield return null;
        }
    }

    public void SkipSlideshow()
    {
        currentIndex = slides.Length - 1;
        ShowDialogue();
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}