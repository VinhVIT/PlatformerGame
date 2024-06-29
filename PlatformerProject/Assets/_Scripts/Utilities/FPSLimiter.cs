using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{   
    [SerializeField] private int maxFPS = 60;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxFPS;
    }

}
