using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startingScene;

    //Animator
    [SerializeField] private Animator storyScreenFade;
    [SerializeField] private Animator transition;


    [Header("Text in Story")]
    //Showing text in story
    [SerializeField] private float wordDelay = 0.005f;
    [SerializeField] private float sentenceDelay = 0.1f;
    [SerializeField] private float skipDelay = 0.000001f;
    [Multiline(3)]
    [SerializeField] private string slide01Text;
    [SerializeField] private Sprite slide01;
    [Multiline(3)]
    [SerializeField] private string slide02Text;
    [SerializeField] private Sprite slide02;
    [Multiline(3)]
    [SerializeField] private string slide03Text;
    [SerializeField] private Sprite slide03;
    [Multiline(3)]
    [SerializeField] private string slide04Text;
    [SerializeField] private Sprite slide04;
    [Multiline(3)]
    [SerializeField] private string slide05Text;
    [SerializeField] private Sprite slide05;
    private string currentText = "";
    [SerializeField] private int currentScreen = 1;


    [Header("Text (for Shadows)")]
    [SerializeField] private List<TextMeshProUGUI> texts;

    //Init Menu
    private GameObject menu;
    private GameObject info;

    //Story init
    private Image storyIMG;
    private TextMeshProUGUI storyText;
    private Button skipButton;
    private bool skipping = false;
    private bool textDisplayed = false;

    void Start()
    {
        //Story
        menu = GameObject.Find("Menu");
        info = GameObject.Find("Info");
        storyIMG = GameObject.Find("StoryScreen").GetComponent<Image>();
        storyText = GameObject.Find("StoryText").GetComponent<TextMeshProUGUI>();
        skipButton = GameObject.Find("SkipButton").GetComponent<Button>();
        info.SetActive(false);
        storyIMG.enabled = false;
        storyText.enabled = false;
        skipButton.enabled = false;
        skipButton.image.enabled = false;

        //Transition
        storyScreenFade = GameObject.Find("StoryScreen").GetComponent<Animator>();
        storyScreenFade.enabled = false;
        transition = GameObject.Find("TransitionPaneMainMenu").GetComponent<Animator>();
        transition.enabled = false;

        //Adding shadows for texts
        foreach(TextMeshProUGUI text in texts)
        {
            text.outlineWidth = 0.1f;
            text.outlineColor = new Color32(0, 0, 0, 255);
        }
    }

    public void GameStart()
    {
        menu.SetActive(false);
        AudioManager.instance.Button();
        Debug.Log("Start Story");
        StartStory();
    }

    public void StartStory()
    {
        storyIMG.enabled = true;
        storyText.enabled = true;
        storyText.outlineWidth = 0.2f;
        storyText.outlineColor = new Color32(0, 0, 0, 255);
        skipButton.enabled = true;
        skipButton.image.enabled = true;
        storyScreenFade.enabled = true;
        AudioManager.instance.StartStoryMusic();
        StartCoroutine(FadeScreen());
        StartCoroutine(ShowText(slide01Text));
    }

    IEnumerator ShowText(string fullText)
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            storyText.text = currentText;
            //pay sfx
            AudioManager.instance.MechanicalButton();

            if (!skipping && fullText[i] != '.')
                yield return new WaitForSeconds(wordDelay);
            else if (!skipping && fullText[i] == '.')
                yield return new WaitForSeconds(sentenceDelay);
            else
                yield return new WaitForSeconds(skipDelay);
        }
        textDisplayed = true;
        //storyIMG.color = new Color32(255, 255, 255, 255);
    }

    IEnumerator FadeScreen()
    {
        Debug.Log("Start fading");
        storyScreenFade.SetTrigger("Fade");
        yield return new WaitForSeconds(1.0f);
        storyScreenFade.ResetTrigger("Fade");
    }

    private void ChangeStoryScreen()
    {
        currentScreen++;
        skipping = false;
        textDisplayed = false;
        storyText.text = " ";
        currentText = " ";
        switch (currentScreen)
        {
            case 2:
                storyIMG.sprite = slide02;
                StartCoroutine(ShowText(slide02Text));
                break;
            case 3:
                storyIMG.sprite = slide03;
                StartCoroutine(ShowText(slide03Text));
                StartCoroutine(FadeScreen());
                break;
            case 4:
                StartCoroutine(FadeScreen());
                AudioManager.instance.Spell();
                storyIMG.sprite = slide04;
                StartCoroutine(ShowText(slide04Text));
                break;
            case 5:
                storyIMG.sprite = slide05;
                StartCoroutine(ShowText(slide05Text));
                break;
            case 6: 
            default: 
                StartCoroutine(GoToMainGame());
                break;
        }
    }

    public void SkipButton()
    {
        if (skipping || textDisplayed)
        {
            AudioManager.instance.Button();
            ChangeStoryScreen();
        }
        else
        {
            skipping = true;
        }
    }

    IEnumerator GoToMainGame()
    {
        transition.enabled = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Start Main Game");
        SceneManager.LoadScene(startingScene);
    }

    public void GameInfo()
    {
        AudioManager.instance.Button();
        info.SetActive(true);
        Debug.Log("Game Info");
    }

    public void CloseGameInfo()
    {
        AudioManager.instance.Button();
        info.SetActive(false);
        Debug.Log("Close Game Info");
    }
    public void GameExit()
    {
        AudioManager.instance.Button();
        Debug.Log("Exit Game");
        Application.Quit();
    }

}
