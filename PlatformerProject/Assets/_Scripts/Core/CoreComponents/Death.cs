using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;
    private ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
    private Stats stats;

    public void Die()
    {
        foreach (var particle in deathParticles)
        {
            ParticleManager.StartParticle(particle);
        }

        core.transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Stats.Health.OnCurrentValueZero += Die;
    }

    private void OnDisable()
    {
        Stats.Health.OnCurrentValueZero -= Die;
    }
}

