using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler<UnlockSkillEventArgs> OnSkillUnlocked;
    public class UnlockSkillEventArgs : EventArgs
    {
        public SkillType skillType;
    }
    public enum SkillType
    {
        None,
        HolySlash,
        LightCutter,
        MaxHP_1,
        Stamina_1,
    }
    private List<SkillType> unlockedSkills;
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
            Debug.Log("Unlocked skill: " + skillType);
        }
    }
    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkills.Contains(skillType);
    }
    public bool CanUnlock(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);
        if (skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.HolySlash: return SkillType.Stamina_1;
            case SkillType.LightCutter: return SkillType.MaxHP_1;
        }
        return SkillType.None;
    }
    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            UnlockSkill(skillType);
            return true;
        }
        else
        {
            return false;
        }
    }

}
