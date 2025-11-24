using System;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip_Warning : MonoBehaviour {

    private static Tooltip_Warning instance;
    
    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private Text tooltipText;
    private Image backgroundImage;
    private RectTransform backgroundRectTransform;
    private Func<string> getTooltipStringFunc;
    private float showTimer;
    private float flashTimer;
    private int flashState;
    private Vector2 fixedPosition;

    private void Awake() {
        instance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();
        backgroundImage = transform.Find("background").GetComponent<Image>();

        HideTooltip();
    }

    private void Update() {
        if (showTimer <= 0f) {
            HideTooltip();
        } else {
            showTimer -= Time.deltaTime;

            flashTimer += Time.deltaTime;
            float flashTimerMax = .033f;
            if (flashTimer > flashTimerMax) {
                flashTimer = 0f;
                flashState++;
                switch (flashState) {
                case 1:
                case 3:
                case 5:
                    tooltipText.color = new Color(255, 219, 0, 155);
                    backgroundImage.color = new Color(128f/255f, 128f/255f, 128f/255f, 1);
                    break;
                case 2:
                case 4:
                    tooltipText.color = new Color(128f/255f, 128f/255f, 128f/255f, 1);
                    backgroundImage.color = new Color(255, 219, 0, 155);
                    break;
                }
            }
        }
    }

    private void ShowTooltip(string tooltipString, float showTimerMax = 1f) {
        ShowTooltip(() => tooltipString, showTimerMax);
    }

    private void ShowTooltip(Func<string> getTooltipStringFunc, float showTimerMax = 1f) {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        this.getTooltipStringFunc = getTooltipStringFunc;
        SetText(getTooltipStringFunc());
        showTimer = showTimerMax;
        flashTimer = 0f;
        flashState = 0;

        SetFixedPosition();
    }

    private void SetFixedPosition() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, uiCamera, out fixedPosition);
        transform.GetComponent<RectTransform>().anchoredPosition = fixedPosition;
    }

    private void SetText(string tooltipString) {
        tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip() {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString) {
        instance.ShowTooltip(tooltipString);
    }

    public static void ShowTooltip_Static(Func<string> getTooltipStringFunc) {
        instance.ShowTooltip(getTooltipStringFunc);
    }

    public static void HideTooltip_Static() {
        instance.HideTooltip();
    }
}