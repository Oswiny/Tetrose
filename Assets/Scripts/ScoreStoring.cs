using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreStoring : MonoBehaviour
{
    // Start is called before the first frame update

    static Dictionary<string, List<float>> scores = new Dictionary<string, List<float>>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    static public void storeScore(string gameMode,float score)
    {
        if (scores.ContainsKey(gameMode))
        {
            scores[gameMode].Add(score);
        }
        else
        {
            scores.Add(gameMode, new List<float>(){score});
        }
    }
    
    static public float getHighScore(string gameMode)
    {
        return scores[gameMode].Max();
    }
}
