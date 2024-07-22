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
    public bool moveAble;
    [ConditionalHide("moveAble", true)]
    public float moveSpeed = 10f;
    public GameObject spellPrefab;
    public Element spellElement;
    public CastType castType;
    public DamageType damageType;
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
public enum CastType
{
    Infront,
    OnGround,
    EnemyPos
}
public enum DamageType
{
    Single,
    Multiple
}
