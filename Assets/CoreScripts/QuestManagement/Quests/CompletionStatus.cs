using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CompletionStatus
{
    Failed = -1,
    Unstarted = 0,
    Started = 1,
    Succeeded = 2,
}
