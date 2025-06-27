using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;

namespace Data
{
    public class SpellData : MonoBehaviour
    {
        [SerializeField] private ISpellConfig _fireball;
        
        public static SpellData Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        
       

        public static SpellConfig GetSpellConfig(SpellName name)
        {
            return new SpellConfig();
        }
    }
}
