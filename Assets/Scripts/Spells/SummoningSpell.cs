using System.Collections;
using Data;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityResources;

namespace Spells
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class SummoningSpell : Spell
    {
        private Sprite _sprite;
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _collider;
        private Hp _targetsHp;
        private float _existTime;
        private float _radius;
        
        protected override SpellName SpellName { get; set; }
        protected CircleCollider2D AttackCollider => _collider;
        protected float Radius => _radius;
        
        protected override void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CircleCollider2D>();
            
            base.Awake();
        }

        private void Initialize(SummoningSpellConfig config)
        {
            SpellDamage = config.Damage;
            CriticalChance = config.CriticalChance;
            CriticalMultiply = config.CriticalMultiply;
            _sprite = config.SummonSprite;
            _existTime = config.ExistingTime;
            _radius = config.AttackRadius;
        }
        
        private IEnumerator Existing()
        {
            yield return new WaitForSeconds(_existTime);

            OnReachTarget(null);
        }
        
        protected virtual void OnReachTarget(Enemy enemy)
        {
            StopAllCoroutines();
            base.ReturnToPool();
        }
        
        public override void Initialize(SpellConfig config)
        {
            if (config is SummoningSpellConfig projectileSpellConfig)
            {
                Initialize(projectileSpellConfig);
            }
            
            base.Initialize(config);
        }

        public override void DoSpell()
        {
            _spriteRenderer.sprite = _sprite;
            gameObject.SetActive(true);
            
            StartCoroutine(Existing());
        }
    }
}
