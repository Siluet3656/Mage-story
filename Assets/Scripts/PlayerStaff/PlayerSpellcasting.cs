using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Data.Enums;


namespace PlayerStaff
{
    [RequireComponent(typeof(SpellResources))]
    [RequireComponent(typeof(PlayerTargeting))]
    public class PlayerSpellcasting : MonoBehaviour
    {
        [SerializeField] private Image _castBar;

        private PlayerStats _stats;
        private SpellResources _resources;
        private PlayerTargeting _targeting;
        private bool _isCasting;
        private float _currentCastTime;
        private float _globalCooldown;
        private float _globalCooldownTimer;

        private void Awake()
        {
            _resources = GetComponent<SpellResources>();
            _targeting = GetComponent<PlayerTargeting>();
            _stats = new PlayerStats();
            _globalCooldown = _stats.GlobalCooldown;
        }

        public void CastSpell(SpellType spellType)
        {
            if (_isCasting || _globalCooldownTimer > 0 || _targeting.HasTarget == false) return;

            SpellConfig spellConfig = SpellData.GetSpellConfig(spellType);

            if (_resources.HasEnoughResources(spellConfig.ShardCost, spellConfig.ReminderCost) == false) return;

            StartCoroutine(CastSpellRoutine(spellConfig));
        }

        private IEnumerator CastSpellRoutine(SpellConfig config)
        {
            _isCasting = true;
            _castBar.color = config.CastBarColor;
            _currentCastTime = config.CastTime;

            // Cast bar progress
            float timer = 0;
            while (timer < _currentCastTime)
            {
                timer += Time.deltaTime;
                _castBar.fillAmount = timer / _currentCastTime;
                yield return null;
            }

            // Create spell and consume resources
            //var spell = Instantiate(spellInfo.PrefubOfSpell, transform.position, Quaternion.identity);
            //spell.SetTarget(_targeting.CurrentTarget);
            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);

            // Global cooldown
            _globalCooldownTimer = _globalCooldown;
            while (_globalCooldownTimer > 0)
            {
                _globalCooldownTimer -= Time.deltaTime;
                yield return null;
            }

            _isCasting = false;
            _castBar.fillAmount = 0;
        }
    }
}