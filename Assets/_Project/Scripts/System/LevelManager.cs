using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum Level
    {
        Headquarters1,
        Treeman,
        Headquarters2, 
        Bikerman,
        Headquarters3,
        Turtleman,
        Headquarters4,
    }

    public Level CurrentLevel = Level.Headquarters1;

    public static LevelManager Instance;

    public const string HeadquartersScene = "Headquarters";
    public const string TreemanScene = "TreeLevel";
    public const string BikermanScene = "BikermanLevel";
    public const string TurtlemanScene = "TurtleLevel";

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    public void GoFirstLevel()
    {
        SceneManager.LoadScene(HeadquartersScene);
    }

    public void GoNextLevel()
    {
        string SceneName = HeadquartersScene;
        switch (CurrentLevel)
        {
            case Level.Headquarters1:
                CurrentLevel = Level.Treeman;
                SceneName = TreemanScene;
                break;
            case Level.Treeman:
                CurrentLevel = Level.Headquarters2;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters2:
                CurrentLevel = Level.Bikerman;
                SceneName = BikermanScene;
                break;
            case Level.Bikerman:
                CurrentLevel = Level.Headquarters3;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters3:
                CurrentLevel = Level.Turtleman;
                SceneName = TurtlemanScene;
                break;
            case Level.Turtleman:
                CurrentLevel = Level.Headquarters4;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters4:
                CurrentLevel = Level.Headquarters1;
                SceneName = HeadquartersScene;
                break;
            default:
                break;
        }
        SceneManager.LoadScene(SceneName);
    }

    public void ResetCurrentLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
