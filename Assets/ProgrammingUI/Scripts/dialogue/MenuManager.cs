using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void OnResumeClick()
    {
        InterfaceSystem.Instance.CloseMenu();
    }

    public void OnSettingsClick()
    {
        InterfaceSystem.Instance.OpenSettingsMenu();
    }

    public void OnLeaveGameClick()
    {
        Debug.Log("Exiting the game...");
    }
}
