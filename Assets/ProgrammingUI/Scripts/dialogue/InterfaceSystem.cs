using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InterfaceSystem : MonoBehaviour
{
    public static InterfaceSystem Instance;

    public DialogManager dialogManager;
    public MenuManager menuManager;
    public settingsManager settingsManager;

    private DialogData dialogData;

    public bool OpenedMenu = false;
    public bool OpenedSettings = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetDialogData(DialogData dialogData)
    {
        this.dialogData = dialogData;
    }

    public void InitDialog()
    {
        dialogManager.gameObject.SetActive(true);
        dialogManager.InitDialog(this.dialogData);
    }

    public void OpenMenu()
    {
        menuManager.gameObject.SetActive(true);
        OpenedMenu = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void CloseMenu()
    {
        menuManager.gameObject.SetActive(false);
        OpenedMenu = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = false;
    }

    public void OpenSettingsMenu() 
    {
        CloseMenu();
        settingsManager.gameObject.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        OpenedSettings = true;

    }

    public void BackToMenu()
    {
        settingsManager.gameObject.SetActive(false);
        OpenMenu();
        OpenedSettings = false;
    }
}
