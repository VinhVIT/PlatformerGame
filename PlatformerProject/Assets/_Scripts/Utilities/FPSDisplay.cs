using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float fps;
    [SerializeField] private TMPro.TextMeshProUGUI FPSText;
    private void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }
    private void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSText.text = "FPS: " + fps.ToString();
    }
}
