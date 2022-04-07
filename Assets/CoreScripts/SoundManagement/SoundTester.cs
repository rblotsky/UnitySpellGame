using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTester : MonoBehaviour
{
    // DATA //
    public SoundEffect sound;


    // FUNCTIONS //
    // Playing Sound
    [ContextMenu("Test Sound")]
    public void SoundPlayer()
    {
        SoundEffect.TryPlaySoundEffectFromPlayer(sound);
    }
}
