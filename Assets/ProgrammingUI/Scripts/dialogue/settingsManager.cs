using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsManager : MonoBehaviour
{
    public void OnBackClick()
    {
        InterfaceSystem.Instance.BackToMenu();
    }
}
