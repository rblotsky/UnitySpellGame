using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpRequestUIManager : UIBase, IDataPersistentComponent
{
    // DATA //
    // References
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questDescriptionText;
    public Button acceptButton;
    public Quest selectedResourceQuest;

    // Cached Data
    private GameObject interactableReference;


    // FUNCTIONS //
    // Management
    public void SetupUIManager(Quest questToUse, GameObject interactable)
    {
        // Caches quest and context
        if (questToUse != null)
        {
            selectedResourceQuest = questToUse;
            interactableReference = interactable;
        }

        // Updates UI
        if(selectedResourceQuest != null)
        {
            questNameText.SetText(selectedResourceQuest.questName);
            questDescriptionText.SetText(selectedResourceQuest.questDescription);
            acceptButton.enabled = selectedResourceQuest.QuestCompletion == CompletionStatus.Unstarted;
            acceptButton.onClick.AddListener(AcceptQuest);
        }

        else
        {
            acceptButton.enabled = false;
        }
    }
    
    public void AcceptQuest()
    {
        Quests.UpdateStages(Quests.RESOURCE_REQUEST_START_UPDATE_PREFIX + selectedResourceQuest.questName, false, interactableReference);
    }


    // Interface Functions
    public string SaveDataToString()
    {
        if(selectedResourceQuest != null)
        {
            return selectedResourceQuest.id.ToString();
        }

        else
        {
            return "none";
        }
    }

    public void LoadDataFromString(string data)
    {
        if (int.TryParse(data, out int parsedID))
        {
            selectedResourceQuest = Quests.GetQuest(parsedID);
        }
    }
}
