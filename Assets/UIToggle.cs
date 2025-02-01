using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{
    public Sprite filledToggle;
    public Sprite emptyToggle;
    public Image toggleButton;
    
    public void ToggleYAxisUI()
    {
        GameSettings.Instance.ToggleIntertYAxis();
        if(GameSettings.Instance.invertYAxis)
        {
            toggleButton.sprite = filledToggle;
        } else
        {
            toggleButton.sprite = emptyToggle;
        }
    }

    public void ToggleCRTFilter()
    {
        GameSettings.Instance.ToggleCRTFilter();
        if (GameSettings.Instance.CRTFilter)
        {
            toggleButton.sprite = filledToggle;
        }
        else
        {
            toggleButton.sprite = emptyToggle;
        }
    }
}
