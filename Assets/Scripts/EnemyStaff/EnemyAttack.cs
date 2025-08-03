using UnityEngine;
using System.Collections;
using Data;
using EntityStaff;

namespace EnemyStaff
{
    public class EnemyAttack : MonoBehaviour
    {
        private Hp _playersHp;
        private void Awake()
        {
            IsReadyToAttack = true;
            _playersHp = G.PlayersHp;
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
