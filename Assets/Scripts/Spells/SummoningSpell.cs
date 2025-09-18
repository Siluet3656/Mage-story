using System.Collections;
using UnityEngine;
using AllyStaff;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;
using View;

namespace Spells
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Ally))]
    public abstract class SummoningSpell : Spell
    {
        private Ally _ally;
        private Sprite _sprite;
        private ViewCatcher _viewCatcher;
        private Hp _targetsHp;
        private float _existTime;
        private float _radius;
        private float _speed;
        
        protected Animator SummonAnimator;
        protected override SpellName SpellName { get; set; }
        protected float Radius => _radius;
        protected float ExistTime => _existTime;
        protected float Speed => _speed;
        
        protected override void Awake()
        {
            _viewCatcher = GetComponent<ViewCatcher>();
            _ally = GetComponent<Ally>();
            SummonAnimator = _viewCatcher.Animator;
            
            base.Awake();
        }

        private void Initialize(SummoningSpellConfig config)
        { 
            SpellDamage = config.Damage;
            _sprite = config.SummonSprite;
            _existTime = config.ExistingTime;
            _radius = config.AttackRadius;
            _speed = config.SummonSpeed;

            _ally.Initialize(config.IsTargetable, config.IsNeedHp, config.SummonHp);
        }
        
        private IEnumerator Existing()
        {
            yield return new WaitForSeconds(_existTime);

            OnExistingEnd(null);
        }
        
        protected virtual void OnExistingEnd(EnemyTargeting enemyTargeting)
        {
            StopAllCoroutines();
            base.ReturnToPool();
        }
        
        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            if (config is SummoningSpellConfig projectileSpellConfig)
            {
                Initialize(projectileSpellConfig);
            }
            
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);
        }

        public override void DoSpell()
        {
            _viewCatcher.SpriteRenderer.sprite = _sprite;
            gameObject.SetActive(true);
            
            StartCoroutine(Existing());
        }
    }
}
