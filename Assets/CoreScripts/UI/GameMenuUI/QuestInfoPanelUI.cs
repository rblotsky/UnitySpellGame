using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestInfoPanelUI : MonoBehaviour
{
    // PANEL DATA //
    // References
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questStageInfoText;

    // Cache Data
    private Quest questReference;

    // FUNCTIONS //
    public void UpdatePanel(QuestListSlotUI questSlot)
    {
        // Gets quest reference
        questReference = questSlot.questReference;

        // If it's null, removes all text boxes
        if(questReference == null)
        { 
            questNameText.SetText("");
            questDescriptionText.SetText("");
            questStageInfoText.SetText("");
            return;
        }

        // Otherwise, adds text according to quest
        questNameText.SetText(questReference.questName);
        questDescriptionText.SetText(questReference.questDescription);

        string questStageInfo = "";
        foreach(QuestStageTransition transition in questReference.CurrentStage.stageTransitions)
        {
            if (transition.displayToPlayer)
            {
                questStageInfo += transition.newStage.transitionToThisStage + "\n";
            }
        }

        questStageInfoText.SetText(questStageInfo);
    }
}
