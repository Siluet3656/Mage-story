using Data.Enums;
using UnityEngine;
using Data.SpellConfigs;
using EntityResources;

namespace Spells
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ProjectileSpell : Spell
    {
        private const float Speed = 800f;
        private const float MinimumDistance = 0.3f;

        private Sprite _sprite;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Enemy _target;
        private Hp _targetsHp;
        private Vector3 _direction;
        private float _angle;

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (!_target) return;

            _direction = _target.transform.position - transform.position;
            Move();
            
            if (Vector2.Distance(transform.position, _target.transform.position) < MinimumDistance)
            {
                OnReachTarget(_target);
            }
        }

        private void Move()
        {
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _rigidbody.velocity = _direction.normalized * (Speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        }
        
        protected virtual void OnReachTarget(Enemy target)
        {
            base.ApplyDamage(_targetsHp);
            base.ReturnToPool();
        }

        public override SpellName SpellName { get; protected set; }
        public override SpellType Type => SpellType.Projectile;

        private ProjectileSpellConfig _config;


        public override void Initialize(SpellConfig config)
        {
            //base.Initialize(config);
            //_config = config as ProjectileSpellConfig;
            if (config is ProjectileSpellConfig projectileSpellConfig)
            {
                Initialize(projectileSpellConfig);
            }
        }

        public override void Initialize(ProjectileSpellConfig config)
        {
            SpellDamage = config.Damage;
            CriticalChance = config.CriticalChance;
            CriticalMultiply = config.CriticalMultiply;
            _sprite = config.ProjectileSprite;
        }

        public override void DoSpell()
        {
            if (_target == null) return;

            _targetsHp = _target.GetComponent<Hp>();
            _spriteRenderer.sprite = _sprite;
            gameObject.SetActive(true);
        }
        
        public bool TrySetTarget(Enemy target)
        {
            if (target == null) return false;
            
            _target = target;

            return true;
        }
    }
}