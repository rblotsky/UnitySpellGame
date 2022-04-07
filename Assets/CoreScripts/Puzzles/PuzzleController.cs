using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    // This class is the main class for running and managing a puzzle
    // PUZZLE DATA //
    // Basic Data
    public PuzzleData[] puzzleDataList;

    // Events
    public UnityEvent OnCompletion;
    public UnityEvent OnPuzzleUpdate;
    public UnityEvent OnFailure;


    // FUNCTIONS //
    public PuzzleData GetData(string name)
    {
        foreach(PuzzleData dataValue in puzzleDataList)
        {
            if (dataValue.dataName.Equals(name))
            {
                return dataValue;
            }
        }

        return null;
    }

    public void UpdateData(int valueChange, string dataName)
    {
        PuzzleData dataToModify = GetData(dataName);
        if (dataToModify != null)
        {
            dataToModify.UpdateValue(valueChange);
        }
    }
}



[System.Serializable]
public class PuzzleData
{
    public string dataName;
    public int value;


    // FUNCTIONS //
    public void UpdateValue(int change)
    {
        value += change;
    }
}


[System.Serializable]
public class PuzzleRequirement
{
    // TODO
}
