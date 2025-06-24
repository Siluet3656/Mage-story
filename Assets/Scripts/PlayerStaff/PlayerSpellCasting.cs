using System.Collections;
using UnityEngine;
using Data;
using Data.Enums;
using UI;


namespace PlayerStaff
{
    [RequireComponent(typeof(SpellResources))]
    [RequireComponent(typeof(PlayerTargeting))]
    [RequireComponent(typeof(PlayerUI))]
    public class PlayerSpellCasting : MonoBehaviour
    {
        private PlayerStats _stats;
        private SpellResources _resources;
        private PlayerTargeting _targeting;
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
            _ui = GetComponent<PlayerUI>();
            _stats = new PlayerStats();
            _globalCooldown = _stats.GlobalCooldown;
        }

        private IEnumerator CastSpellRoutine(SpellConfig config)
        {
            _isCasting = true;
            _ui.SetCastBarColor(config.CastBarColor);
            _currentCastTime = config.CastTime;

            _globalCooldownTimer = _globalCooldown;

            float castTimer = 0;
            while (castTimer < _currentCastTime || _globalCooldownTimer > 0)
            {
                if (castTimer < _currentCastTime)
                {
                    castTimer += Time.deltaTime;
                    _ui.ChangeCastBarFillAmount(castTimer / _currentCastTime);
                }

                if (_globalCooldownTimer > 0)
                {
                    _globalCooldownTimer -= Time.deltaTime;
                }

                yield return null;
            }

            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);

            _isCasting = false;
            _ui.ChangeCastBarFillAmount(0);
        }
        
        public void CastSpell(SpellType spellType)
        {
            if (_isCasting || _globalCooldownTimer > 0 || _targeting.HasTarget == false) return;

            SpellConfig spellConfig = SpellData.GetSpellConfig(spellType);

            if (_resources.HasEnoughResources(spellConfig.ShardCost, spellConfig.ReminderCost) == false) return;
            _spellCastRoutine = CastSpellRoutine(spellConfig);
            StartCoroutine(_spellCastRoutine);
        }

        public void StopCast()
        {
            if (_isCasting == false) return;
            
            StopCoroutine(_spellCastRoutine);
        }
    }
}