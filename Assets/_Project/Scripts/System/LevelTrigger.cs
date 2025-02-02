using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public bool StartupScene = false;

    private void Start()
    {
        if(StartupScene)
        {
            LevelManager.Instance.GoFirstLevel();
        }
    }

    public void GoNextLevel()
    {
        LevelManager.Instance.GoNextLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        LevelManager.Instance.GoNextLevel();
    }
}
