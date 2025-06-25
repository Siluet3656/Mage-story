using System.Collections;
using UnityEngine;
using Data;
using Data.Enums;
using UI;


namespace PlayerStaff
{
    [RequireComponent(typeof(SpellResources))]
    [RequireComponent(typeof(PlayerTargeting))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerUI))]
    public class PlayerSpellCasting : MonoBehaviour
    {
        private PlayerStats _stats;
        private SpellResources _resources;
        private PlayerTargeting _targeting;
        private PlayerMovement _movement;
        private PlayerUI _ui;
            
        private IEnumerator _spellCastRoutine;
        private bool _isCasting;
        private float _currentCastTime;
        private float _globalCooldown;
        private float _globalCooldownTimer;

        private void Awake()
        {
            _resources = GetComponent<SpellResources>();
            _targeting = GetComponent<PlayerTargeting>();
            _movement = GetComponent<PlayerMovement>();
            _ui = GetComponent<PlayerUI>();
            _stats = new PlayerStats();
            _globalCooldown = _stats.GlobalCooldown;
        }
        
        private IEnumerator CastSpellWithCastTime(SpellConfig config)
        {
            _ui.SetCastBarColor(config.CastBarColor);
            _currentCastTime = config.CastTime;
            _globalCooldownTimer = _globalCooldown;

            float castTimer = 0;
            while (castTimer < _currentCastTime || _globalCooldownTimer > 0)
            {
                if (castTimer <= _currentCastTime)
                {
                    castTimer += Time.deltaTime;
                    _ui.UpdateCastBar(castTimer / _currentCastTime);
                }

                if (_globalCooldownTimer >= 0)
                {
                    _globalCooldownTimer -= Time.deltaTime;
                    _ui.UpdateGcdBars(_globalCooldownTimer);
                }

                yield return null;
            }
            _ui.UpdateCastBar(0f);
            
            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);
            _movement.UpdateMovementSpeed(_movement.GetAdjustedPlayerSpeed());
            _isCasting = false;
        }
        
        private void CastSpellInstantly(SpellConfig config)
        {
            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);
            _movement.UpdateMovementSpeed(_movement.GetAdjustedPlayerSpeed());
            _isCasting = false;
        }
        
        public void CastSpell(SpellType spellType)
        {
            if (_isCasting || _globalCooldownTimer > 0) return;

            SpellConfig spellConfig = SpellData.GetSpellConfig(spellType);

            if (_resources.HasEnoughResources(spellConfig.ShardCost, spellConfig.ReminderCost) == false) return;
            if (spellConfig.RequireTarget && _targeting.HasTarget == false) return;
                
            _isCasting = true;
            _movement.UpdateMovementSpeed(_movement.GetAdjustedPlayerSpeed() - 1);
            if (spellConfig.CastTime > 0)
            {
                _spellCastRoutine = CastSpellWithCastTime(spellConfig);
                StartCoroutine(_spellCastRoutine);
            }
            else
            {
                CastSpellInstantly(spellConfig);
            }
            
        }

        public void StopCast()
        {
            if (_isCasting == false) return;
            
            StopCoroutine(_spellCastRoutine);
            _spellCastRoutine = null;
            _isCasting = false;
            _globalCooldownTimer = 0f;
            _ui.UpdateGcdBars(0f);
            _ui.UpdateCastBar(0f);
        }
    }
}