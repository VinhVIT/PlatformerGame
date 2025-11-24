using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "new Skill Data", menuName = "Data/SkillData")]
public class SkillSO : ScriptableObject
{   
    public string skillName;
    public string skillDescription;
    public SkillStatus skillStatus;
    public int EnergyCost = 0;
    public RuntimeAnimatorController skillAnimatorController; 
    public Color skillUnlockedColor;
    public Color skillPathUnlockedColor;

}
public enum SkillStatus
{
    Locked,
    Available,
    Learned
}
