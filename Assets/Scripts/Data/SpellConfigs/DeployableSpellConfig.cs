using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "DeployableSpell", menuName = "Spells/DeployableSpell", order = 52)]
    public class DeployableSpellConfig : SpellConfig, IDeploy, ICast, INeedPrefab
    {
        [Header("Deploying")] 
        [SerializeField] private Color _ghostColor;
        [SerializeField] private Sprite _deployedSprite;
        [SerializeField, Min(0.1f)] private float _duration;

        [Header("Cast")] 
        [SerializeField, Min(0)] private float _castTime;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private Color _castBarColor;
        
        [Header("Spell")]
        [SerializeField, Min(0)] private float _damage;
        [SerializeField, Min(1)] private float _scaleFactor;
        
        public override SpellType GetSPellType() => SpellType.PlacedSpell;
        public Color GetGhostColor() => _ghostColor;
        public Sprite DeployedSprite => _deployedSprite;
        public float GetCastTime() => _castTime;
        public float GetCooldown() => _cooldown;
        public Color GetCastBarColor() => _castBarColor;
        public float Damage => _damage;
        public float ScaleFactor => _scaleFactor;
        public float Duration => _duration;
    }
}
