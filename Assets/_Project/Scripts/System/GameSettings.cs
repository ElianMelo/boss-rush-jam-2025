using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public bool invertYAxis = false;

    public static GameSettings Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }
}
