using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : CoreComponent
{   
    public GameObject dustParticle;
    private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();

        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
    }
    public GameObject StartParticle(GameObject particlePrebab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(particlePrebab, position, rotation, particleContainer);
    }
    public GameObject StartParticle(GameObject particlePrefab)
    {
        return Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
    public GameObject StartParicleWithRandomRotation(GameObject particlePrebab)
    {
        return Instantiate(particlePrebab, transform.position,
                Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }
}
