using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioSourcePrefab;

    private List<AudioSource> audioSources = new List<AudioSource>();

    // instantiate a new audio source gameobject and add it to the sound manager list
    private AudioSource InstantiateAudioSource()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity, transform);
        audioSources.Add(audioSource);
        return audioSource;
    }

    // return if the given audio clip is playing in any of the audio manager children
    public bool isAlreadyPlaying(AudioClip audioClip)
    {
        if (audioSources.Count > 0)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.isPlaying && audioSource.clip == audioClip)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // if the given audio clip is playing in any of the audio manager children, stop it return the audio source
    public AudioSource StopPlayingAudioClip(AudioClip audioClip)
    {
        if (audioSources.Count > 0)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.isPlaying && audioSource.clip == audioClip)
                {
                    audioSource.Stop();
                    return audioSource;
                }
            }
        }

        return null;
    }

    // return a free audio source or null
    public AudioSource SearchFreeAudioSource()
    {
        if (audioSources.Count > 0)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.isPlaying == false)
                {
                    return audioSource;
                }
            }
        }

        return null;
    }

    // return a randomized pitch between two values
    public float RandomizePitch(float minPitch = -3.0f, float maxPitch = 3.0f)
    {
        float pitch = 1.0f;

        if (minPitch < maxPitch)
        {
            pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        }
        else
        {
            pitch = UnityEngine.Random.Range(maxPitch, minPitch);
        }

        return Mathf.Clamp(pitch, -3.0f, 3.0f);
    }

    /// <summary>
    /// Play given audio clip with the following parameters: loop, priority, volume and pitch.
    /// </summary>
    /// <param name="audioClip">Sets the Audio Clip to play.</param>
    /// <param name="stopPrevious">Stops the Audio Source that is already playing the same Audio Clip.</param>
    /// <param name="loop">Sets the loop property of the Audio Source.</param>
    /// <param name="priority">Sets the priority property of the Audio Source. Priority is an integer between 0 and 255. 0=highest priority, 255=lowest priority.</param>
    /// <param name="volume">Sets the volume property of the Audio Source.</param>
    /// <param name="pitch">Sets the pitch property of the Audio Source.</param>
    /// <returns>Returns true if given Audio Clip started playing. Returns false if given Audio Clip could not be started.</returns>
    public bool PlayAudioClip(AudioClip audioClip, bool stopPrevious = false, bool loop = false, int priority = 128, float volume = 1.0f, float pitch = 1.0f)
    {

        AudioSource audioSourceToUse = null;

        if (stopPrevious)
        {
            audioSourceToUse = StopPlayingAudioClip(audioClip);
        }

        if (audioSourceToUse == null)
        {
            audioSourceToUse = SearchFreeAudioSource();
        }

        if (audioSourceToUse == null)
        {
            audioSourceToUse = InstantiateAudioSource();
        }

        priority = Mathf.Clamp(priority, 0, 255);
        volume = Mathf.Clamp01(volume);
        pitch = Mathf.Clamp(pitch, -3.0f, 3.0f);

        if (audioSourceToUse != null)
        {
            audioSourceToUse.clip = audioClip;
            audioSourceToUse.loop = loop;
            audioSourceToUse.priority = priority;
            audioSourceToUse.volume = volume;
            audioSourceToUse.pitch = pitch;
            audioSourceToUse.Play();

            return true;
        }

        return false;

    }

    // toggle audio listner attached to main camera on or off
    public void ToggleAudioListner()
    {
        Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;
    }

}
