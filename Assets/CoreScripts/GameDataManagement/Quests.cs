using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class Quests
{
    // DATA //
    // Master Lists
    public static Quest[] allQuests;

    // Constants
    public static readonly string RESOURCE_REQUEST_START_UPDATE_PREFIX = "RESOURCE_REQUEST_ACCEPT_";

    // Dictionaries
    private static Dictionary<int, Quest> idKeyedQuestTable = new Dictionary<int, Quest>();
    private static Dictionary<Quest, QuestGlobalData> questGlobalDataTable = new Dictionary<Quest, QuestGlobalData>();
    

    // FUNCTIONS //
    // Quest Updating Functions
    /// <summary>
    /// Runs a quest update with the given event and null context. Only runs first available one unless multipleUpdates is true.
    /// </summary>
    /// <param name="questUpdateEvent"></param>
    /// <param name="multipleUpdates">Whether to keep going past the first transition.</param>
    /// <returns>Whether any stage transitions were run.</returns>
    public static bool UpdateStages(string questUpdateEvent, bool multipleUpdates)
    {
        return UpdateStages(questUpdateEvent, multipleUpdates, null);
    }

    /// <summary>
    /// Runs a quest update with the given event. Only runs first available one unless multipleUpdates is true.
    /// </summary>
    /// <param name="questUpdateEvent"></param>
    /// <param name="multipleUpdates">Whether to keep going past the first transition.</param>
    /// <param name="context">The GameObject triggering the update event. Used to display dialogue at that location.</param>
    /// <returns>Whether any stage transitions were run.</returns>
    public static bool UpdateStages(string questUpdateEvent, bool multipleUpdates, GameObject context)
    {
        bool returnValue = false;

        // Loops through all quests and updates the first transition that meets requirements.
        foreach (Quest quest in allQuests)
        {
            // If the stage is updated and only one update is allowed, breaks loop.
            if (quest.UpdateStage(questUpdateEvent, context))
            {
                if (!multipleUpdates)
                {
                    returnValue = true;
                    break;
                }
            }
        }

        return returnValue;
    }


    // Request Functions
    /// <summary>
    /// Runs a dialogue request with the given request name. Returns first available dialogue.
    /// </summary>
    /// <param name="requestValue"></param>
    /// <param name="context">The GameObject triggering the request. Used to display dialogue at that location.</param>
    /// <returns>First available dialogue for this request.</returns>
    public static bool RunDialogueRequest(string requestValue, GameObject context)
    {
        // Loops through all quests and runs dialogue requests
        foreach(Quest quest in allQuests)
        {
            // Returns true if any main dialogues run
            if (quest.CurrentStage.CheckDialogueRequests(requestValue, context))
            {
                return true;
            }
        }

        // Returns false if none run
        return false;
    }

    /// <summary>
    /// Adds all map location data for the given request to the location data container object.
    /// </summary>
    /// <param name="requestValue"></param>
    /// <param name="locationDataContainer">The data container to hold all found data.</param>
    public static void RunMapLocationDataRequests(string requestValue, MapLocationQuestDataContainer locationDataContainer)
    {
        // Runs location data request for the STAGE TRANSITIONS and INFO POINTS in all quests.
        // Since map locations should display the next task, this will show the next stages, not the current one.
        foreach(Quest quest in allQuests)
        {
            // Runs data request for all linked stages from quest's current stage (Does not include non-displayable stages)
            foreach(QuestStage stage in quest.GetLinkedStages(quest.CurrentStage, false))
            {
                stage.MapLocationDataRequest(requestValue, locationDataContainer, stage);
            }
        }
    }


    // Searching Functions
    /// <summary>
    /// Returns the quest with given name, null if nonexistent.
    /// </summary>
    /// <param name="questFileName"></param>
    /// <returns></returns>
    public static Quest GetQuest(int questID)
    {
        if(idKeyedQuestTable.ContainsKey(questID))
        {
            return idKeyedQuestTable[questID];
        }

        return null;
    }

    /// <summary>
    /// Returns a List of all quests with given CompletionStatus.
    /// </summary>
    /// <param name="requiredCompletion"></param>
    /// <returns></returns>
    public static List<Quest> GetQuests(CompletionStatus requiredCompletion)
    {
        List<Quest> returnedList = new List<Quest>();

        foreach (Quest quest in allQuests)
        {
            if (quest.QuestCompletion == requiredCompletion)
            {
                returnedList.Add(quest);
            }
        }

        return returnedList;
    }

    /// <summary>
    /// Gets the data container for the given quest. Stores quest current stage + completion status.
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public static QuestGlobalData GetQuestGlobalData(Quest quest)
    {
        // If the quest is in table, returns it
        if (questGlobalDataTable.ContainsKey(quest))
        {
            return questGlobalDataTable[quest];
        }

        // Otherwise, logs error and returns null.
        else
        {
            Debug.LogError("[Quests] (" + quest.name + ") GetQuestGlobalData given quest not in global data table!");
            return null;
        }
    }

    /// <summary>
    /// For use within Quest.CompletionStatus only. If using elsewhere, just use Quest.CompletionStatus instead. 
    /// </summary>
    /// <param name="quest"></param>
    /// <returns>Returns quest completion if it has a generated data container. Otherwise, returns unstarted.</returns>
    public static CompletionStatus GetQuestCompletionStatus(Quest quest)
    {
        // Gets global data for quest
        QuestGlobalData globalData = GetQuestGlobalData(quest);

        // If not null, returns quest completion
        if(globalData != null)
        {
            return globalData.questCompletion;
        }

        // Otherwise, returns unstarted
        else
        {
            return CompletionStatus.Unstarted;
        }
    }

    /// <summary>
    /// For use within Quest.CurrentStage only. If using elsewhere, just use Quest.CurrentStage instead. 
    /// </summary>
    /// <param name="quest"></param>
    /// <returns>Returns quest current stage if it has a generated data container. Otherwise, returns initial stage.</returns>
    public static QuestStage GetQuestCurrentStage(Quest quest)
    {
        // Gets global data for quest
        QuestGlobalData globalData = GetQuestGlobalData(quest);

        // If not null, returns current stage
        if (globalData != null)
        {
            return quest.GetStage(globalData.currentStageID);
        }

        // Otherwise, returns initial stage
        else
        {
            return quest.initialStage;
        }
    }


    // Data Updating
    public static void SetQuestCompletion(Quest quest, CompletionStatus newCompletion)
    {
        Debug.Log("[Quests] Setting Quest Completion: " + quest.name + ", " + newCompletion.ToString());
        // Gets global data for quest
        QuestGlobalData globalData = GetQuestGlobalData(quest);

        // If not null, sets completion status to new value
        if (globalData != null)
        {
            globalData.questCompletion = newCompletion;
        }
    }

    public static void SetQuestCurrentStage(Quest quest, QuestStage newStage)
    {
        // Gets global data for quest
        QuestGlobalData globalData = GetQuestGlobalData(quest);

        // If not null, sets current stage to new value
        if (globalData != null)
        {
            globalData.currentStageID = newStage.id;
        }
    }

    public static void LoadQuestGlobalData(int questID, int currentQuestStageID, CompletionStatus questCompletionStatus)
    {
        // Gets the given quest
        Quest assignedQuest = GetQuest(questID);

        // Updates its global data quest stage and completion status
        questGlobalDataTable[assignedQuest].currentStageID = currentQuestStageID;
        questGlobalDataTable[assignedQuest].questCompletion = questCompletionStatus;
    }

    public static void LoadQuestGlobalData(QuestGlobalData questGlobalData, int questID)
    {
        Quest assignedQuest = GetQuest(questID);
        if(assignedQuest == null)
        {
            Debug.LogError("NULL QUEST: " + questID);
        }
        questGlobalDataTable[assignedQuest] = questGlobalData;
    }


    // Data Management
    public static Queue<string> SaveDataList(Formatting format = Formatting.None)
    {
        Queue<string> lineQueue = new Queue<string>();

        foreach (Quest quest in allQuests)
        {
            string questJson = JsonConvert.SerializeObject(GetQuestGlobalData(quest), format);
            lineQueue.Enqueue(questJson);
        }

        return lineQueue;
    }

    public static void LoadDataList(string saveFileText)
    {
        string[] questsLines = saveFileText.Split('\n');

        foreach(string line in questsLines)
        {
            QuestGlobalData questData = JsonConvert.DeserializeObject<QuestGlobalData>(line.Trim());
            if (questData != null)
            {
                LoadQuestGlobalData(questData, questData.questID);
            }
        }
    }


    // Setup
    public static void MainSetup()
    {
        // Gets a list of all quests from Resources
        allQuests = Resources.LoadAll<Quest>("Quests");

        // Empties both dictionaries
        idKeyedQuestTable.Clear();
        questGlobalDataTable.Clear();

        // Resets all quests and adds them into mainList in correct locations
        foreach (Quest quest in allQuests)
        {
            // Adds it to the name keyed table
            idKeyedQuestTable.Add(quest.id, quest);

            // Assigns an empty global data entry and enters it into table
            QuestGlobalData emptyGlobalData = new QuestGlobalData(quest.initialStage.id, CompletionStatus.Unstarted, quest.id);
            questGlobalDataTable.Add(quest, emptyGlobalData);

            // Runs quest setup and reset
            quest.SetupQuest();
            quest.ResetQuest();
        }
    }
}
