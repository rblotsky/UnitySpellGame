using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGlobalData
{
    // Stores global quest data that can be edited easily
    // DATA //
    public int questID;
    public int currentStageID;
    public CompletionStatus questCompletion;


    // Constructors //
    public QuestGlobalData() { }

    public QuestGlobalData(int stageID, CompletionStatus completion, int id)
    {
        currentStageID = stageID;
        questCompletion = completion;
        questID = id;
    }
}
