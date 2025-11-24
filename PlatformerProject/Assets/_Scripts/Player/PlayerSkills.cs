using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler OnSKillPointsChanged;
    public event EventHandler<UnlockSkillEventArgs> OnSkillUnlocked;
    public class UnlockSkillEventArgs : EventArgs
    {
        public SkillType skillType;
    }
    public enum SkillType
    {
        None,
        HolySlash, HolySlash_1,
        LightCutter, LightCutter_1,
        HP_1, HP_2,
        Stamina_1, Stamina_2,
        XP_1, XP_2,
        EGain_1, Energy,
        Damage_1, Damage_2,
        AttackBuff,
        DefenseBuff,
        DownWardStrike,
        Block_1,
        BlockTimer,
        BCDmg_1, BCDmg_2,
        Movement_1, Movement_2,


    }
    private List<SkillType> unlockedSkills;
    private int skillPoints = 20;
    public PlayerSkills()
    {
        unlockedSkills = new List<SkillType>();
    }
    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkills.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new UnlockSkillEventArgs { skillType = skillType });
        }
    }
    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkills.Contains(skillType);
    }
    public bool CanUnlock(SkillType skillType)
    {
        List<SkillType> skillRequirements = GetSkillRequirement(skillType);

        if (skillRequirements.Count == 0)
        {
            return true;
        }
        foreach (SkillType requirement in skillRequirements)
        {
            if (IsSkillUnlocked(requirement))
            {
                return true; 
            }
        }

        return false;
    }
    public List<SkillType> GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.EGain_1: return new List<SkillType> { SkillType.XP_1 };
            case SkillType.Energy: return new List<SkillType> { SkillType.EGain_1 };
            case SkillType.Damage_1: return new List<SkillType> { SkillType.XP_1 };
            case SkillType.Damage_2: return new List<SkillType> { SkillType.Damage_1 };
            case SkillType.AttackBuff: return new List<SkillType> { SkillType.Energy };
            case SkillType.HolySlash: return new List<SkillType> { SkillType.Damage_2 };
            case SkillType.HolySlash_1: return new List<SkillType> { SkillType.HolySlash };

            case SkillType.Block_1: return new List<SkillType> { SkillType.HP_1 };
            case SkillType.BlockTimer: return new List<SkillType> { SkillType.Block_1 };
            case SkillType.HP_2: return new List<SkillType> { SkillType.Block_1 };
            case SkillType.BCDmg_1: return new List<SkillType> { SkillType.BlockTimer };
            case SkillType.BCDmg_2: return new List<SkillType> { SkillType.HP_2 };
            case SkillType.DownWardStrike: return new List<SkillType> { SkillType.BCDmg_1 };
            case SkillType.DefenseBuff: return new List<SkillType> { SkillType.BCDmg_2 };

            case SkillType.XP_2: return new List<SkillType> { SkillType.Movement_1, SkillType.Stamina_1 };
            case SkillType.Movement_2: return new List<SkillType> { SkillType.XP_2 };
            case SkillType.Stamina_2: return new List<SkillType> { SkillType.XP_2 };
            case SkillType.LightCutter: return new List<SkillType> { SkillType.Movement_2, SkillType.Stamina_2 };
            case SkillType.LightCutter_1: return new List<SkillType> { SkillType.LightCutter };


            default: return new List<SkillType>();
        }
    }
    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            if (skillPoints > 0)
            {
                skillPoints--;
                OnSKillPointsChanged?.Invoke(this, EventArgs.Empty);
                UnlockSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public int GetSkillPoints() => skillPoints;
    public bool EnoughSkillPoints() => skillPoints > 0;
}
