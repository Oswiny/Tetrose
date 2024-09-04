using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playFieldBG;
    public GameObject scriptRunner;
    public GameObject deathMenuObject;
    public GameObject escapeMenuScriptRunner;
    ModeSettings modeSettings;
    ClearingAndPoints clearingAndPoints;
    
    public TMP_Text scoreOfThisSession;
    public TMP_Text scoreOfAllTime;
    public TMP_Text timeSpent;
    public TMP_Text pointsPerSecond;

    float timeLimit;
    float lineLimit;


    void Start()
    {
        //Debug.Log(playFieldBG.transform.position.y + playFieldBG.transform.localScale.y/2);
        clearingAndPoints = FindObjectOfType<ClearingAndPoints>();

        modeSettings = FindObjectOfType<ModeSettings>();
        timeLimit = modeSettings.timeLimit;
        lineLimit = modeSettings.lineLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(playFieldBG.transform.position.y + playFieldBG.transform.localScale.y / 2);
        }
    }

    public void checkYLimit(GameObject piece)
    {
        float upperLimit = playFieldBG.transform.position.y + playFieldBG.transform.localScale.y / 2;
        List<float> yPoses = new List<float>();
        foreach(Transform cube in piece.transform)
        {
            yPoses.Add(cube.position.y);
        }
        if (yPoses.Min() > upperLimit)
        {
            Debug.Log("DEATH HAS BEEN DETECTED" + " " + yPoses.Min() + " " +  upperLimit);
            onDeath();
        }
    }

    public void checkTimeLimit(float secondsSpent)
    {
        if (secondsSpent >= timeLimit && timeLimit != 0)
        {
            Debug.Log("DEATH HAS BEEN DETECTED TIME");
            onDeath();
        }
    }

    public void checkLineLimit(float lineAmount)
    {
        if(lineAmount >= lineLimit && lineLimit != 0)
        {
            Debug.Log("DEATH HAS BEEN DETECTED BY LINE");
            onDeath();
        }
    }

    void onDeath()
    {
        scoreOfThisSession.text = clearingAndPoints.pointsText.text;
        scriptRunner.SetActive(false);
        escapeMenuScriptRunner.SetActive(false);
        deathMenu();
        scoreOfAllTime.text = ScoreStoring.getHighScore(ModeSettings.selectedMode).ToString();
        EscapeMenu.isPaused = true;
    }

    void deathMenu()
    {
        deathMenuObject.SetActive(true);
    }

    public void returnToMenu()
    {
        Movement.pieceObjects.Clear();
        ScoreStoring.storeScore(ModeSettings.selectedMode, Convert.ToInt32(scoreOfThisSession.text));
        EscapeMenu.isPaused = false;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void retry()
    {
        Movement.pieceObjects.Clear();
        ScoreStoring.storeScore(ModeSettings.selectedMode, Convert.ToInt32(scoreOfThisSession.text));
        EscapeMenu.isPaused = false;
        SceneManager.LoadSceneAsync("PlayScene");
    }
}
