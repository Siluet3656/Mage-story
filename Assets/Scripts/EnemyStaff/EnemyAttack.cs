using System.Collections;
using System.Linq;
using UnityEngine;
using Data;
using Data.Enums;
using EntityStaff;
using PlayerStaff;
using View;

namespace EnemyStaff
{
    public class EnemyAttack : MonoBehaviour
    {
        private Hp _playersHp;
        private EnemyView _enemyView;
        private Player _player;
        private Hp _enemyHp;
        private Enemy _me;
        
        private float _currentSwipeProgress;
        
        private float _attackCooldownTime = 5f;
        private float _attackDamage = 50f;
        
        private float _castCooldownTime = 5f;
        private float _castValue = 50f;
        
        private void Awake()
        {
            IsReadyToAttack = true;
            _playersHp = G.PlayersHp;
            _player = G.Player;
            _enemyView = GetComponent<EnemyView>();
            _enemyHp = GetComponent<Hp>();
            _me = GetComponent<Enemy>();
        }

        private void OnDisable()
        {
            _currentSwipeProgress = 0f;
            _enemyView.UpdateAttackSwingBar(_currentSwipeProgress);
            
            StopAllCoroutines();
        }

        private IEnumerator Cooldown(float cooldown)
        {
            IsReadyToAttack = false;
            float elapsedTime = 0f;
            _currentSwipeProgress = 0f;

            while (elapsedTime < cooldown)
            {
                elapsedTime += Time.deltaTime;
                _currentSwipeProgress = Mathf.Clamp01(elapsedTime / cooldown);
                _enemyView.UpdateAttackSwingBar(_currentSwipeProgress);
                yield return null;
            }

            IsReadyToAttack = true;
            _currentSwipeProgress = 1f;
            _enemyView.UpdateAttackSwingBar(_currentSwipeProgress);
        }
        
        public bool IsReadyToAttack { get; private set; }
        
        public void SetAttackStats(float damage, float rate)
        {
            _attackDamage = damage;
            _attackCooldownTime = rate;
        }
        
        public void SetCastStats(float value, float rate)
        {
            _castValue = value;
            _castCooldownTime = rate;
        }

        public void StartAttackCooldown()
        {
            StartCoroutine(Cooldown(_attackCooldownTime));
        }

        public void StartCastCooldown()
        {
            StartCoroutine(Cooldown(_castCooldownTime));
        }
        
        public void PerformAttack()
        {
            _playersHp.TryToTakeDamage(_attackDamage, false);
            
        }
        
        public void ShootPlayer()
        {
            Projectile projectile = EnemyProjectilesFactory.Instance.PoolProjectile(EnemyProjectileName.Arrow);
            projectile.transform.position = transform.position;
            projectile.SetDamage(_attackDamage);
        }

        public void HealMyself()
        {
            _enemyHp.Heal(_castValue);
        }
        
        public void HealNearbyEnemy()
        {
            ITargetable targetToHeal = _me.HealCastingCircle.NearbyTargets.OrderBy(x => GetComponent<Hp>().CurrentHealth).FirstOrDefault();

            if (targetToHeal != null)
            {
                Hp targetHp = targetToHeal.GameObject.GetComponent<Hp>();
                targetHp.Heal(_castValue);
            }
        }
    }
}