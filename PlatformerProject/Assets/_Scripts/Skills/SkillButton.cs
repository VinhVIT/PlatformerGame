using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private SkillSO skillData;
    [SerializeField] private Image[] skillPathImage;
    private PlayerSkills.SkillType skillType;
    private Image skillBorder;
    private Image skillImage;
    private Button skillButton;
    private PlayerSkills playerSkills;
    private SkillTreeUI skillTreeUI;

    private void Awake()
    {

        Initial();
    }
    private void Initial()
    {
        skillButton = GetComponent<Button>();
        skillImage = transform.GetChild(0).GetComponent<Image>();
        skillBorder = GetComponent<Image>();

        skillType = (PlayerSkills.SkillType)Enum.Parse(typeof(PlayerSkills.SkillType), gameObject.name);
        SetSkillStatus(SkillStatus.Locked);
    }
    public void Initialize(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        skillButton.onClick.AddListener(() => CheckCanUnlock());
    }
    public void SetSkillTreeUI(SkillTreeUI skillTreeUI)
    {
        this.skillTreeUI = skillTreeUI;
    }
    private void CheckCanUnlock()
    {
        if (!playerSkills.TryUnlockSkill(skillType))
        {
            if (!playerSkills.EnoughSkillPoints())
            {
                Tooltip_Warning.ShowTooltip_Static("Not enough skill points!");
            }
            else
            {
                Tooltip_Warning.ShowTooltip_Static("Cannot unlock!");
            }
        }
        else
        {
            SetSkillStatus(SkillStatus.Learned);
            skillTreeUI.UpdateSkillInfoUI(skillData);
            skillButton.interactable = false;

        }
    }
    public void UpdateVisual()
    {
        if (playerSkills.IsSkillUnlocked(skillType))
        {
            skillImage.material = null;
            skillBorder.color = skillData.skillUnlockedColor;
        }
    }
    public void SetSkillPathColor()
    {
        if (playerSkills.IsSkillUnlocked(skillType) || playerSkills.CanUnlock(skillType))
        {
            foreach (Image pathImage in skillPathImage)
            {

                pathImage.color = skillData.skillPathUnlockedColor;
            }
        }
    }
    private void SetSkillStatus(SkillStatus skillStatus)
    {
        skillData.skillStatus = skillStatus;

    }
    public SkillSO GetSkillData() => skillData;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playerSkills.CanUnlock(skillType) && !playerSkills.IsSkillUnlocked(skillType))
        {
            SetSkillStatus(SkillStatus.Available);
        }

        // Update UI in the SkillTreeUI
        skillTreeUI.UpdateSkillInfoUI(skillData);
    }
}

