using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Slider staminaWheel;
    [SerializeField] private Slider usageWheel;

    private Quaternion initialRotation;
    private float previousStaminaPercentage;
    private PlayerStats PlayerStats => playerStats ?? player.Core.GetCoreComponent(ref playerStats);
    private PlayerStats playerStats;

    private void Start()
    {
        initialRotation = transform.rotation;
        previousStaminaPercentage = (float)PlayerStats.Stamina.CurrentValue / PlayerStats.Stamina.MaxValue;
        UpdateStaminaUI(); // Ensure UI is updated on start
    }

    private void LateUpdate()
    {
        float staminaPercentage = (float)PlayerStats.Stamina.CurrentValue / PlayerStats.Stamina.MaxValue;

        // Update stamina and usage wheels
        staminaWheel.value = staminaPercentage;

        if (staminaPercentage < previousStaminaPercentage)
        {
            usageWheel.value = staminaPercentage + 0.05f;
        }
        previousStaminaPercentage = staminaPercentage;

        // Update UI visibility based on stamina
        UpdateStaminaUI();

        // Reset rotation
        transform.rotation = initialRotation;
    }

    private void UpdateStaminaUI()
    {
        float staminaPercentage = (float)PlayerStats.Stamina.CurrentValue / PlayerStats.Stamina.MaxValue;

        // Show UI only when stamina is not full
        staminaWheel.gameObject.SetActive(staminaPercentage < 1f);
        usageWheel.gameObject.SetActive(staminaPercentage < 1f);
    }
}
