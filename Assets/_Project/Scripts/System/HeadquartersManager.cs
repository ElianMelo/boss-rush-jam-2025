using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeadquartersState
{
    Walking,
    Talking,
    Paused
}

public class HeadquartersMananger : MonoBehaviour
{
    public static HeadquartersMananger Instance;

    public HeadquartersState CurrentState = HeadquartersState.Walking;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeHeadquartersState(HeadquartersState newState)
    {
        CurrentState = newState;
    }
    
}
