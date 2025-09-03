using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkipper : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            LevelManager.Instance.GoNextLevel();
        }
    }
}
