using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public bool invertYAxis = false;
    public bool CRTFilter = false;
    public UniversalRendererData URPData;

    public static GameSettings Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    public void ToggleIntertYAxis()
    {
        invertYAxis = !invertYAxis;
    }

    public void ToggleCRTFilter()
    {
        CRTFilter = !CRTFilter;
        foreach (var item in URPData.rendererFeatures)
        {
            if(item.name == "FullScreenPassRendererFeature")
            {
                item.SetActive(!CRTFilter);
            }
        }
    }
}
