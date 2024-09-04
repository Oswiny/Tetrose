using UnityEngine;

public class LoadGameMode : MonoBehaviour
{
    public void loadMode(string gameMode)
    {
        ModeSettings.selectedMode = gameMode;
    }
}
