using UnityEngine;
using System;

public class FPSManager : Singleton<LifeManager>
{
    [SerializeField] private int targetFPS = 60;

    void Awake()
    {
        // 1. Disable VSync (Required, otherwise targetFrameRate is ignored)
        QualitySettings.vSyncCount = 0;

        // 2. Set the target frame rate
        Application.targetFrameRate = targetFPS;
    }
}
