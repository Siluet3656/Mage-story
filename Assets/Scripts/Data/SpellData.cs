using System.Collections.Generic;
using UnityEngine;
using Data.Enums;

namespace Data
{
    public static class SpellData
    {   
        private static readonly Dictionary<SpellType, SpellConfig> SpellValues = new Dictionary<SpellType, SpellConfig>
        {
            {
                SpellType.NoSpell,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/EmptyIconSpot"),
                    ShardCost = new Vector3Int(0, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 0f, 0f) 
                }
            },
            {
                SpellType.Fireball,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Fireball"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 3f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0.1f, 0.1f) 
                }
            },
            {
                SpellType.Boom,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Boom"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.8f, 0.3f, 0.1f) 
                }
            },
            {
                SpellType.Firewall,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firewall"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 2f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0.3f, 0.1f) 
                }
            },
            {
                SpellType.Firespirit,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firespirit"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.8f, 0.3f, 0.2f) 
                }
            },
            {
                SpellType.Fireaura,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Fireaura"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.8f, 0.3f, 0.2f) 
                }
            },
            {
                SpellType.Firemark,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firemark"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0.5f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0.5f, 0.2f) 
                }
            },
            {
                SpellType.Firelaser,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Fire/Firelaser"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(1f, 0f, 0.2f) 
                }
            },
            {
                SpellType.FrostWhirlwind,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/FrostWhirlwind"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f) 
                }
            },
            {
                SpellType.FlashFreeze,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/FlashFreeze"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f) 
                }
            },
            {
                SpellType.FrostAegis,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/FrostWhirlwind"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f) 
                }
            },
            {
                SpellType.StasisFreeze,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/StasisFreeze"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f) 
                }
            },
            {
                SpellType.IcicleBarrage,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/IcicleBarrage"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 1f) 
                }
            },
            {
                SpellType.CryoLeach,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/CryoLeach"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.1f, 0.4f, 1f) 
                }
            },
            {
                SpellType.AvalancheCore,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Frost/AvalancheCore"),
                    ShardCost = new Vector3Int(0, 1, 0), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.1f, 0f, 1f) 
                }
            },
            {
                SpellType.Spike,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/Spike"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 3f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.4f, 1f, 0.2f) 
                }
            },
            {
                SpellType.EarthShield,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/EarthShield"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.4f, 1f, 0.2f) 
                }
            },
            {
                SpellType.DeathZone,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/DeathZone"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 0f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.4f, 1f, 0.2f) 
                }
            },
            {
                SpellType.NaturePower,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/NaturePower"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.5f, 1f, 0.3f) 
                }
            },
            {
                SpellType.Plague,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/Plague"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 1f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 1f, 0.4f) 
                }
            },
            {
                SpellType.Flower,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/Flower"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 2f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0.5f, 0.7f, 0.2f) 
                }
            },
            {
                SpellType.NatureWraith,
                new SpellConfig
                {
                    Icon = Resources.Load<Sprite>("Sprites/UI/SkillIcons/Earth/NatureWraith"),
                    ShardCost = new Vector3Int(0, 0, 1), 
                    ReminderCost = 0f, 
                    CastTime = 4f, 
                    Cooldown = 0f,
                    CastBarColor = new Color(0f, 0.5f, 0f) 
                }
            }
        };

        public static SpellConfig GetSpellConfig(SpellType type)
        {
            return SpellValues.TryGetValue(type, out SpellConfig config) ? config : GetSpellConfig(SpellType.NoSpell);
        }
    }
    
    //  Иконка - Стоимость шарды - Стоимость остаток - Время каста - Время отката - Цвет кастбара
    public class SpellConfig
    {
        public Sprite Icon { get; set; }
        public Vector3Int ShardCost { get; set; }
        public float ReminderCost { get; set; }
        public float CastTime { get; set; }
        public float Cooldown { get; set; }
        public Color CastBarColor { get; set; }
    }
}
