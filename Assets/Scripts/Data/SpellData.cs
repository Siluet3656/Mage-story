using System.Collections.Generic;
using Data.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    public static class SpellData
    {   
        private static readonly Dictionary<SpellType, SpellConfig> SpellValues = new Dictionary<SpellType, SpellConfig>
        {
            // Примеры конфигураций заклинаний
            {
                SpellType.Fireball,
                new SpellConfig
                {
                    Icon = Resources.Load<Image>("Spells/Icons/Fireball"),
                    ShardCost = new Vector3Int(1, 0, 0), 
                    ReminderCost = 20f, 
                    CastTime = 1.5f, 
                    Cooldown = 3f,
                    CastBarColor = new Color(1f, 0.3f, 0.1f) 
                }
            },
            {
                SpellType.Zap,
                new SpellConfig
                {
                    Icon = Resources.Load<Image>("Spells/Icons/Zap"),
                    ShardCost = new Vector3Int(0, 1, 0),
                    ReminderCost = 15f,
                    CastTime = 0.5f,
                    Cooldown = 2f,
                    CastBarColor = new Color(0.9f, 0.9f, 0.1f)
                }
            },
            {
                SpellType.FrostWhirlwind,
                new SpellConfig
                {
                    Icon = Resources.Load<Image>("Spells/Icons/FrostWhirlwind"),
                    ShardCost = new Vector3Int(0, 0, 1),
                    ReminderCost = 30f,
                    CastTime = 2f,
                    Cooldown = 8f,
                    CastBarColor = new Color(0.2f, 0.7f, 1f) 
                }
            }
            
        };

        public static SpellConfig GetSpellConfig(SpellType type)
        {
            return SpellValues.TryGetValue(type, out SpellConfig config) ? config : null;
        }
    }
    
    //  Иконка - Стоимость шарды - Стоимость остаток - Время каста - Время отката - Цвет кастбара
    public class SpellConfig
    {
        public Image Icon { get; set; }
        public Vector3Int ShardCost { get; set; }
        public float ReminderCost { get; set; }
        public float CastTime { get; set; }
        public float Cooldown { get; set; }
        public Color CastBarColor { get; set; }
    }
}
