using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PercentObjectPair<T>
{
    // DATA //
    // Instance Data
    public float percentValue;
    public T pairedObject;
    public float rangeMinValue;
    public float rangeMaxValue;

    // Constructors
    public PercentObjectPair(float percent, T obj)
    {
        percentValue = percent;
        pairedObject = obj;
    }

    public PercentObjectPair()
    {
        percentValue = 0;
        pairedObject = default;
    }


    // FUNCTIONS //
    // TODO
}


// DATA STRUCTURES //
[System.Serializable]
public class PercentItemTypePair : PercentObjectPair<UsableType> { }

[System.Serializable]
public class PercentUsablePair : PercentObjectPair<Usable> { }
