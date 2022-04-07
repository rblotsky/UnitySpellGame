using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Requirement
{
    // REQUIREMENT DATA //
    public bool not = false;
    public RequirementType requirementType;
    public Quest quest;
    public QuestStage stage;
    public CompletionStatus completion;
    public Usable usable;
    public int usableAmount;


    // FUNCTIONS //
    public bool CheckRequirement()
    {
        // Sets return operator (the opposite of not variable, so if it is set to NOT then will fail when passed, if it isnt will pass when passed)
        bool returnOperator = !not;

        // Checks each requirement
        switch (requirementType)
        {
            case RequirementType.Quest_Completion:

                if (quest.QuestCompletion == completion)
                {
                    return returnOperator;
                }

                else
                {
                    return !returnOperator;
                }

            case RequirementType.Quest_Current_Stage:

                if( quest.CurrentStage == stage)
                {
                    return returnOperator;
                }

                else
                {
                    return !returnOperator;
                }

            case RequirementType.Usable:

                if (PlayerEquipment.main != null)
                {
                    if(PlayerEquipment.main.GetNumOfItem(usable, true) >= usableAmount)
                    {
                        return returnOperator;
                    }

                    else
                    {
                        return !returnOperator;
                    }
                }

                else
                {
                    return false;
                }
        }

        // Returns true by default
        return true;
    }

    // Called by RequirementsList when all requirements are met to take required items
    public void CompleteRequirement()
    {
        // If it is a usable req, takes the items
        if(requirementType == RequirementType.Usable)
        {
            if(PlayerEquipment.main != null)
            {
                if(!PlayerEquipment.main.RemoveItemFromInventory(usable, true, usableAmount, false))
                {
                    Debug.LogError("[Requirement] Requirement attempted removing item from inventory in amount " + usableAmount + " but failed for unknown reasons.");
                }
            }
        }
    }
}
