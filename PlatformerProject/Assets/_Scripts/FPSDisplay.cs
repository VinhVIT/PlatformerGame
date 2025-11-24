using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    private TextMeshProUGUI m_Text;
    private float m_DeltaTime;
    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        m_DeltaTime += (Time.unscaledDeltaTime - m_DeltaTime) * 0.1f;
        float fps = 1.0f / m_DeltaTime;
        m_Text.text = $"FPS: {Mathf.Ceil(fps)}";
    }
}