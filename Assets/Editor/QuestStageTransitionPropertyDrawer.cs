using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(QuestStageTransition))]
public class QuestStageTransitionPropertyDrawer : PropertyDrawer
{
    // Property Drawer Data //
    const float objectPropertyWidth = 250;
    const float percentPropertyWidth = 75;
    const float maxPercent = 100;
    const float minPercent = 0;

    // Functions //
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Gets properties
        SerializedProperty eventProperty = property.FindPropertyRelative("requiredEvent");
        SerializedProperty stageProperty = property.FindPropertyRelative("newStage");
        SerializedProperty displayBoolProperty = property.FindPropertyRelative("displayToPlayer");
        SerializedProperty requirementsProperty = property.FindPropertyRelative("transitionRequirements");

        //TODO:
        //Draws transition name
        //Draws newstage
        //Draws displaytoplayer, possibly to the right of transition name?
        //Draws a notice on how to use it
        //Draws transition requirement info without a list

        //FROM PERCENTOBJECTPAIRPROPERTYDRAWER FOR REFERENCE ON HOW TO USE THIS:
        // Draws percent value 
        //EditorGUI.PropertyField(new Rect(position.x, position.y, percentPropertyWidth, position.height), percentProperty, GUIContent.none);

        // Draws usable object property
        //int indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;
        //EditorGUI.PropertyField(new Rect(position.width - objectPropertyWidth, position.y, objectPropertyWidth, position.height), objectProperty, GUIContent.none);
        //EditorGUI.indentLevel = indent;
    }
}
