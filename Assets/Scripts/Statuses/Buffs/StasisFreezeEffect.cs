using UnityEngine;
using Data;
using Data.Enums;
using Data.StatusConfigs;
using EnemyStaff;
using EntityResources;
using PlayerStaff;
using View;

namespace Statuses.Buffs
{
    public class StasisFreezeEffect : StatusEffect
    {
        private Hp _targetHp;
        private PlayerMovement _playerMovement;
        private PlayerSpellCasting _playerSpellCasting;
        private Enemy _enemy;

        private IceTomb _tomb;
        
        public StasisFreezeEffect(StatusEffectData data) : base(data)
        {
        }

        public override void Apply(GameObject target)
        {
            base.Apply(target);

            _targetHp = target.GetComponent<Hp>();
            _playerMovement = target.GetComponent<PlayerMovement>();
            _playerSpellCasting = target.GetComponent<PlayerSpellCasting>();
            _enemy = target.GetComponent<Enemy>();
            
            if (_targetHp != null && _playerMovement != null && _playerSpellCasting != null)
            {
                _targetHp.GetInvulnerable();
                _playerMovement.DisableMovement();
                _playerSpellCasting.DisableCasting();
            }
            else if (_targetHp != null && _enemy != null /*&& _playerSpellCasting != null*/)
            {
                _targetHp.GetInvulnerable();
                _enemy.SetMovementAvailability(false, MovementDisableSource.IceTomb);
            }
            
            _tomb = target.GetComponentInChildren<IceTomb>(true); 
            if (_tomb != null)  _tomb.gameObject.SetActive(true);
        }

        public override void Remove(GameObject target)
        {
            if (_targetHp != null && _playerMovement != null && _playerSpellCasting != null)
            {
                _targetHp.GetVulnerable();
                _playerMovement.EnableMovement();
                _playerSpellCasting.EnableCasting();
            }
            else if (_targetHp != null && _enemy != null /*&& _playerSpellCasting != null*/)
            {
                _targetHp.GetInvulnerable();
                _enemy.SetMovementAvailability(true, MovementDisableSource.IceTomb);
            }
            
            if (_tomb != null)  _tomb.gameObject.SetActive(false);
        
            base.Remove(target);
        }
    }
}
