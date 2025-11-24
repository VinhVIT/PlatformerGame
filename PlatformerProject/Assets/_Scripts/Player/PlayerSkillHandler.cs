using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private Player player;
    public PlayerSkills PlayerSkills { get; private set; }
    private PlayerStats PlayerStats => playerStats ?? player.Core.GetCoreComponent(ref playerStats);
    private PlayerStats playerStats;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        PlayerSkills = new PlayerSkills();
    }
    private void Start()
    {
        PlayerSkills.OnSkillUnlocked += HandlerOnSkillUnlocked;

    }
    private void HandlerOnSkillUnlocked(object sender, PlayerSkills.UnlockSkillEventArgs e)
    {
        if (e.skillType == PlayerSkills.SkillType.HP_1 || e.skillType == PlayerSkills.SkillType.HP_2)
            IncreaseMaxAmount(PlayerStats.Health, 1);
        else if (e.skillType == PlayerSkills.SkillType.Stamina_1 || e.skillType == PlayerSkills.SkillType.Stamina_2)
            IncreaseMaxAmount(PlayerStats.Stamina, 15);
        else if (e.skillType == PlayerSkills.SkillType.EGain_1)
            IncreaseEnergyGain();
        else if (e.skillType == PlayerSkills.SkillType.Energy)
            IncreaseMaxAmount(PlayerStats.Energy, 20);
        else if (e.skillType == PlayerSkills.SkillType.Damage_1 || e.skillType == PlayerSkills.SkillType.Damage_2)
            IncreaseAttackDamage();
        else if (e.skillType == PlayerSkills.SkillType.BCDmg_1 || e.skillType == PlayerSkills.SkillType.BCDmg_2)
            IncreaseBlockCounterBonusDmg();
        else if (e.skillType == PlayerSkills.SkillType.Movement_1 || e.skillType == PlayerSkills.SkillType.Movement_2)
            IncreaseMovementVelocity();
        // else if (e.skillType == PlayerSkills.SkillType.XP_1 || e.skillType == PlayerSkills.SkillType.XP_2)
        //     // IncreaseMaxAmount(PlayerStats.Damage, 1);
        else if (e.skillType == PlayerSkills.SkillType.HolySlash_1)
            IncreaseHolySlashDamage();
        else if (e.skillType == PlayerSkills.SkillType.LightCutter_1)
            IncreaseLightCutterDamage();
        else if (e.skillType == PlayerSkills.SkillType.Block_1)
            ReduceBlockStamina();
        else if (e.skillType == PlayerSkills.SkillType.BlockTimer)
            IncreasePerfectBlockTime();
    }
    private void IncreaseMaxAmount(Stat stat, int amount) => stat.SetMaxValue(stat.MaxValue + amount);
    private void IncreaseEnergyGain()
    {
        player.AirAttackState.IncreaseEneryGain(5);
        player.GroundAttackState.IncreaseEneryGain(5);
    }
    private void IncreaseAttackDamage()
    {
        player.AirAttackState.IncreaseAttackBonus(5);
        player.GroundAttackState.IncreaseAttackBonus(5);
    }
    private void IncreaseBlockCounterBonusDmg()
    {
        player.BlockCounterState.IncreaseAttackBonus(5);
    }
    private void IncreaseMovementVelocity()
    {
        Debug.Log(player.MoveState.GetMovementVelocity());

        player.MoveState.IncreaseMovementVelocity(1);
        Debug.Log(player.MoveState.GetMovementVelocity());
    }
    private void ReduceBlockStamina()
    {
        player.BlockState.ReduceBlockStamina(5);
    }
    private void IncreasePerfectBlockTime()
    {
        player.BlockState.IncreaseBlockTime(0.2f);
    }
    public bool CanUseHolySlash() => PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.HolySlash);
    public void IncreaseHolySlashDamage() => player.HolySlashState.IncreaseAttackBonus(5);
    public bool CanUseLightCutter() => PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.LightCutter);
    public void IncreaseLightCutterDamage() => player.LightCutAttackState.IncreaseAttackBonus(5);
    public bool CanUseDownwardAttack() => PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.DownWardStrike);
    public bool CanUseAttackBuff() => PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.AttackBuff);
    public bool CanUseDefenseBuff() => PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.DefenseBuff);


}
