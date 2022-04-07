using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestListSlotUI : MonoBehaviour
{
    // SLOT DATA //
    // References
    public TextMeshProUGUI questNameText;
    public Quest questReference;


    // FUNCTIONS //
    // Assigning References
    public void UpdateQuestReference(Quest quest)
    {
        // Updates reference
        questReference = quest;

        // Makes slot invisible if there is no quest assigned, otherwise sets name and makes it visible
        if(questReference == null)
        {
            // Resets text and makes it invisible
            questNameText.SetText("");
            SetVisibility(false);
        }

        else
        {
            // Updates text and makes it visible
            questNameText.SetText(quest.questName);
            SetVisibility(true);
        }
    }


    // Managing Visibility
    public void SetVisibility(bool value)
    {
        // Sets visibility of slot
        gameObject.SetActive(value);
    }
}
