using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newdeadStateData", menuName = "Data/State Data/Dead State")]
public class D_DeadState : ScriptableObject
{
    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;

    public GameObject energyPrefab;
    public int energyCount = 2;
    public float eruptionForce = 2f;
    public float spreadAngle = 90f;
}
