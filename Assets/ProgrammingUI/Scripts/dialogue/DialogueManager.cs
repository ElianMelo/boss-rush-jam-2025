using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Image portraitLeft;
    public Image portraitRight;
    public TextMeshProUGUI textUI;
    public TextMeshProUGUI skipUI;
    public TextMeshProUGUI closeUI;
    public DialogData dialogData;
    public float letterDelay;

    private int currentDialogIndex;
    private Dialog currentDialog;
    private bool isWriting = false;
    private IEnumerator typeWriterEffectCoroutine;
    private InterfaceSystem interfaceSystem;

    private void Start()
    {
        interfaceSystem = GetComponentInParent<InterfaceSystem>();
    }

    public void InitDialog(DialogData dialogData)
    {
        if (HeadquartersMananger.Instance.CurrentState == HeadquartersState.Paused) return;
        HeadquartersMananger.Instance.ChangeHeadquartersState(HeadquartersState.Talking);
        this.dialogData = dialogData;
        //Debug.Log(dialogData.dialogs.Count);
        Reset();
    }

    public void Reset()
    {
        portraitLeft.sprite = dialogData.leftPortrait;
        portraitRight.sprite = dialogData.rightPortrait;
        textUI.text = "";
        currentDialog = null;
        currentDialogIndex = -1;
        NextDialog();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Reset();

            //SoundEffectManager.Instance.StopDialogSfx();
            HeadquartersMananger.Instance.ChangeHeadquartersState(HeadquartersState.Walking);
            this.gameObject.SetActive(false);
            return;
        }

        if (currentDialog != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isWriting)
                {
                    StopCoroutine(typeWriterEffectCoroutine);
                    skipUI.text = "SPACE - Next";
                    textUI.text = "";
                    textUI.text = currentDialog.text;
                    //SoundEffectManager.Instance.StopDialogSfx();
                    isWriting = false;
                }
                else
                {
                    NextDialog();
                }
            }
        }
    }

    private void NextDialog()
    {
        currentDialogIndex += 1;

        if (currentDialogIndex >= dialogData.dialogs.Count)
        {
            Reset();

            //SoundEffectManager.Instance.StopDialogSfx();
            HeadquartersMananger.Instance.ChangeHeadquartersState(HeadquartersState.Walking);
            this.gameObject.SetActive(false);
            return;
        }

        currentDialog = dialogData.dialogs[currentDialogIndex];
        typeWriterEffectCoroutine = TypeWriterEffect();
        StartCoroutine(typeWriterEffectCoroutine);

        // Portrait transparency
        if (currentDialog.isBothPortrait)
        {
            Color colorLeft = portraitLeft.color;
            colorLeft.a = 1f;
            portraitLeft.color = colorLeft;

            Color colorRight = portraitRight.color;
            colorRight.a = 1f;
            portraitRight.color = colorRight;
        }
        else
        {
            Color colorLeft = portraitLeft.color;
            colorLeft.a = currentDialog.isFirstPortrait ? 1f : 0.3f;
            portraitLeft.color = colorLeft;

            Color colorRight = portraitRight.color;
            colorRight.a = currentDialog.isFirstPortrait ? 0.3f : 1f;
            portraitRight.color = colorRight;
        }

        closeUI.gameObject.SetActive(true);
    }

    public IEnumerator TypeWriterEffect()
    {
        char[] chars = currentDialog.text.ToCharArray();
        textUI.text = "";
        isWriting = true;
        //SoundEffectManager.Instance.PerformDialogSfx();

        foreach (var ch in chars)
        {
            textUI.text += ch;
            yield return new WaitForSeconds(letterDelay);
        }

        isWriting = false;
        //SoundEffectManager.Instance.StopDialogSfx()
        yield return null;
    }
}