using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> playerAudioSource = new List<AudioSource>();
    public AudioSource playerDrillingLoopSource;

    public List<AudioClip> attackSounds = new List<AudioClip>();
    public List<AudioClip> treemanProvocations = new List<AudioClip>();
    public List<AudioClip> bikermanProvocations = new List<AudioClip>();
    public List<AudioClip> turtlemanProvocations = new List<AudioClip>();
    public AudioClip dashSound;
    public AudioClip jumpSound;
    public AudioClip eletricPillarSound;
    public AudioClip pillarSound;
    public AudioClip shootingSound;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDashSound()
    {
        PlaySoundClip(dashSound);
    }

    public void PlayBrokenPillar()
    {
        PlaySoundClip(eletricPillarSound, true);
    }

    public void PlayPillarSound()
    {
        PlaySoundClip(pillarSound);
    }

    public void PlayBossShooting()
    {
        PlaySoundClip(shootingSound);
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

    public void PlayTreemanProvocationSound()
    {
        var bossSound = treemanProvocations[Random.Range(0, treemanProvocations.Count)];
        PlaySoundClip(bossSound, true);
    }

    public void PlayTurtlemanProvocationSound()
    {
        var bossSound = turtlemanProvocations[Random.Range(0, turtlemanProvocations.Count)];
        PlaySoundClip(bossSound, true);
    }

    public void PlayBikermanProvocationSound()
    {
        var bossSound = bikermanProvocations[Random.Range(0, bikermanProvocations.Count)];
        PlaySoundClip(bossSound, true);
    }

    public void PlaySoundClip(AudioClip audioClip, bool forcePitch = false)
    {
        AudioSource source = SearchAvaliableAudioSource();
        if (source == null) return;
        source.clip = audioClip;

        if(forcePitch)
            source.pitch = 1;
        else
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
