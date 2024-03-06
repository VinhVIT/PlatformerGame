using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticle;
    private ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    private Stats Stats => stats ?? core.GetCoreComponent(ref stats);
    private Stats stats;
    public void Die()
    {
        foreach (var particle in deathParticle)
        {
            ParticleManager.StartParticle(particle);
        }
        core.transform.parent.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Stats.OnHealthZero += Die;
    }
    private void OnDisable()
    {
        Stats.OnHealthZero -= Die;
    }
}
