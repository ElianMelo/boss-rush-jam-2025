using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog
{
    [TextArea]
    public string text;
    public bool isFirstPortrait = true;
    public bool isBothPortrait = false;
    public AudioClip clip;
}