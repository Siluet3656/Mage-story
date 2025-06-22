using System.Collections;
using Data;
using Data.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlayerStaff
{
    public class PlayerSpellcasting : MonoBehaviour
    {
        [SerializeField] private SpellTypeData _spellData;
        [SerializeField] private Image _castBar;
        [SerializeField] private float _globalCooldown = 0.5f;

        private SpellResources _resources;
        private PlayerTargeting _targeting;
        private bool _isCasting;
        private float _currentCastTime;
        private float _globalCooldownTimer;

        private void Awake()
        {
            _resources = GetComponent<SpellResources>();
            _targeting = GetComponent<PlayerTargeting>();
        }

        public void CastSpell(SpellType spellType)
        {
            if (_isCasting || _globalCooldownTimer > 0 || !_targeting.HasTarget) return;

            var spellInfo = _spellData.GetDataByType(spellType);

            if (!_resources.HasEnoughResources(spellInfo.ShardsCost, spellInfo.ReminderCost)) return;

            StartCoroutine(CastSpellRoutine(spellInfo));
        }

        private IEnumerator CastSpellRoutine(SpellData spellInfo)
        {
            _isCasting = true;
            _castBar.color = GetSpellColor(spellInfo.Type);
            _currentCastTime = spellInfo.CastTime;

            // Cast bar progress
            float timer = 0;
            while (timer < _currentCastTime)
            {
                timer += Time.deltaTime;
                _castBar.fillAmount = timer / _currentCastTime;
                yield return null;
            }

            // Create spell and consume resources
            var spell = Instantiate(spellInfo.PrefubOfSpell, transform.position, Quaternion.identity);
            //spell.SetTarget(_targeting.CurrentTarget);
            _resources.ConsumeResources(spellInfo.ShardsCost, spellInfo.ReminderCost);

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

        private Color GetSpellColor(SpellType type)
        {
            // Return appropriate color based on spell type
            // (Implementation from original code)
            return new Color(0,0,0);
        }
    }
}