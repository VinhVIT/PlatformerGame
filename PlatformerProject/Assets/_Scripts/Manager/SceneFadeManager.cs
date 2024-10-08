using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance;
    [SerializeField] private Image fadeOutImage;
    [Range(0.1f, 10f), SerializeField] private float fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float fadeInSpeed = 5f;
    [SerializeField] private Color fadeColor;
    public bool IsFadingOut { get; set; }
    public bool IsFadingIn { get; set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        fadeColor.a = 0f;
    }
    private void Update()
    {   
        if (IsFadingOut)
        {
            if (fadeOutImage.color.a < 1f)
            {
                fadeColor.a += Time.deltaTime * fadeOutSpeed;
                fadeOutImage.color = fadeColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }
        if (IsFadingIn)
        {
            if (fadeOutImage.color.a > 0f)
            {
                fadeColor.a -= Time.deltaTime * fadeInSpeed;
                fadeOutImage.color = fadeColor;
            }
            else
            {   
                IsFadingIn = false;
            }
        }
    }
    public void StartFadeOut()
    {
        fadeOutImage.color = fadeColor;
        IsFadingOut = true;
    }
    public void StartFadeIn()
    {
        if (fadeOutImage.color.a >= 1f)
        {
            fadeOutImage.color = fadeColor;
            IsFadingIn = true;
        }

    }
}
