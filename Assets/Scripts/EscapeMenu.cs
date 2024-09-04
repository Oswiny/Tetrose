using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EscapeMenu : MonoBehaviour
{

    public TMP_Text highScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text stateGameText;
    public GameObject escapeObject;
    public GameObject confirmObject;
    public GameObject scriptObject;

    ClearingAndPoints clearingAndPoints;
    ScoreStoring scoreStoring;

    // Start is called before the first frame update
    void Start()
    {
        clearingAndPoints = FindObjectOfType<ClearingAndPoints>();
        scoreStoring = FindObjectOfType<ScoreStoring>();
    }


    public static bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                escapeObject.SetActive(true);
                isPaused = true;
                scriptObject.SetActive(false);
            }
            else
            {
                escapeObject.SetActive(false);
                isPaused = false;
                scriptObject.SetActive(true);
            }

            string gameMode = ModeSettings.selectedMode;
            currentScoreText.text = clearingAndPoints.pointsText.text;
            highScoreText.text = ScoreStoring.getHighScore(gameMode).ToString();
            stateGameText.text = "PAUSED";
        }
    }

    public void continueGame()
    {
        escapeObject.SetActive(false);
        isPaused = false;
    }

    public void confirmMenuReturn()
    {
        escapeObject.SetActive(false);
        confirmObject.SetActive(true);
    }

    public void cancelConfirmMenuReturn()
    {
        escapeObject.SetActive(true);
        confirmObject.SetActive(false);
    }

    
}
