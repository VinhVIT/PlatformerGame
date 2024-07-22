using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticle;
    private ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
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
        EventManager.Player.OnZeroHealth += Die;
    }
    private void OnDisable()
    {
        EventManager.Player.OnZeroHealth -= Die;
    }
}
