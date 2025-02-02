using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CreditsVideoPlayer : MonoBehaviour
{
    private VideoPlayer player;
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += OnLoopPointReached;
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Credits.mp4");
        player.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            LevelManager.Instance.GoMenuLevel();
        }
    }
    void OnLoopPointReached(VideoPlayer vp)
    {
        LevelManager.Instance.GoMenuLevel();
    }
}
