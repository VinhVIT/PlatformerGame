using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SkillTreeUI skillTreeUI;
    private void Start()
    {
        skillTreeUI.SetPlayerSkills(player.PlayerSkills);
    }
}
