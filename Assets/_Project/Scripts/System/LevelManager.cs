using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum Level
    {
        Menu,
        Tutorial,
        Headquarters1,
        Treeman,
        Headquarters2, 
        Bikerman,
        Headquarters3,
        Turtleman,
        Headquarters4,
    }

    public Level CurrentLevel = Level.Menu;

    public static LevelManager Instance;

    public const string MenuScene = "Menu";
    public const string TutorialScene = "TutorialLevel";
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
        SceneManager.LoadScene(MenuScene);
    }

    public void GoNextLevel()
    {
        string SceneName = HeadquartersScene;
        switch (CurrentLevel)
        {
            case Level.Menu:
                CurrentLevel = Level.Tutorial;
                SceneName = TutorialScene;
                break;
            case Level.Tutorial:
                CurrentLevel = Level.Headquarters1;
                SceneName = HeadquartersScene;
                break;
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
                CurrentLevel = Level.Menu;
                SceneName = MenuScene;
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
