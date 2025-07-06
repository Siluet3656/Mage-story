using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityResources;

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
        private Vector3 _direction;

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            
            base.Awake();
        }
        
        private void FixedUpdate()
        {
            if (_followed == null) return;

            _direction = _followed.GameObject.transform.position - transform.position;
            Move();
            
            if (Vector2.Distance(transform.position, _followed.GameObject.transform.position) < MinimumDistance)
            {
                OnReachTarget(_followed);
            }
        }
        
        private void OnEnable()
        {
            AttackCollider.radius = DefaultRadius * Radius;
        }

        private void OnDisable()
        {
            AttackCollider.radius = DefaultRadius;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            
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

        protected override void OnReachTarget(Enemy enemy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Hp>().TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
            
            SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Explosion);
            Spell fireballExplosion = SpellFactory.Instance.CreateSpell(SpellName.Explosion);

            if (fireballExplosion != null)
            {
                fireballExplosion.transform.position = transform.position;
                fireballExplosion.Initialize(config, CriticalMultiply, CriticalChance);
                fireballExplosion.gameObject.SetActive(true);
                fireballExplosion.DoSpell();
            }
            
            base.OnReachTarget(enemy);
        }
    }
}
