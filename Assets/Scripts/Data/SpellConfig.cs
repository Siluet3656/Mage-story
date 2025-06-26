using Data.Enums;
using UnityEngine;

namespace Data
{
    public class SpellConfig
    {
        public Sprite Icon { get; set; }
        public Vector3Int ShardCost { get; set; }
        public float ReminderCost { get; set; }
        public float CastTime { get; set; }
        public float Cooldown { get; set; }
        public Color CastBarColor { get; set; }
        public bool RequireTarget { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalMultiply { get; set; }
        public float Damage { get; set; }
        public SpellType Type { get; set; }
    }
}