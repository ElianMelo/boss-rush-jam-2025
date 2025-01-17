using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> playerAudioSource = new List<AudioSource>();
    public AudioSource playerDrillingLoopSource;

    public List<AudioClip> attackSounds = new List<AudioClip>();
    public AudioClip dashSound;
    public AudioClip jumpSound;

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
        var attackSound = attackSounds[Random.Range(0, attackSounds.Count)];
        PlaySoundClip(attackSound);
    }
    public void PlayJumpSound()
    {
        PlaySoundClip(jumpSound);
    }

    public void StartDrillingSound()
    {
        playerDrillingLoopSource.pitch = Random.Range(0.8f, 1.2f);
        playerDrillingLoopSource.volume = 1;
        playerDrillingLoopSource.Play();
    }

    public void StopDrillingSound()
    {
        StartCoroutine(SmoothDisableSound(0.2f));
    }

    public void PlaySoundClip(AudioClip audioClip)
    {
        AudioSource source = SearchAvaliableAudioSource();
        if (source == null) return;
        source.clip = audioClip;
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
    }

    private AudioSource SearchAvaliableAudioSource()
    {
        foreach (AudioSource source in playerAudioSource)
        {
            if (source.isPlaying) continue;
            return source;
        }
        return null;
    }

    private IEnumerator SmoothDisableSound(float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            playerDrillingLoopSource.volume = Mathf.Lerp(1f,0f, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        playerDrillingLoopSource.volume = 0f;
        playerDrillingLoopSource.Stop();
        yield return null;
    }
}
