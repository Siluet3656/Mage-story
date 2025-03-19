using UnityEngine;

public enum SpellType
    {
        NoSpell,
        Fireball,
        Zap,
        Frost_whirlwind,
        Spike,
        Boom,
        Firewall
    }

public class SpellTypeData : MonoBehaviour
{
    [SerializeField] private Sprite basic;
    [Header("FIREBALL")]
    [SerializeField] private Sprite fireballIcon;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballDamage;
    [SerializeField] private float fireballCD;
    [SerializeField] private float fireballCastTime;
    [SerializeField] private Vector3Int firaballCost;
    [Header("FROST_WHIRLWIND")]
    [SerializeField] private Sprite frost_whirlwindIcon;
    [SerializeField] private GameObject frost_whirlwindPrefab;
    [SerializeField] private float frost_whirlwindDamage;
    [SerializeField] private float frost_whirlwindCD;
    [SerializeField] private float frost_whirlwindCastTime;
    [SerializeField] private Vector3Int frost_whirlwindCost;
    [Header("SPIKE")]
    [SerializeField] private Sprite spikeIcon;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private float spikeDamage;
    [SerializeField] private float spikeCD;
    [SerializeField] private float spikeCastTime;
    [SerializeField] private Vector3Int spikeCost;
    [Header("ZAP")]
    [SerializeField] private Sprite zapIcon;
    [SerializeField] private GameObject zapPrefab;
    [SerializeField] private float zapDamage;
    [SerializeField] private float zapCD;
    [SerializeField] private float zapCost;
    [Header("Boom")]
    [SerializeField] private Sprite boomIcon;
    [SerializeField] private GameObject boomPrefab;
    [SerializeField] private float boomDamage;
    [SerializeField] private float boomCD;
    [SerializeField] private Vector3Int boomCost;
    [Header("Firewall")]
    [SerializeField] private Sprite firewallIcon;
    [SerializeField] private GameObject firewallPrefab;
    [SerializeField] private float firewallDamage;
    [SerializeField] private float firewallCD;
    [SerializeField] private float firewallCastTime;
    [SerializeField] private Vector3Int firewallCost;
    
    public SpellData GetDataByType(SpellType type)
    {
        switch (type)
        {
            case SpellType.Fireball:
                return new SpellData(fireballIcon,fireballPrefab,fireballDamage,fireballCD,fireballCastTime,firaballCost);
            case SpellType.Frost_whirlwind:
                return new SpellData(frost_whirlwindIcon,frost_whirlwindPrefab,frost_whirlwindDamage,frost_whirlwindCD,frost_whirlwindCastTime,frost_whirlwindCost);
            case SpellType.Spike:
                return new SpellData(spikeIcon,spikePrefab,spikeDamage,spikeCD,spikeCastTime,spikeCost);
            case SpellType.Zap:
                return new SpellData(zapIcon,zapPrefab,zapDamage,zapCD,0,zapCost);
            case SpellType.Boom:
                return new SpellData(boomIcon,boomPrefab,boomDamage,boomCD,0,boomCost);
            case SpellType.Firewall:
                return new SpellData(firewallIcon,firewallPrefab,firewallDamage,firewallCD,firewallCastTime,firewallCost);
        }
        return new SpellData(basic,new GameObject("Null"),0,0,0,0);
    }
}