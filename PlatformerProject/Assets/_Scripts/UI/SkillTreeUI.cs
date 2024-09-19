using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] private Material skillLockMaterial;
    [SerializeField] private Material skillUnlockMaterial;
    private PlayerSkills playerSkills;
    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        UpdateVisuals();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.UnlockSkillEventArgs e)
    {
        UpdateVisuals();
    }

    public void UnlockHolySlash()
    {
        if (!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.HolySlash))
        {
            Tooltip_Warning.ShowTooltip_Static("Cannot unlock!");
        }

    }
    public void UnlockLightCutter()
    {
        if (!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.LightCutter))
        {
            Tooltip_Warning.ShowTooltip_Static("Cannot unlock!");
        }
    }

    public void UnlockMaxHP_1() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.MaxHP_1);
    public void UnlockStamina_1() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Stamina_1);

    private void UpdateVisuals()
    {
        if (playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.HolySlash))
        {
            transform.Find("HolySlash").GetComponent<Image>().material = null;
        }
        else
        {
            if (playerSkills.CanUnlock(PlayerSkills.SkillType.HolySlash))
            {
                transform.Find("HolySlash").GetComponent<Image>().material = skillUnlockMaterial;
            }
            else
            {
                transform.Find("HolySlash").GetComponent<Image>().material = skillLockMaterial;
            }
        }
    }
}
