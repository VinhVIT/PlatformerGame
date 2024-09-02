using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [field: SerializeField] public Stat Stamina { get; private set; }
    [field: SerializeField] public Stat Energy { get; private set; }
    [SerializeField] private int staminaRecoveryRate = 5;
    [SerializeField] private float staminaRecoveryDelay = 2f;


    protected override void Awake()
    {
        base.Awake();
        Stamina.Init();
        Energy.InitZero();
    }

    private void Update()
    {
        if (Time.time >= Stamina.LastDecreaseTime + staminaRecoveryDelay)
        {
            Stamina.ContinuousIncrease(staminaRecoveryRate, .1f);
        }
    }
}

