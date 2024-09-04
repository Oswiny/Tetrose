using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ClearingAndPoints : MonoBehaviour
{
    public TMP_Text pointsText;
    public TMP_Text levelText;
    public TMP_Text timeText;
    public TMP_Text lineText;

    int totalClearedLines = 0;
    public int currentLevel = 1;



    public ModeSettings modeSettings;
    Death death;

    public float currentTimeSpent;
    float timeLimit;
    float lineLimit;

    // Start is called before the first frame update
    List<GameObject> pieceObjects;
    void Start()
    {
        Movement movement = new Movement();
        pieceObjects = movement.getPieceObjects();


        modeSettings = FindObjectOfType<ModeSettings>();
        death = FindObjectOfType<Death>();


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            printDict();
        }

        currentTimeSpent += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTimeSpent);
        
        timeText.text = time.ToString("mm':'ss");
        death.checkTimeLimit(time.Seconds);
        
    }

    public void Clearing()
    {

        pieceObjectsToDict();
        checkForClearableLines();
    }



    void printDict()
    {
        for (int i = 0; i < pieceDict.Count; i++)
        {
            Debug.Log("Float yPos = " + pieceDict.Keys.ToList()[i] + "Game Object Count " + pieceDict[pieceDict.Keys.ToList()[i]].Count);
        }
    }

    Dictionary<float, List<GameObject>> pieceDict;
    void pieceObjectsToDict()
    {
        pieceDict = new Dictionary<float, List<GameObject>>();
        foreach (GameObject obj in pieceObjects)
        {
            if (checkIfAHasAnyB(obj))
            {
                pieceDict[roundOneAfterDot(obj.transform.position.y)].Add(obj);
            }
            else
            {
                pieceDict.Add(roundOneAfterDot(obj.transform.position.y), new List<GameObject>() { obj });
            }
        }
    }

    float roundOneAfterDot(float number)
    {
        return (float)Math.Round((double)number, 1);
    }

    bool checkIfAHasAnyB(GameObject obj)
    {
        foreach (float key in pieceDict.Keys)
        {
            if (roundOneAfterDot(key) == roundOneAfterDot(obj.transform.position.y))
            {
                return true;
            }
        }
        return false;
    }

    void checkForClearableLines()
    {
        List<float> toClear = new List<float>();
        List<float> keys = new List<float>(pieceDict.Keys);
        for (int i = 0; i < pieceDict.Count; i++)
        {
            if (pieceDict[keys[i]].Count >= 10)
            {
                toClear.Add(keys[i]);
            }
        }


        clearLines(toClear);
        fallLines(toClear);


        //totalClearedLines += toClear.Count;

        totalClearedLines += toClear.Count;
        lineText.text = (totalClearedLines).ToString();

        if (totalClearedLines >= 10)
        {
            totalClearedLines = totalClearedLines - 10;
            currentLevel += 1;
        }

        levelText.text = currentLevel.ToString();

        death.checkLineLimit(totalClearedLines);

        pointsUpdaterOnClear(toClear.Count, currentLevel);


    }

    void pointsUpdaterOnClear(int linesCleared, int level)
    {
        if (linesCleared == 1)
        {
            pointsText.text = Convert.ToString(Convert.ToInt32(pointsText.text) + 100 * level);
        }
        else if (linesCleared == 2)
        {
            pointsText.text = Convert.ToString(Convert.ToInt32(pointsText.text) + 300 * level);
        }
        else if (linesCleared == 3)
        {
            pointsText.text = Convert.ToString(Convert.ToInt32(pointsText.text) + 500 * level);
        }
        else if (linesCleared == 4)
        {
            pointsText.text = Convert.ToString(Convert.ToInt32(pointsText.text) + 800 * level);
        }
    }

    public void pointsUpdaterOnDrop(int dropType, int cellsFell)
    {
        pointsText.text = Convert.ToString(Convert.ToInt32(pointsText.text) + dropType * cellsFell);
    }

    void fallLines(List<float> toClear)
    {
        if (toClear.Count > 0)
        {
            float startClearingYpos = toClear.Max() + 1;
            int amountOfTimesAlineFalls = toClear.Count();

            for (float i = startClearingYpos; i < pieceDict.Keys.Max() + 1; i++)
            {
                for (int j = 0; j < pieceDict[i].Count; j++)
                {
                    pieceDict[i][j].transform.position += Vector3.down * amountOfTimesAlineFalls;
                }
            }
        }
    }

    void clearLines(List<float> yPoses)
    {
        foreach (float yPos in yPoses)
        {

            List<GameObject> toBeDestroyed = new List<GameObject>(pieceDict[yPos]);

            // Destroy the objects
            foreach (GameObject obj in toBeDestroyed)
            {
                Destroy(obj);
                Debug.Log("Child Count: " + obj.transform.parent.childCount);
                pieceObjects.Remove(obj);
            }


            // Remove the key from the dictionary after clearing
            pieceDict.Remove(yPos);

        }
    }
}