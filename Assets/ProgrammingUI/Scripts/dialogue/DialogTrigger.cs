using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogData TreemanData;
    public DialogData BikermanData;
    public DialogData TurtlemanData;
    private Collider dialogCollider;

    private void Start()
    {
        dialogCollider = GetComponent<Collider>();
        var currentLobbyState = LevelManager.Instance.CurrentLevel;

        switch (currentLobbyState)
        {
            case LevelManager.Level.Treeman:
                InterfaceSystem.Instance.SetDialogData(TreemanData);
                break;
            case LevelManager.Level.Bikerman:
                InterfaceSystem.Instance.SetDialogData(BikermanData);
                break;
            case LevelManager.Level.Turtleman:
                InterfaceSystem.Instance.SetDialogData(TurtlemanData);
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (HeadquartersMananger.Instance.CurrentState == HeadquartersState.Talking) return;

            if (!InterfaceSystem.Instance.OpenedMenu && !InterfaceSystem.Instance.OpenedSettings)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            InterfaceSystem.Instance.InitDialog();
            dialogCollider.enabled = false;
        }
    }
}
