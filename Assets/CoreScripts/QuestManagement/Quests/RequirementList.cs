using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequirementList
{
    // Data Structures
    public enum LogicOperator
    {
        Or = 0,
        And = 1,
    }

    // REQUIREMENT DATA //
    public LogicOperator requirementOperator = LogicOperator.And;
    public Requirement[] requirements;

    public bool CheckRequirements()
    {
        // Returns true if no requirements
        if (requirements.Length == 0)
        {
            return true;
        }

        // Goes through all requirements, requires all to pass if AND or at least one if OR
        List<Requirement> passedReqs = new List<Requirement>();

        foreach(Requirement requirement in requirements)
        {
            if (requirement.CheckRequirement())
            {
                passedReqs.Add(requirement);

                // Stops loop if OR
                if(requirementOperator == LogicOperator.Or)
                {
                    break;
                }
            }

            else
            {
                // Stops loop if AND
                if(requirementOperator == LogicOperator.And)
                {
                    passedReqs.Clear();
                    break;
                }
            }
        }

        // Completes all passed requirements
        foreach(Requirement requirement in passedReqs)
        {
            requirement.CompleteRequirement();
        }

        // If at least one requirement passed, returns true. If AND operator and any requirements didn't pass, the list would have been emptied.
        return passedReqs.Count > 0;
    }
}