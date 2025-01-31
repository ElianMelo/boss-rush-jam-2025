using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelMusicData", order = 1)]
public class LevelMusicData : ScriptableObject
{
    public AudioClip MenuMusic;
    public AudioClip TutorialMusic;
    public AudioClip HeadquartersMusic;
    public AudioClip TreemanMusic;
    public AudioClip BikermanMusic;
    public AudioClip TurtlemanMusic;
    public AudioClip EndingMusic;
}
