using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void CastSpell(GameObject spellPrefab, Vector3 castPosition, Quaternion castRotation)
    {
        Instantiate(spellPrefab, castPosition, castRotation);
    }
}
