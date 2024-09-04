using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSettings : MonoBehaviour
{

    public bool gravity;
    public float timeLimit;
    public float lineLimit;

    public static string selectedMode;
    // Start is called before the first frame update
    void Start()
    {
        setModeSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void setModeSettings()    
    {
        if(selectedMode == "normal")
        {
            gravity = true;
            timeLimit = 0f;
            lineLimit = 0f;
            Debug.Log("NORMAL");
        }
        else if(selectedMode == "40")
        {
            gravity = true;
            timeLimit = 0f;
            lineLimit = 40f;
        }
        else if (selectedMode == "2min")
        {
            gravity = true;
            timeLimit = 120f;
            lineLimit = 0;
        }
        else if (selectedMode == "zen")
        {
            gravity = false;
            timeLimit = 0f;
            lineLimit = 0f;
        }
    }
}
