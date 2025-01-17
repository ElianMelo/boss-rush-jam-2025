using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    private CinemachineImpulseSource CinemachineImpulseSource;

    public static ScreenShakeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeScreen()
    {
        CinemachineImpulseSource.GenerateImpulse();
    }
}
