using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveComponent : MonoBehaviour
{
    public GameDataManager saveManager;

    public void LoadSave(string nameOfSave)
    {
        if(saveManager.LoadGameProgress(nameOfSave))
        {
            Debug.Log("Successfully Loaded!");
        }

        else
        {
            Debug.Log("Failed Loading!");
        }
    }

    public void WriteSave()
    {
        if (saveManager.SaveGameProgress(DataRef.baseData.saveName, SceneManager.GetActiveScene().name))
        {
            Debug.Log("Successfully Saved: " + DataRef.baseData.saveName + "!");
        }

        else
        {
            Debug.Log("Failed Saving: " + DataRef.baseData.saveName + "!");
        }
    }
}
