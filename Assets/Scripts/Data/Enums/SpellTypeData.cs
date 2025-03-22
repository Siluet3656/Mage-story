using UnityEngine;

public enum SpellType
    {
        NoSpell,
        Fireball,
        Zap,
        Frost_whirlwind,
        Spike,
        Boom,
        Firewall,
        Firespirit,
        Firelaser,
        Fireaura,
        Firemark
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
    [Header("BOOM")]
    [SerializeField] private Sprite boomIcon;
    [SerializeField] private GameObject boomPrefab;
    [SerializeField] private float boomDamage;
    [SerializeField] private float boomCD;
    [SerializeField] private Vector3Int boomCost;
    [Header("FIREWALL")]
    [SerializeField] private Sprite firewallIcon;
    [SerializeField] private GameObject firewallPrefab;
    [SerializeField] private float firewallDamage;
    [SerializeField] private float firewallCD;
    [SerializeField] private float firewallCastTime;
    [SerializeField] private Vector3Int firewallCost;
    [Header("FIRESPIRIT")]
    [SerializeField] private Sprite firespiritIcon;
    [SerializeField] private GameObject firespiritPrefab;
    [SerializeField] private float firespiritDamage;
    [SerializeField] private float firespiritCD;
    [SerializeField] private float firespiritCastTime;
    [SerializeField] private Vector3Int firespiritCost;
    [Header("FIRELASER")]
    [SerializeField] private Sprite firelaserIcon;
    [SerializeField] private GameObject firelaserPrefab;
    [SerializeField] private float firelaserDamage;
    [SerializeField] private float firelaserCD;
    [SerializeField] private float firelaserCastTime;
    [SerializeField] private Vector3Int firelaserCost;
    [Header("FIREAURA")]
    [SerializeField] private Sprite fireauraIcon;
    [SerializeField] private GameObject fireauraPrefab;
    [SerializeField] private float fireauraDamage;
    [SerializeField] private float fireauraCD;
    [SerializeField] private Vector3Int fireauraCost;
    [Header("FIREMARK")]
    [SerializeField] private Sprite firemarkIcon;
    [SerializeField] private GameObject firemarkPrefab;
    [SerializeField] private float firemarkDamage;
    [SerializeField] private float firemarkCD;
    [SerializeField] private float firemarkCastTime;
    [SerializeField] private Vector3Int firemarkCost;
    
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
            case SpellType.Firespirit:
                return new SpellData(firespiritIcon,firespiritPrefab,firespiritDamage,firespiritCD,firespiritCastTime,firespiritCost);
            case SpellType.Firelaser:
                return new SpellData(firelaserIcon,firelaserPrefab,firelaserDamage,firelaserCD,firelaserCastTime,firelaserCost);
            case SpellType.Fireaura:
                return new SpellData(fireauraIcon,fireauraPrefab,fireauraDamage,fireauraCD,0,fireauraCost);
            case SpellType.Firemark:
                return new SpellData(firemarkIcon,firemarkPrefab,firemarkDamage,firemarkCD,firemarkCastTime,firemarkCost);
        }
        return new SpellData(basic,new GameObject("Null"),0,0,0,0);
    }
}