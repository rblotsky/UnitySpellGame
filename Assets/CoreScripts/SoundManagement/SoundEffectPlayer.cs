using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectPlayer : MonoBehaviour
{
    // DATA //
    // Cached Data
    private List<AudioSource> generatedAudioSources = new List<AudioSource>();

    // Static Data
    private static List<SoundEffectPlayer> activeSoundEffectPlayers = new List<SoundEffectPlayer>();

    // Constants
    public static readonly float CLEANUP_DELAY = 1f;


    // FUNCTIONS //
    // Basic Functions
    private void OnEnable()
    {
        // Adds itself to the static list of sound effect players once enabled
        CacheNewSoundEffectPlayer(this);
    }

    private void OnDisable()
    {
        // Removes itself from the static list of sound effect players
        RemoveExistingSoundEffectPlayer(this);
    }


    // Playing Sound Effects
    public void StartSoundEffect(SoundEffect effect)
    {
        // Generates a new AudioSource to play the sound and adds it to generatedAudioSources
        AudioSource audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.loop = false;
        audioPlayer.spatialBlend = 1;

        // Sets up and plays sound effect
        audioPlayer.clip = effect.clip;
        audioPlayer.volume = effect.GetRandomizedVolume();
        audioPlayer.pitch = effect.GetRandomizedPitch();
        audioPlayer.Play();

        // Adds the generated audio source to the generated sources list
        generatedAudioSources.Add(audioPlayer);
    }

    public void StopSoundEffect(SoundEffect effect)
    {
        // Stops all sources playing the specified effect
        //TODO
    }

    public void StopAllSounds()
    {
        // Stops all sound effects currently playing
        //TODO
    }

    public bool IsPlayingSound(SoundEffect effect)
    {
        // Loops through all sources, returns true if any are playing specified sound effect
        foreach (AudioSource source in generatedAudioSources)
        {
            if (source.isPlaying && source.clip == effect.clip)
            {
                return true;
            }
        }

        // Returns false if no sources are current playing
        return false;
    }

    public bool IsPlayingSound()
    {
        // Loops through all sources, returns true if any are playing sounds
        foreach (AudioSource source in generatedAudioSources)
        {
            if (source.isPlaying)
            {
                return true;
            }
        }

        // Returns false if no sources are current playing
        return false;
    }

    public void CleanupGeneratedAudioSources()
    {
        // Caches generated audio sources
        AudioSource[] generatedSourcesCached = generatedAudioSources.ToArray();

        // Checks all generated sources to see if they've been stopped
        foreach (AudioSource audioSource in generatedSourcesCached)
        {
            // If it is no longer playing, removes it from main list and destroys it
            if (!audioSource.isPlaying)
            {
                generatedAudioSources.Remove(audioSource);
                Destroy(audioSource);
            }
        }
    }


    // Static Functions
    public static void CleanupFinishedSoundPlayers()
    {        
        // Loops through all cached sound effect players and has them clean themselves up
        foreach (SoundEffectPlayer soundPlayer in activeSoundEffectPlayers)
        {
            soundPlayer.CleanupGeneratedAudioSources();
        }        
    }

    public static bool CacheNewSoundEffectPlayer(SoundEffectPlayer soundPlayer)
    {
        // Adds and returns true if it isn't yet cached
        if (!activeSoundEffectPlayers.Contains(soundPlayer))
        {
            activeSoundEffectPlayers.Add(soundPlayer);
            return true;
        }

        // Returns false if not added
        return false;
    }

    public static bool RemoveExistingSoundEffectPlayer(SoundEffectPlayer soundPlayer)
    {
        // Removes and returns true if it exists
        if (activeSoundEffectPlayers.Contains(soundPlayer))
        {
            activeSoundEffectPlayers.Remove(soundPlayer);
            return true;
        }

        // Returns false if not removed
        return false;
    }
}
