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
    public LevelMusicData levelMusicData;
    public AudioSource audioSource;

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
        PlayerCurrentClip(levelMusicData.MenuMusic);
        SceneManager.LoadScene(MenuScene);
    }

    public void GoNextLevel()
    {
        string SceneName = HeadquartersScene;
        AudioClip currentClip = levelMusicData.MenuMusic;
        switch (CurrentLevel)
        {
            case Level.Menu:
                CurrentLevel = Level.Tutorial;
                currentClip = levelMusicData.TutorialMusic;
                SceneName = TutorialScene;
                break;
            case Level.Tutorial:
                CurrentLevel = Level.Headquarters1;
                currentClip = levelMusicData.HeadquartersMusic;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters1:
                CurrentLevel = Level.Treeman;
                currentClip = levelMusicData.TreemanMusic;
                SceneName = TreemanScene;
                break;
            case Level.Treeman:
                CurrentLevel = Level.Headquarters2;
                currentClip = levelMusicData.HeadquartersMusic;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters2:
                CurrentLevel = Level.Bikerman;
                currentClip = levelMusicData.BikermanMusic;
                SceneName = BikermanScene;
                break;
            case Level.Bikerman:
                CurrentLevel = Level.Headquarters3;
                currentClip = levelMusicData.HeadquartersMusic;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters3:
                CurrentLevel = Level.Turtleman;
                currentClip = levelMusicData.TurtlemanMusic;
                SceneName = TurtlemanScene;
                break;
            case Level.Turtleman:
                CurrentLevel = Level.Headquarters4;
                currentClip = levelMusicData.HeadquartersMusic;
                SceneName = HeadquartersScene;
                break;
            case Level.Headquarters4:
                CurrentLevel = Level.Menu;
                currentClip = levelMusicData.MenuMusic;
                SceneName = MenuScene;
                break;
            default:
                break;
        }
        PlayerCurrentClip(currentClip);
        SceneManager.LoadScene(SceneName);
    }

    private void PlayerCurrentClip(AudioClip clip)
    {
        if (audioSource.clip == clip) return;
        audioSource.clip = clip;
        audioSource.Stop();
        audioSource.Play();
    }

    public void ResetCurrentLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
