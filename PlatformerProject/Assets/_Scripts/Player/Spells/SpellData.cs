using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 [CreateAssetMenu(menuName = "Spell/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public float manaCost = 10f;
    public float damage = 10f;
    public float cooldownTime = 3f;
    public GameObject spellPrefab;
    public Element spellElement;
    public SpellType spellType;
    public virtual void Cast(Vector3 initPos, Quaternion rotation)
    {

    }
    public virtual bool CanCast()
    {
        // Thêm logic kiểm tra như mana, cooldown, v.v. ở đây
        return true; // Mặc định cho phép cast
    }

}
public enum Element
{
    Fire,
    Water,
    Earth,
    Air,
    Arcane,
}
public enum SpellType
{
    OneTime,
    Area,
    Target,
    NoneDamage
}
