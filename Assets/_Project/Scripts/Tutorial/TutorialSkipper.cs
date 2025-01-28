using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkipper : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            LevelManager.Instance.GoNextLevel();
        }
    }
}
