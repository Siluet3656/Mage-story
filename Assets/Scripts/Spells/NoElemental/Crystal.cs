using System.Linq;
using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using PlayerStaff;


namespace Spells.NoElemental
{
    public class Crystal : SummoningSpell
    {
        private TargetingCircle _targetingCircle;

        private readonly float _zapCooldown = 0.1f;
        private readonly float _spikeCriticalChance = 0.2f;
        private readonly float _spikeCriticalMultiply = 1.2f;
        
        private bool _isReadyToShoot;
        private float _time;
        
        protected override void Awake()
        {
            _isReadyToShoot = true;
            _time = 0f;
            
            base.Awake();
        }
        
        private void Update()
        {
            if (_isReadyToShoot)
            {
                TryToShoot();
            }
            else
            {
                Cooldown();
            }
        }
        
        private void Cooldown()
        {
            _time += Time.deltaTime;
                
            if (_time > _zapCooldown)
            {
                _time = 0f;
                _isReadyToShoot = true;
            }
        }

        private void TryToShoot()
        {
            if (_targetingCircle.NearbyTargets.Count == 0) return;
             
            ITargetable nearestEnemy = _targetingCircle.NearbyTargets
                .Where(enemy => enemy is Enemy)
                .OrderBy(enemy => Random.value)
                .FirstOrDefault();

            if (nearestEnemy == null) return;
            
            Spell spell = SpellFactory.Instance.PoolSpell(SpellName.Zap);

            if ((spell as LineSpell)?.TrySetTarget(nearestEnemy) == false) return;
            spell.transform.position = transform.position;
            spell.Initialize(SpellData.Instance.GetSpellConfig(SpellName.Zap), _spikeCriticalMultiply, _spikeCriticalChance);
            spell.DoSpell();
            
            _isReadyToShoot = false;
        }

        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);

            _targetingCircle = GetComponentInChildren<TargetingCircle>();
            _targetingCircle.gameObject.transform.localScale = new Vector3(Radius,Radius,1f);
        }
    }
}
