using System;
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
        
        private void Awake()
        {
            IsReadyToAttack = true;
            _playersHp = G.PlayersHp;
            _enemyView = GetComponent<EnemyView>();
        }

        private void Update()
        {
            //_enemyView.UpdateAttackSwingBar();
        }

        private IEnumerator AttackCooldown(float cooldown)
        {
            IsReadyToAttack = false;
            yield return new WaitForSeconds(cooldown);
            IsReadyToAttack = true;
        }
        
        public bool IsReadyToAttack { get; private set; }

        public void PerformAttack(float damage, float cooldown)
        {
            _playersHp.TryToTakeDamage(damage, false);
            StartCoroutine(AttackCooldown(cooldown));
        }
    }
}