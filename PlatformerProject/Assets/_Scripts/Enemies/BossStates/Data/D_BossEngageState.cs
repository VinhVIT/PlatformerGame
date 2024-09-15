using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newBossEngageStateData", menuName = "Data/State Data/Boss Engage State")]

public class D_BossEngageState : ScriptableObject
{
    public Vector3 engageSize = new Vector3(1.2f, 1.2f, 1);
    public Material engageMaterial;
}
