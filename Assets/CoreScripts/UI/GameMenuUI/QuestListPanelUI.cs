using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListPanelUI : MonoBehaviour
{
    // PANEL DATA //
    // References
    public QuestListSlotUI[] questSlots;

    // Quest Data Storage
    public List<Quest> startedQuests = new List<Quest>();

    // Cache Data
    private int minValue;
    private int maxValue;


    // FUNCTIONS //
    // Basic and Setup Functions
    public void StartUp()
    {
        // Gets all started quests
        startedQuests = Quests.GetQuests(CompletionStatus.Started);

        // Resets min and max values for quest list
        minValue = 0;
        maxValue = questSlots.Length - 1;

        // Displays list
        UpdateList();
    }


    // External Functions
    public void ScrollDownList()
    {
        minValue++;
        maxValue++;
    }


    // Panel Update Functions
    public void UpdateList()
    {
        // Sets iterator variable, adds quests according to this
        int currentValue = minValue;

        // Loops through all slots
        foreach(QuestListSlotUI slot in questSlots)
        {
            Quest currentQuest = GetQuest(currentValue);

            if (currentQuest != null)
            {
                if (GetQuest(currentValue).displayToPlayer)
                {
                    // Updated quest reference is the quest at index currentValue
                    slot.UpdateQuestReference(GetQuest(currentValue));

                    // Increments currentValue to get next item in list
                    currentValue++;
                }
            }
        }
    }


    // Internal Management Functions
    private Quest GetQuest(int index)
    {
        if(index > startedQuests.Count - 1)
        {
            return null;
        }

        else
        {
            return startedQuests[index];
        }
    }
}
