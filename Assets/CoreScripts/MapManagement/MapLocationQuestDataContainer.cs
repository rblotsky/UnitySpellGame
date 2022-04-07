using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationQuestDataContainer
{
    // DATA //
    // Marked Stages / Info Points
    public List<QuestStage> markedStages;
    public List<MapLocationInfoPoint> infoPoints;

    // Constants
    public static readonly Color MARKED_STAGE_DISPLAY_UI_QUEST_NAME_COLOUR = Color.green;
    public static readonly Color MARKED_STAGE_DISPALY_UI_STAGE_NAME_COLOUR = Color.white;
    public static readonly string EMPTY_INFO_POINTS_DISPLAY_TEXT = "No info points.";
    public static readonly string EMPTY_MARKED_STAGES_DISPLAY_TEXT = "No marked quest stages.";

    
    // Constructor
    public MapLocationQuestDataContainer()
    {
        markedStages = new List<QuestStage>();
        infoPoints = new List<MapLocationInfoPoint>();
    }


    // Functions //
    // Generating display text
    public string GenerateMarkedStageDisplayText()
    {
        // Creates a string
        string generatedString = "";

        // Iterates through all marked stages and adds them
        foreach (QuestStage markedStage in markedStages)
        {
            // Generates formatted quest and stage names
            string formattedQuestName = GameUtility.GenerateHTMLColouredText("<b>[" + markedStage.questReference.questName + "]</b>", MARKED_STAGE_DISPLAY_UI_QUEST_NAME_COLOUR);
            string formattedStageName = GameUtility.GenerateHTMLColouredText(markedStage.transitionToThisStage, MARKED_STAGE_DISPALY_UI_STAGE_NAME_COLOUR);

            // Adds them together and adds to the generated text
            generatedString += formattedQuestName + " " + formattedStageName + "\n";
        }

        // If the generated text is still empty, sets it to default value
        if (generatedString.Length <= 1)
        {
            generatedString = EMPTY_MARKED_STAGES_DISPLAY_TEXT;
        }

        return generatedString;
    }

    public string GenerateInfoPointsDisplayText()
    {
        // Creates single string of notifications, ordered by priority
        //TODO: Order by priority
        string generatedString = "";

        // Iterates through all info points and adds them
        foreach (MapLocationInfoPoint infoPoint in infoPoints)
        {
            // Adds colour using HTML format
            string colouredText = GameUtility.GenerateHTMLColouredText(infoPoint.text, MapLocationInfoPoint.priorityColoursOrdered[infoPoint.priority - 1]);

            // Adds text to generated text
            generatedString += colouredText + "\n";
        }

        // If the generated text is still empty, sets it to default value
        if (generatedString.Length <= 1)
        {
            generatedString = EMPTY_INFO_POINTS_DISPLAY_TEXT;
        }

        return generatedString;
    }
}
