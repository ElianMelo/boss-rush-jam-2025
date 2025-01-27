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

    public DialogData InitNPC;
    public DialogData DeathToBossOneNPC;
    public DialogData WinToBossOneNPC;
    public DialogData DeathToBossTwoNPC;
    public DialogData WinToBossTwoNPC;
    public DialogData DeathToBossThreeNPC;
    public DialogData WinToBossThreeNPC;

    private void Start()
    {
        var currentLobbyState = LevelManager.Instance.CurrentLevel;

        switch (currentLobbyState)
        {
            case LevelManager.Level.Headquarters1:
                InterfaceSystem.Instance.SetDialogData(InitNPC);
                break;
            case LevelManager.Level.Treeman:
                InterfaceSystem.Instance.SetDialogData(DeathToBossOneNPC);
                break;
            case LevelManager.Level.Headquarters2:
                InterfaceSystem.Instance.SetDialogData(WinToBossOneNPC);
                break;
            case LevelManager.Level.Bikerman:
                InterfaceSystem.Instance.SetDialogData(DeathToBossTwoNPC);
                break;
            case LevelManager.Level.Headquarters3:
                InterfaceSystem.Instance.SetDialogData(WinToBossTwoNPC);
                break;
            case LevelManager.Level.Turtleman:
                InterfaceSystem.Instance.SetDialogData(DeathToBossThreeNPC);
                break;
            case LevelManager.Level.Headquarters4:
                InterfaceSystem.Instance.SetDialogData(WinToBossThreeNPC);
                break;
        }
    }

    void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayer))
        {
            cursorImage.sprite = interactCursor;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                Debug.Log("Dialogo iniciado");
                InterfaceSystem.Instance.InitDialog();
            }

        }
        else
        {
            cursorImage.sprite = defaultCursor;
        }
    }
}
