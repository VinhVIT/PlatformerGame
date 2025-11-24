using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] private List<SkillButton> skillTreeList;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [Header("Skill Info")]
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI SkillDescriptionText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI energyCostText;
    [SerializeField] private Animator skillVideoAnimator;
    [SerializeField] private RuntimeAnimatorController noneSkillAC;
    private PlayerSkills playerSkills;
    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        foreach (SkillButton skillButton in skillTreeList)
        {
            skillButton.Initialize(playerSkills);
            skillButton.SetSkillTreeUI(this);
        }

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnSKillPointsChanged += PlayerSkills_OnSKillPointsChanged;

        UpdateVisuals();
        UpdateSkillPoints();
        UpdateSkillInfoUI(skillTreeList[0].GetSkillData());
    }

    private void PlayerSkills_OnSKillPointsChanged(object sender, EventArgs e)
    {
        UpdateSkillPoints();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.UnlockSkillEventArgs e)
    {
        UpdateVisuals();
    }
    private void UpdateSkillPoints()
    {
        skillPointsText.SetText(playerSkills.GetSkillPoints().ToString());
    }
    private void UpdateVisuals()
    {
        foreach (SkillButton skillButton in skillTreeList)
        {
            skillButton.UpdateVisual();
        }
        //apply skill path color
        foreach (SkillButton skillButton in skillTreeList)
        {
            skillButton.SetSkillPathColor();
        }
    }
    public void UpdateSkillInfoUI(SkillSO skillData)
    {
        skillNameText.SetText(skillData.skillName);
        SkillDescriptionText.SetText(skillData.skillDescription);
        statusText.SetText(skillData.skillStatus.ToString());
        energyCostText.SetText(skillData.EnergyCost.ToString());
        if(skillData.skillAnimatorController == null)
        {
            skillVideoAnimator.runtimeAnimatorController = noneSkillAC;
        }
        else
        {
            skillVideoAnimator.runtimeAnimatorController = skillData.skillAnimatorController;
        }

    }
}

