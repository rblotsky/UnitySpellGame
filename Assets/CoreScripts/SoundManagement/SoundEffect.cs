using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Effect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject
{
    // DATA //
    public AudioClip clip;
    [Range(-3, 3)] public float pitch = 1;
    [Range(0, 1)] public float volume = 1;
    [Range(0, 3)] public float pitchRandomizer = 0.1f;
    [Range(0, 1)] public float volumeRandomizer = 0f;
    public float maxAudibleDistance = 10f;


    // FUNCTIONS //
    // Playing Effects
    public void PlayEffect(SoundEffectPlayer effectPlayer)
    {
        effectPlayer.StartSoundEffect(this);
    }

    public void PlayEffectFromPlayerCharacter()
    {
        PlayEffect(DataRef.playerReference.component_SoundPlayer);
    }


    // Getting modified values
    public float GetRandomizedPitch()
    {
        return pitch + Random.Range(-pitchRandomizer, pitchRandomizer);
    }

    public float GetRandomizedVolume()
    {
        return volume + Random.Range(-volumeRandomizer, volumeRandomizer);
    }


    // Static Functions
    public static bool TryPlaySoundEffect(SoundEffect effect, SoundEffectPlayer effectPlayer)
    {
        if (effect != null && effectPlayer != null)
        {
            effect.PlayEffect(effectPlayer);
            return true;
        }

        return false;
    }

    public static bool TryPlaySoundEffectFromPlayer(SoundEffect effect)
    {
        // Plays the sound effect if it isn't null and the player main reference is assigned
        if (effect != null && DataRef.playerReference != null && DataRef.playerReference.component_SoundPlayer != null)
        {
            effect.PlayEffectFromPlayerCharacter();
            return true;
        }

        // Returns false if fails to play for any reason
        return false;
    }
}
