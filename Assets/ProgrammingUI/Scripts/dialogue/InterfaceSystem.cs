using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSystem : MonoBehaviour
{
    public static InterfaceSystem Instance;

    public DialogManager dialogManager;

    private DialogData dialogData;

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
}
