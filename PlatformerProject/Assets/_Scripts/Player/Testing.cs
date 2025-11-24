using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerSkillHandler playerSkillHandler;
    [SerializeField] private SkillTreeUI skillTreeUI;
    private void Start()
    {
        skillTreeUI.SetPlayerSkills(playerSkillHandler.PlayerSkills);
    }
}
