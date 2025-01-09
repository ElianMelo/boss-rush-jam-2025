using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> audioSource = new List<AudioSource>();

    public AudioClip dashSound;
    public AudioClip attackSound;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDashSound()
    {
        PlaySoundClip(dashSound);
    }

    public void PlayAttackSound()
    {
        PlaySoundClip(attackSound);
    }

    public void PlaySoundClip(AudioClip audioClip)
    {
        AudioSource source = SearchAvaliableAudioSource();
        if (source == null) return;
        source.clip = audioClip;
        source.Play();
    }

    private AudioSource SearchAvaliableAudioSource()
    {
        foreach (AudioSource source in audioSource)
        {
            if (source.isPlaying) continue;
            return source;
        }
        return null;
    }
}
