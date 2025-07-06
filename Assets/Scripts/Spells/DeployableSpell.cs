using System.Collections;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;

namespace Spells
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class DeployableSpell : Spell
    {
        protected override SpellName SpellName { get; set; }
        private SpriteRenderer _spriteRenderer;
        private Sprite _sprite;
        private float _duration;

        protected override void Awake()
        {
            base.Awake();

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Initialize(DeployableSpellConfig config)
        {
            SpellDamage = config.Damage;
            _sprite = config.DeployedSprite;
            _duration = config.Duration;
        }

        private IEnumerator Existing()
        {
            yield return new WaitForSeconds(_duration);
            
            base.ReturnToPool();
        }
        
        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            if (config is DeployableSpellConfig deployableSpellConfig)
            {
                Initialize(deployableSpellConfig);
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
