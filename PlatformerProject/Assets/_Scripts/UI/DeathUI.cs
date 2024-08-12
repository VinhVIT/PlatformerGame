using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private Animator anim;
    private void Start()
    {
        playerStats.Health.OnCurrentValueZero += HandlePlayerDeath;
        anim = GetComponentInChildren<Animator>();
    }
    private void OnDisable()
    {
        playerStats.Health.OnCurrentValueZero -= HandlePlayerDeath;
    }
    private void HandlePlayerDeath()
    {
        anim.SetBool("On", true);
    }
}
