using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EntityStaff;

namespace Spells
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ProjectileSpell : Spell
    {
        private const float Speed = 800f;
        private const float MinimumDistance = 0.3f;
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private ITargetable _target;
        private Hp _targetsHp;
        private Vector3 _direction;
        private float _angle;

        protected override SpellName SpellName { get; set; }
        protected Animator Animator => _animator;

        private ProjectileSpellConfig _config;

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            base.Awake();
        }

        private void Initialize(ProjectileSpellConfig config)
        {
            SpellDamage = config.Damage;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            _direction = _target.GameObject.transform.position - transform.position;
            Move();
            
            if (Vector2.Distance(transform.position, _target.GameObject.transform.position) < MinimumDistance)
            {
                OnReachTarget(_target);
            }
        }

        private void Move()
        {
            _rigidbody.velocity = _direction.normalized * (Speed * Time.fixedDeltaTime);

            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        }
        
        protected virtual void OnReachTarget(ITargetable target)
        {
            base.ApplyDamage(_targetsHp);
            base.ReturnToPool();
        }
        
        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            if (config is ProjectileSpellConfig projectileSpellConfig)
            {
                Initialize(projectileSpellConfig);
            }
            
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);
        }
        
        public void OnTargetDeath()
        {
            _rigidbody.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            
            ReturnToPool();
        }

        public override void DoSpell()
        {
            if (_target == null) return;

            _targetsHp = _target.GameObject.GetComponent<Hp>();
            gameObject.SetActive(true);
        }
        
        public bool TrySetTarget(ITargetable target)
        {
            if (target == null) return false;
            
            _target = target;

            return true;
        }
    }
}