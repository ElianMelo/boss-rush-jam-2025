using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void OnResumeClick()
    {
        HeadquartersMananger.Instance.ChangeHeadquartersState(HeadquartersState.Walking);
        InterfaceSystem.Instance.CloseMenu();
    }

    public void OnSettingsClick()
    {
        InterfaceSystem.Instance.OpenSettingsMenu();
    }

    public void OnHelpClick()
    {
        InterfaceSystem.Instance.OpenHelpMenu();
    }

    public void OnLeaveGameClick()
    {
        //Debug.Log("Exiting the game...");
        Application.Quit();
    }
}
