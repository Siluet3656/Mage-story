using System.Collections.Generic;
using UnityEngine;
using Data.Enums;

namespace Data
{
    public static class SpellData
    {   
        private static readonly Dictionary<SpellName, SpellConfig> SpellValues = new Dictionary<SpellName, SpellConfig>
        {
            {
                SpellName.NoSpell,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/EmptyIconSpot"),
                    ShardCost = new Vector3Int(0, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 0f, 0f),
                    RequireTarget = false,
                    CriticalChance = 0f,
                    CriticalMultiply = 0f,
                    Damage = 0f,
                    Type = SpellType.Unknown
                }
            },
            {
                SpellName.Fireball,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Fireball"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 3f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0.1f, 0.1f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f,
                    Type = SpellType.Projectile
                }
            },
            {
                SpellName.Boom,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Boom"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.8f, 0.3f, 0.1f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.Firewall,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firewall"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 2f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0.3f, 0.1f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.FireSpirit,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firespirit"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.8f, 0.3f, 0.2f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.FireAura,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Fireaura"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.8f, 0.3f, 0.2f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.FireMark,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firemark"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0.5f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0.5f, 0.2f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.FireLaser,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firelaser"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0f, 0.2f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.FrostWhirlwind,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/FrostWhirlwind"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f,
                    Type = SpellType.Projectile
                }
            },
            {
                SpellName.FlashFreeze,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/FlashFreeze"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.FrostAegis,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/FrostWhirlwind"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.StasisFreeze,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/StasisFreeze"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.IcicleBarrage,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/IcicleBarrage"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.CryoLeach,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/CryoLeach"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.1f, 0.4f, 1f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.AvalancheCore,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/AvalancheCore"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.1f, 0f, 1f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.Spike,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/Spike"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 3f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.4f, 1f, 0.2f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f,
                    Type = SpellType.Projectile
                }
            },
            {
                SpellName.EarthShield,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/EarthShield"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.4f, 1f, 0.2f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.DeathZone,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/DeathZone"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.4f, 1f, 0.2f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.NaturePower,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/NaturePower"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.5f, 1f, 0.3f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.Plague,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/Plague"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 0.4f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.Flower,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/Flower"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 2f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.5f, 0.7f, 0.2f),
                    RequireTarget = false,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            },
            {
                SpellName.NatureWraith,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/NatureWraith"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 0.5f, 0f),
                    RequireTarget = true,
                    CriticalChance = 1f,
                    CriticalMultiply = 1f,
                    Damage = 0f
                }
            }
        };

        public static SpellConfig GetSpellConfig(SpellName name)
        {
            return SpellValues.TryGetValue(name, out SpellConfig config) ? config : GetSpellConfig(SpellName.NoSpell);
        }
    }
}
