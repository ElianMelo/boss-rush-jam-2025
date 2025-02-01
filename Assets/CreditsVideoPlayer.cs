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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LevelManager.Instance.GoMenuLevel();
        }
    }
    void OnLoopPointReached(VideoPlayer vp)
    {
        LevelManager.Instance.GoMenuLevel();
    }
}
