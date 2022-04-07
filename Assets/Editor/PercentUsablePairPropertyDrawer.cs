using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof(PercentUsablePair))]
public class PercentUsablePairPropertyDrawer : PropertyDrawer
{
    // Property Drawer Data //
    const float objectPropertyWidth = 250;
    const float percentPropertyWidth = 40;
    const float maxPercent = 100;
    const float minPercent = 0;

    // Functions //
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Gets properties
        SerializedProperty percentProperty = property.FindPropertyRelative("percentValue");
        SerializedProperty objectProperty = property.FindPropertyRelative("pairedObject");

        // Draws percent value 
        EditorGUI.PropertyField(new Rect(position.x, position.y, percentPropertyWidth, position.height), percentProperty, GUIContent.none);

        // Draws usable object property
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;
        EditorGUI.PropertyField(new Rect(position.width - objectPropertyWidth, position.y, objectPropertyWidth, position.height), objectProperty, GUIContent.none);
        EditorGUI.indentLevel = indent;
    }
}
