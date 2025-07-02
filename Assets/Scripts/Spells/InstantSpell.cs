using System.Collections;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityResources;

namespace Spells
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InstantSpell : Spell
    {
        private Sprite _sprite;
        private SpriteRenderer _spriteRenderer;
        private ITargetble _target;
        private Hp _targetsHp;
        private float _exitsTime;
        
        public override SpellName SpellName { get; protected set; }
        public override SpellType Type => SpellType.AoeInstantSpell;

        private IEnumerator Existing()
        {
            yield return new WaitForSeconds(_exitsTime);
        }

        protected override void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            base.Awake();
        }
        
        private void Initialize(InstantSpellConfig config)
        {
            SpellDamage = config.Damage;
            CriticalChance = config.CriticalChance;
            CriticalMultiply = config.CriticalMultiply;
            _sprite = config.CastSprite;
            _exitsTime = config.ExistTime;
        }
        
        public override void Initialize(SpellConfig config)
        {
            if (config is InstantSpellConfig instantSpellConfig)
            {
                Initialize(instantSpellConfig);
            }
            
            base.Initialize(config);
        }

        public override void DoSpell()
        {
            _spriteRenderer.sprite = _sprite;
            gameObject.SetActive(true);
            StartCoroutine(Existing());
            base.ReturnToPool();
        }
    }
}
