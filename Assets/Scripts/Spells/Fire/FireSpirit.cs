using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;
using PlayerStaff;

namespace Spells.Fire
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FireSpirit : SummoningSpell
    {
        private const float Speed = 80f;
        private const float MinimumDistance = 0.3f;
        private const float DefaultRadius = 1f;
        
        private Rigidbody2D _rigidbody;
        private Enemy _followed;
        private TargetingCircle _targetingCircle;
        private Vector3 _direction;

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _targetingCircle = GetComponentInChildren<TargetingCircle>();
            _targetingCircle.OnEnemyEnterCircle += TryToStartFollowEnemy;
            
            base.Awake();
        }
        
        private void FixedUpdate()
        {
            if (_followed == null) return;

            _direction = _followed.GameObject.transform.position - transform.position;
            Move();
            
            if (Vector2.Distance(transform.position, _followed.GameObject.transform.position) < MinimumDistance)
            {
                OnExistingEnd(_followed);
            }
        }
        
        private void OnEnable()
        {
            _targetingCircle.Collider.radius = DefaultRadius * Radius;
        }

        private void OnDisable()
        {
            _targetingCircle.Collider.radius = DefaultRadius;
        }

        private void TryToStartFollowEnemy(ITargetable target)
        {
            Enemy enemy = target as Enemy;
            
            if (enemy  == null) return;
            
            SetFollowed(enemy);
        }
        
        private void Move()
        {
            _rigidbody.velocity = _direction.normalized * (Speed * Time.fixedDeltaTime);
        }

        private void SetFollowed(Enemy followed)
        {
            _followed = followed;
        }

        protected override void OnExistingEnd(Enemy enemy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Hp>().TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
            
            SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Explosion);
            Spell fireballExplosion = SpellFactory.Instance.PoolSpell(SpellName.Explosion);

            if (fireballExplosion != null)
            {
                fireballExplosion.transform.position = transform.position;
                fireballExplosion.Initialize(config, CriticalMultiply, CriticalChance);
                fireballExplosion.gameObject.SetActive(true);
                fireballExplosion.DoSpell();
            }
            
            base.OnExistingEnd(enemy);
        }
    }
}
