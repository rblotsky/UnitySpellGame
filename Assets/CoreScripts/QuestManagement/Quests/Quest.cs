using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Quest Object", menuName = "Quests/Quest Object")]
public class Quest : GameResource
{
    // QUEST DATA //
    // Basic Data
    public string questName;
    [TextArea(1, 5)]
    public string questDescription;
    public bool displayToPlayer;
    [Tooltip("NOTE: This will autogenerate the initial stage's first transition required event. Be careful not to apply this to the wrong quest!")]
    public bool autoGenInitialFirstTransitionEvent = false;
    public QuestStage initialStage;
    public QuestStage[] stageArray;

    // Cached Data
    private Dictionary<int, QuestStage> stageIDTable = new Dictionary<int, QuestStage>();

    // Properties
    public CompletionStatus QuestCompletion { get { return Quests.GetQuestCompletionStatus(this); } set { Quests.SetQuestCompletion(this, value); } }
    public QuestStage CurrentStage { get { return Quests.GetQuestCurrentStage(this); } set { Quests.SetQuestCurrentStage(this, value); } }


    // FUNCTIONS //
    // Basic Quest Functions
    public bool UpdateStage(string questUpdateEvent, GameObject context)
    {
        // Logs error and returns false if there is no current stage
        if (CurrentStage == null)
        {
            Debug.LogError("[Quest] UpdateStage found null value for currentStage. Quest: " + questName);
        }

        // Gets which stage should be transitioned to from the current stage.
        // Return value: true = transitioned to new stage, false = no transition occurred.

        // Gets new stage
        if(CurrentStage == null)
        {
            Debug.LogError("CURRENTSTAGE NULL");
        }
        QuestStage newStage = CurrentStage.RunStageTransition(questUpdateEvent, context);

        // Does nothing if new stage is the same as current stage
        if (newStage == CurrentStage)
        {
            return false;
        }

        else
        {
            // If the current stage is the initial stage, starts quest.
            if (CurrentStage == initialStage)
            {
                // CompletionStatus is set to started, the notification is displayed.
                QuestCompletion = CompletionStatus.Started;

                // Notification only displayed if overlay manager isn't null
                if (DataRef.overlayManagerReference != null && displayToPlayer)
                {
                    DataRef.overlayManagerReference.DisplayQuestNotification(questName, QuestCompletion);
                }
            }

            // If the new stage is a finish stage, finishes the quest.
            if (newStage.isFinishStage)
            {
                // Completion value is set to finish stage completion value, then notification is displayed.
                QuestCompletion = newStage.completionStatus;

                // Notification only displayed if overlay manager isn't null
                if (DataRef.overlayManagerReference != null && displayToPlayer)
                {
                    DataRef.overlayManagerReference.DisplayQuestNotification(questName, QuestCompletion);
                }
            }

            // Updates currentStage, returns true.
            CurrentStage = newStage;
            return true;
        }
    }


    // Getting Data
    public QuestStage GetStage(int stageID)
    {
        QuestStage returnValue = null;
        stageIDTable.TryGetValue(stageID, out returnValue);
        return returnValue;
    }

    public QuestStage[] GetStageArray()
    {
        // Creates list for stage list
        List<QuestStage> returnList = new List<QuestStage>();

        // Calls recursive function to add stages to list
        AddLinkedStagesToListRecursive(returnList, initialStage);

        // Returns list
        return returnList.ToArray();
    }

    public List<QuestStage> GetLinkedStages(QuestStage stage, bool includeNonDisplayStages)
    {
        // Creates list of all linked stages
        List<QuestStage> linkedStages = new List<QuestStage>();

        // Runs through all stage transitions for the stage
        foreach (QuestStageTransition stageTransition in stage.stageTransitions)
        {
            // Only adds to list if it includes all stages, otherwise only if it is meant to be displayed
            if (includeNonDisplayStages || stageTransition.displayToPlayer)
            {
                linkedStages.Add(stageTransition.newStage);
            }
        }

        // Returns completed list
        return linkedStages;
    }

    private void AddLinkedStagesToListRecursive(List<QuestStage> stageList, QuestStage stage)
    {
        // Stops if this stage is already added
        if (stageList.Contains(stage))
        {
            return;
        }

        else
        {
            stageList.Add(stage);

            // Adds every linked stage and all its linked stages to the list
            foreach (QuestStageTransition transition in stage.stageTransitions)
            {
                if (stageList.Contains(transition.newStage))
                {
                    // If the stagelist contains the new stage, doesn't add it.
                    continue;
                }

                else
                {
                    // Adds all linked stages from that stage
                    AddLinkedStagesToListRecursive(stageList, transition.newStage);
                }
            }

            return;
        }
    }


    // Management Functions
    public void SetupQuest()
    {
        // Gets an array of stages and resets stage table
        stageArray = GetStageArray();
        stageIDTable.Clear();

        // Sets up all stages
        foreach (QuestStage stage in stageArray)
        {
            // Sets up stage
            stage.SetupStage(this);

            // Adds to quest stage table
            stageIDTable.Add(stage.id, stage);
        }

        // If it is a resource quest, sets the initial stage's first update to use quest name prefix
        if (autoGenInitialFirstTransitionEvent)
        {
            if(initialStage.stageTransitions.Length == 1)
            {
                initialStage.stageTransitions[0].requiredEvent = Quests.RESOURCE_REQUEST_START_UPDATE_PREFIX + name;
            }
        }
    }

    public void ResetQuest()
    {
        // Resets current stage and completion status
        QuestCompletion = CompletionStatus.Unstarted;
        CurrentStage = initialStage;
    }


    // Editor Functions
    [ContextMenu("Generate Quest JSON")]
    public void GenerateQuestJSON()
    {
        StreamWriter fileWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, (name + "JsonFile.json")));
        Debug.Log(Application.persistentDataPath);
        fileWriter.WriteLine(JsonConvert.SerializeObject(this));
        fileWriter.WriteLine("\nSTAGES\n\n");
        foreach (QuestStage stage in GetStageArray())
        {
            fileWriter.WriteLine(JsonConvert.SerializeObject(stage));
        }
        fileWriter.Close();
    }

    [ContextMenu("Generate Quest Data JSON")]
    public void GenerateQuestDataJSON()
    {
        StreamWriter fileWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, (name + "JsonFile.json")));
        Debug.Log(Application.persistentDataPath);
        fileWriter.WriteLine(JsonConvert.SerializeObject(Quests.GetQuestGlobalData(this)));
        fileWriter.Close();
    }

    [ContextMenu("Setup Stage Array")]
    public void SetupStageArray()
    {
        stageArray = GetStageArray();
    }
}
