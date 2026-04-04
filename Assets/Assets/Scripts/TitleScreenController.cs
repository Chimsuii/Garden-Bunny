using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [Header("Arrow Indicator")]
    public RectTransform arrow;
    public Vector3 offset;
    public float speed = 10f;

    private Vector3 targetPos;

    [Header("Buttons")]
    public RectTransform newGameButton;
    public RectTransform exitButton;

    void Start()
    {
        MoveArrow(newGameButton); // start at New Game
        arrow.position = targetPos; // prevent weird starting slide
    }

    void Update()
    {
        arrow.position = Vector3.Lerp(arrow.position, targetPos, Time.deltaTime * speed);
    }

    public void MoveArrow(RectTransform target)
    {
        targetPos = target.position + offset;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SlideshowScene");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}