using UnityEngine;
using Animations;
using Data.Enums;
using Data.SpellConfigs;
using EntityStaff;

namespace Spells
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AoeInstantSpell : Spell
    {
        private Animator _animator;
        private AnimationEventCatcher _animationEventCatcher;
        private ITargetable _target;
        private Hp _targetsHp;

        protected override SpellName SpellName { get; set; }
        protected Animator Animator => _animator;
        protected AnimationEventCatcher AnimationEventCatcher => _animationEventCatcher;

        protected override void Awake()
        {
            _animator = GetComponent<Animator>();
            _animationEventCatcher = GetComponent<AnimationEventCatcher>();
            
            base.Awake();
        }
        
        private void Initialize(AoeInstantSpellConfig config)
        {
            SpellDamage = config.Damage;
        }
        
        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            if (config is AoeInstantSpellConfig instantSpellConfig) Initialize(instantSpellConfig);
            
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);
        }

        public override void DoSpell()
        {
            gameObject.SetActive(true);
        }
        
        protected virtual void OnAnimationEnd()
        {
            base.ReturnToPool();
        }

    }
}
