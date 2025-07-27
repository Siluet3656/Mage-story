using System.Collections;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityResources;

namespace Spells
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AoeInstantSpell : Spell
    {
        private Sprite _sprite;
        private SpriteRenderer _spriteRenderer;
        private ITargetable _target;
        private Hp _targetsHp;
        private float _exitsTime;

        protected override SpellName SpellName { get; set; }

        private IEnumerator Existing()
        {
            yield return new WaitForSeconds(_exitsTime);
            
            base.ReturnToPool();
        }

        protected override void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            base.Awake();
        }
        
        private void Initialize(AoeInstantSpellConfig config)
        {
            SpellDamage = config.Damage;
            _sprite = config.CastSprite;
            _exitsTime = config.ExistTime;
        }
        
        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            if (config is AoeInstantSpellConfig instantSpellConfig)
            {
                Initialize(instantSpellConfig);
            }
            
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);
        }

        public override void DoSpell()
        {
            _spriteRenderer.sprite = _sprite;
            gameObject.SetActive(true);

            StartCoroutine(Existing());
        }
    }
}
