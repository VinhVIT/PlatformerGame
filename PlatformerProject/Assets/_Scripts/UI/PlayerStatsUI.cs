using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Player player;
    private PlayerStats playerStats;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI blockText;
    [Header("Buttons Toggle")]
    [SerializeField] private Button healpButton;
    [SerializeField] private GameObject helpPanel;
    private void Awake()
    {
        playerStats = player.GetComponentInChildren<PlayerStats>();
    }
    private void Start()
    {
        playerStats.Health.OnMaxValueChanged += PlayerStats_Health_OnMaxValueChanged;
        playerStats.Stamina.OnMaxValueChanged += PlayerStats_Stamina_OnMaxValueChanged;
        playerStats.Energy.OnMaxValueChanged += PlayerStats_Energy_OnMaxValueChanged;
        player.GroundAttackState.OnAttackBonusChanged += GroundAttackState_OnAttackBonusChanged;
        player.MoveState.OnMovementVelocityChanged += MoveState_OnMovementVelocityChanged;

        UpdateText();
    }

    private void MoveState_OnMovementVelocityChanged() => speedText.text = player.MoveState.GetMovementVelocity().ToString();


    private void GroundAttackState_OnAttackBonusChanged() => damageText.text = player.GroundAttackState.AttackBonus.ToString();


    private void PlayerStats_Energy_OnMaxValueChanged(int obj) => energyText.text = playerStats.Energy.MaxValue.ToString();


    private void PlayerStats_Stamina_OnMaxValueChanged(int obj) => staminaText.text = playerStats.Stamina.MaxValue.ToString();


    private void PlayerStats_Health_OnMaxValueChanged(int value) => healthText.text = playerStats.Health.MaxValue.ToString();

    private void UpdateText()
    {
        healthText.text = playerStats.Health.MaxValue.ToString();
        staminaText.text = playerStats.Stamina.MaxValue.ToString();
        energyText.text = playerStats.Energy.MaxValue.ToString();
        damageText.text = player.GroundAttackState.AttackBonus.ToString();
        speedText.text = player.MoveState.GetMovementVelocity().ToString();
    }
    public void ToggleHealPanel()
    {   
        bool isActive = helpPanel.activeSelf;
        if (!isActive)
        {
            helpPanel.SetActive(true);
        }
        else
        {
            helpPanel.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        playerStats.Health.OnMaxValueChanged -= PlayerStats_Health_OnMaxValueChanged;
        playerStats.Stamina.OnMaxValueChanged -= PlayerStats_Stamina_OnMaxValueChanged;
        playerStats.Energy.OnMaxValueChanged -= PlayerStats_Energy_OnMaxValueChanged;
        player.GroundAttackState.OnAttackBonusChanged -= GroundAttackState_OnAttackBonusChanged;
        player.MoveState.OnMovementVelocityChanged -= MoveState_OnMovementVelocityChanged;
    }
}
