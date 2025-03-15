using UnityEngine;

public class SpellData
{
    public Sprite Icon {private set; get; }
    public GameObject PrefubOfSpell {private set; get; }
    public float Damage {private set; get; }
    public float Cd {private set; get; }
    public float CastTime { private set; get; }
    public Vector3Int ShardsCost {private set; get; }
    public float ReminderCost {private set; get; }

    public SpellData(Sprite icon,GameObject prefubOfSpell, float damage, float cd, float castTime ,Vector3Int shardsCostcost)
    {
        Icon = icon;
        Damage = damage;
        Cd = cd;
        ShardsCost = shardsCostcost;
        CastTime = castTime;
        PrefubOfSpell = prefubOfSpell;
    }
    
    public SpellData(Sprite icon, GameObject prefubOfSpell, float damage, float cd, float castTime , float reminderCost)
    {
        Icon = icon;
        Damage = damage;
        Cd = cd;
        ReminderCost = reminderCost;
        CastTime = castTime;
        PrefubOfSpell = prefubOfSpell;
    }
}
