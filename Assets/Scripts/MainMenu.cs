using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private string startingScene;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    //Animator
    [SerializeField] private Animator storyScreenFade;
    [SerializeField] private Animator transition;


    [Header("Text in Story")]
    //Showing text in story
    [SerializeField] private float wordDelay = 0.05f;
    [SerializeField] private float sentenceDelay = 0.1f;
    [SerializeField] private float skipDelay = 0.001f;
    [Multiline(10)]
    [SerializeField] private string fullText;
    private string currentText = "";

    //Story init
    private Image storyIMG;
    private TextMeshProUGUI storyText;
    private Button skipButton;
    private bool skipping = false;

    //For Recording the trailer
    public bool record = false;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        //Story
        //storyIMG = GameObject.Find("StoryScreen").GetComponent<Image>();
        storyText = GameObject.Find("StoryText").GetComponent<TextMeshProUGUI>();
        skipButton = GameObject.Find("SkipButton").GetComponent<Button>();
        storyIMG.enabled = false;
        storyText.enabled = false;
        skipButton.enabled = false;
        skipButton.image.enabled = false;

        //Transition
        transition = GameObject.Find("TransitionPaneMainMenu").GetComponent<Animator>();

        if (record)
            StartStory();
    }

    public void GameStart()
    {
        Debug.Log("Start Story");
        StartStory();
    }

    public void StartStory()
    {
        //storyIMG.enabled = true;
        storyText.enabled = true;
        skipButton.enabled = true;
        //skipButton.image.enabled = true;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            storyText.text = currentText;

            if (!skipping && fullText[i] != '.')
                yield return new WaitForSeconds(wordDelay);
            else if (!skipping && fullText[i] == '.')
                yield return new WaitForSeconds(sentenceDelay);
            else
                yield return new WaitForSeconds(skipDelay);
        }
        //storyIMG.color = new Color32(255, 255, 255, 255);
        storyScreenFade.SetTrigger("Fade");
        yield return new WaitForSeconds(1.0f);
    }

    public void SkipButton()
    {
        if (skipping || currentText.Equals(""))
        {
            StartCoroutine(GoToMainGame());
        }
        else
        {
            skipping = true;
        }
    }

    IEnumerator GoToMainGame()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Start Main Game");
        SceneManager.LoadScene(startingScene);
    }

    public void GameInfo()
    {
        Debug.Log("Game Info");
    }


    public void GameExit()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

}
