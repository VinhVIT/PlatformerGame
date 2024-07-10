using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHander : MonoBehaviour
{
    public static SpellHander Instance;
    void Awake()
    {
        Instance = this;
    }
    public void CastSpell(GameObject spellPrefab, Transform caster)
    {
        SpellSetting spellSetting = spellPrefab.GetComponent<SpellSetting>();

        if (spellSetting != null)
        {
            Vector3 castPosition = spellSetting.GetCastPosition(caster);

            if (castPosition != Vector3.zero)
            {
                Instantiate(spellPrefab, castPosition, caster.rotation);
                Debug.Log($"Casting {spellPrefab.name} at {castPosition}");
            }
            else
            {
                Debug.LogError($"Failed to determine cast position for spell {spellPrefab.name}.");
            }
        }
        else
        {
            Debug.LogError($"Spell prefab {spellPrefab.name} does not have a SpellSetting component.");
        }
    }

}

