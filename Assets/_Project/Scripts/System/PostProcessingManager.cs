using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager Instance;

    public Volume volume;
    private Vignette vignette;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        VolumeProfile profile = volume.sharedProfile;
        profile.TryGet<Vignette>(out vignette);
    }

    public void ActivateVignette()
    {
        vignette.active = true;
    }
    public void DeactivateVignette()
    {
        vignette.active = false;
    }
}
