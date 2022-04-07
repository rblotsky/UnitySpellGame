using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableEffectInstance
{
    // Summary //
    /*
     * This class stores an instance of a usable being used.
     * Its effect is run from the Usable itself, it's used to
     * simply store data for when it should run out.
     */

    // DATA //
    public float effectStartTime;
    public float effectValue;


    // CONSTRUCTORS //
    public UsableEffectInstance(float startTime, float value)
    {
        effectStartTime = startTime;
        effectValue = value;
    }


}
