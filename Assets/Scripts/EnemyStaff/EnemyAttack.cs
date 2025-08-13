using System.Collections;
using UnityEngine;
using Data;
using EntityStaff;
using View;

namespace EnemyStaff
{
    public class EnemyAttack : MonoBehaviour
    {
        private Hp _playersHp;
        private EnemyView _enemyView;
        
        private float _currentSwipeProgress;
        
        private void Awake()
        {
            IsReadyToAttack = true;
            _playersHp = G.PlayersHp;
            _enemyView = GetComponent<EnemyView>();
        }

        private IEnumerator AttackCooldown(float cooldown)
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

        public void PerformAttack(float damage, float cooldown)
        {
            _playersHp.TryToTakeDamage(damage, false);
            StartCoroutine(AttackCooldown(cooldown));
        }
    }
}