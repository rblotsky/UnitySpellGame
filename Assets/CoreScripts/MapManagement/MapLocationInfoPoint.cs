using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapLocationInfoPoint
{
    // DATA //
    // Instance Data
    [Range(1,3)]
    public int priority;
    public string text;

    // Static Data
    public static readonly Color highPriorityColour = Color.red;
    public static readonly Color medPriorityColour = Color.yellow;
    public static readonly Color lowPriorityColour = Color.white;
    public static readonly Color[] priorityColoursOrdered = new Color[3] { lowPriorityColour, medPriorityColour, highPriorityColour };
}
