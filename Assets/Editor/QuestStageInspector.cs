using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestStage))]
public class QuestStageInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestStage stage = (QuestStage)target;
    }
}
