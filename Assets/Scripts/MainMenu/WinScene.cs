using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    [Header("Text in Story")]
    //Showing text in story
    [SerializeField] private float wordDelay = 0.005f;
    [SerializeField] private float sentenceDelay = 0.1f;
    [SerializeField] private string text = "Finally, both of you blend together and you get filled with warmth. Both of your pieces are together again. You completed your mission. You are whole again.";

    private string currentText = "";
    private TextMeshProUGUI storyText;

    void Start()
    {
        storyText = GameObject.Find("StoryText").GetComponent<TextMeshProUGUI>();
        storyText.outlineWidth = 0.2f;
        storyText.outlineColor = new Color32(0, 0, 0, 255);
        AudioManager.instance.Spell();
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < text.Length; i++)
        {
            currentText = text.Substring(0, i);
            storyText.text = currentText;
            //pay sfx
            AudioManager.instance.MechanicalButton();

            if (text[i] != '.')
                yield return new WaitForSeconds(wordDelay);
            else
                yield return new WaitForSeconds(sentenceDelay);
        }
    }

    public void BackToMainMenu()
    {
        AudioManager.instance.Button();
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void GameExit()
    {
        AudioManager.instance.Button();
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
