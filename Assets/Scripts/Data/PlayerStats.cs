using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats", order = 53)]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private float _globalCooldown;
        
        [Header("Fire spells")]
        [SerializeField, Min(1)] private float _fireCriticalMultiplier;
        [SerializeField, Range(0f, 1f)] private float _fireCriticalChance;
        
        [Header("Frost spells")]
        [SerializeField, Min(1)] private float _frostCriticalMultiplier;
        [SerializeField, Range(0f, 1f)] private float _frostCriticalChance;
        
        [Header("Frost spells")]
        [SerializeField, Min(1)] private float _earthCriticalMultiplier;
        [SerializeField, Range(0f, 1f)] private float _earthCriticalChance;
        
        [Header("No element spells")]
        [SerializeField, Min(1)] private float _noElementCriticalMultiplier;
        [SerializeField, Range(0f, 1f)] private float _noElementCriticalChance;

        public float GlobalCooldown => _globalCooldown;
        public float FireCriticalMultiplier => _fireCriticalMultiplier;
        public float FireCriticalChance => _fireCriticalChance;
        public float FrostCriticalMultiplier => _frostCriticalMultiplier;
        public float FrostCriticalChance => _frostCriticalChance;
        public float EarthCriticalMultiplier => _earthCriticalMultiplier;
        public float EarthCriticalChance => _earthCriticalChance;
        public float NoElementCriticalMultiplier => _noElementCriticalMultiplier;
        public float NoElementCriticalChance => _noElementCriticalChance;
    }
}