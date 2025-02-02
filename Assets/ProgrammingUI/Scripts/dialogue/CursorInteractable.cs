using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CenterCursorInteraction : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite interactCursor;  
    [SerializeField] private float maxRayDistance = 8f;
    [SerializeField] private LayerMask interactableLayer;

    public DialogData Headquarters1Data;
    public DialogData TreemanData;
    public DialogData Headquarters2Data;
    public DialogData BikermanData;
    public DialogData Headquarters3Data;
    public DialogData TurtlemanData;
    public DialogData Headquarters4Data;

    private void Start()
    {
        var currentLobbyState = LevelManager.Instance.CurrentLevel;

        switch (currentLobbyState)
        {
            case LevelManager.Level.Headquarters1:
                InterfaceSystem.Instance.SetDialogData(Headquarters1Data);
                break;
            case LevelManager.Level.Treeman:
                InterfaceSystem.Instance.SetDialogData(TreemanData);
                break;
            case LevelManager.Level.Headquarters2:
                InterfaceSystem.Instance.SetDialogData(Headquarters2Data);
                break;
            case LevelManager.Level.Bikerman:
                InterfaceSystem.Instance.SetDialogData(BikermanData);
                break;
            case LevelManager.Level.Headquarters3:
                InterfaceSystem.Instance.SetDialogData(Headquarters3Data);
                break;
            case LevelManager.Level.Turtleman:
                InterfaceSystem.Instance.SetDialogData(TurtlemanData);
                break;
            case LevelManager.Level.Headquarters4:
                InterfaceSystem.Instance.SetDialogData(Headquarters4Data);
                break;
        }
    }

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (HeadquartersMananger.Instance.CurrentState == HeadquartersState.Talking) return;

            if (!InterfaceSystem.Instance.OpenedMenu && !InterfaceSystem.Instance.OpenedSettings && !InterfaceSystem.Instance.OpenedHelp)
            {
                HeadquartersMananger.Instance.ChangeHeadquartersState(HeadquartersState.Paused);
                InterfaceSystem.Instance.OpenMenu();
            }
            else
            {
                HeadquartersMananger.Instance.ChangeHeadquartersState(HeadquartersState.Walking);
                InterfaceSystem.Instance.CloseMenu();
            }
            
        }
    }

    private void CheckForInteractable()
    {
        if (HeadquartersMananger.Instance.CurrentState == HeadquartersState.Talking) return;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayer))
        {
            cursorImage.sprite = interactCursor;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Dialogo iniciado");
                InterfaceSystem.Instance.InitDialog();
            }

        }
        else
        {
            cursorImage.sprite = defaultCursor;
        }
    }
}
