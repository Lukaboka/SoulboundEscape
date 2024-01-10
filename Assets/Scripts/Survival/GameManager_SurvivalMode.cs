using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_SurvivalMode : MonoBehaviour
{
    [SerializeField] private GameObject[] phaseAnnouncements;
    [SerializeField] private GameObject[] levels;

    [SerializeField] private GameObject _loseCondition;
    [SerializeField] private GameObject _winCondition;

    [SerializeField] private int levelID = -1;

    private void Start()
    {
        NextLevel();
    }

    private void NextLevel()
    {
        levelID = (levelID + 1) % 8;

        phaseAnnouncements[levelID].SetActive(true);
        levels[levelID].SetActive(true);
    }

    public void WinLevel()
    {
        levels[levelID].SetActive(false);
        phaseAnnouncements[levelID].SetActive(false);

        _winCondition.SetActive(true);
        Invoke("LoseAllTheUI", 2f);

        NextLevel();
    }

    private void LoseAllTheUI()
    {
        _winCondition.SetActive(false);
    }

    public void LoseLevel()
    {
        levelID = -1;
        _loseCondition.SetActive(true);
    }

    public void RetryButton()
    {
        _loseCondition.SetActive(false);
        NextLevel();
    }
}
