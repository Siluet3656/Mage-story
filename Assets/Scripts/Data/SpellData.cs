using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;

namespace Data
{
    public class SpellData : MonoBehaviour
    {
        [Header("Fire")]
        [SerializeField] private SpellConfig _fireball;
        [SerializeField] private SpellConfig _boom;
        [SerializeField] private SpellConfig _fireWall;
        [SerializeField] private SpellConfig _fireAura;
        [SerializeField] private SpellConfig _fireMark;
        [SerializeField] private SpellConfig _fireSpirit;
        [SerializeField] private SpellConfig _fireLaser;
        
        [Header("Frost")]
        [SerializeField] private SpellConfig _frostWhirlwind;
        [SerializeField] private SpellConfig _flashFreeze;
        [SerializeField] private SpellConfig _frostAegis;
        [SerializeField] private SpellConfig _icicleBarrage;
        [SerializeField] private SpellConfig _stasisFreeze;
        [SerializeField] private SpellConfig _cryoLeach;
        [SerializeField] private SpellConfig _avalancheCore;

        [Header("Earth")] 
        [SerializeField] private SpellConfig _spike;
        [SerializeField] private SpellConfig _earthShield;
        [SerializeField] private SpellConfig _deathZone;
        [SerializeField] private SpellConfig _naturePower;
        [SerializeField] private SpellConfig _plague;
        [SerializeField] private SpellConfig _flower;
        [SerializeField] private SpellConfig _natureWraith;
        
        private Dictionary<SpellName, SpellConfig> _spellValues;
        
        public static SpellData Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            _spellValues = new Dictionary<SpellName, SpellConfig>
            {
                { SpellName.Fireball, _fireball },
                { SpellName.Boom, _boom },
                { SpellName.Firewall, _fireWall },
                { SpellName.FireAura, _fireAura },
                { SpellName.FireMark, _fireMark },
                { SpellName.FireSpirit, _fireSpirit },
                { SpellName.FireLaser, _fireLaser },
                
                { SpellName.FrostWhirlwind, _frostWhirlwind },
                { SpellName.FlashFreeze, _flashFreeze },
                { SpellName.FrostAegis, _frostAegis },
                { SpellName.IcicleBarrage, _icicleBarrage },
                { SpellName.StasisFreeze, _stasisFreeze },
                { SpellName.CryoLeach, _cryoLeach },
                { SpellName.AvalancheCore, _avalancheCore },
                
                { SpellName.Spike, _spike },
                { SpellName.NaturePower, _naturePower },
                { SpellName.EarthShield, _earthShield },
                { SpellName.DeathZone, _deathZone },
                { SpellName.Plague, _plague },
                { SpellName.Flower, _flower },
                { SpellName.NatureWraith, _natureWraith }
            };
        }
        
       

        public SpellConfig GetSpellConfig(SpellName spellName)
        {
            if (_spellValues.TryGetValue(spellName, out SpellConfig config))
            {
                return config;
            }
    
            return null;
        }
    }
}