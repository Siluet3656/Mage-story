using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;

namespace Data
{
    public class SpellData : MonoBehaviour
    {
        [SerializeField] private SpellConfig _fireball;

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
                { SpellName.Fireball, _fireball }
            };
        }
        
       

        public SpellConfig GetSpellConfig(SpellName spellName)
        {
            return _spellValues.TryGetValue(spellName, out SpellConfig config) ? config : GetSpellConfig(SpellName.NoSpell);
        }
    }
}
