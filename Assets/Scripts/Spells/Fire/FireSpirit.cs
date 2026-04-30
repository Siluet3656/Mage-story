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
        private const float MinimumDistance = 0.3f;
        private const float DefaultRadius = 1f;
        
        private Rigidbody2D _rigidbody;
        private EnemyTargeting _followed;
        private TargetingCircle _targetingCircle;
        private Vector3 _direction;

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
            
            SummonAnimator.Play("FireSpiritIdle");
            SummonAnimator.SetFloat("FireSpiritSpeed", 0f);
            //SummonAnimator.SetBool("isFireSpirit", true);
        }

        private void OnDisable()
        {
            _targetingCircle.Collider.radius = DefaultRadius;
        }

        private void TryToStartFollowEnemy(ITargetable target)
        {
            EnemyTargeting enemyTargeting = target as EnemyTargeting;
            
            if (enemyTargeting  == null) return;
            
            SetFollowed(enemyTargeting);
        }
        
        private void Move()
        {
            _rigidbody.velocity = _direction.normalized * (Speed * Time.fixedDeltaTime);
            SummonAnimator.SetFloat("FireSpiritSpeed", _rigidbody.velocity.magnitude);

            if (_rigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private void SetFollowed(EnemyTargeting followed)
        {
            _followed = followed;
        }

        protected override void OnExistingEnd(EnemyTargeting enemyTargeting)
        {
            if (enemyTargeting != null)
            {
                enemyTargeting.GetComponent<Hp>().TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
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
            
            base.OnExistingEnd(enemyTargeting);
        }
        
        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _targetingCircle = GetComponentInChildren<TargetingCircle>();
            _targetingCircle.OnEnemyEnterCircle += TryToStartFollowEnemy;
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("FireSpiritEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
            
            base.Awake();
        }

        protected override void ResetSummonState()
        {
            base.ResetSummonState();
            
            // Reset fire spirit specific state
            _followed = null;
            _direction = Vector3.zero;
            
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            
            // Unsubscribe and resubscribe to prevent duplicate subscriptions
            if (_targetingCircle != null)
            {
                _targetingCircle.OnEnemyEnterCircle -= TryToStartFollowEnemy;
                _targetingCircle.OnEnemyEnterCircle += TryToStartFollowEnemy;
            }
        }
    }
}
