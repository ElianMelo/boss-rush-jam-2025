using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    public GameObject buttonsObject;
    public GameObject SettingsObject;

    public void OpenSettings()
    {
        buttonsObject.SetActive(false);
        SettingsObject.SetActive(true);
    }

    public void CloseSettings()
    {
        buttonsObject.SetActive(true);
        SettingsObject.SetActive(false);
    }
}
