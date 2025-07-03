using System.Collections;
using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using Shard;
using Spells;
using View;

namespace PlayerStaff
{
    [RequireComponent(typeof(SpellResources))]
    [RequireComponent(typeof(PlayerTargeting))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerUI))]
    public class PlayerSpellCasting : MonoBehaviour
    {
        private PlayerStats _stats;
        private SpellResources _resources;
        private PlayerTargeting _targeting;
        private PlayerMovement _movement;
        private PlayerUI _ui;
        private PlayersShard _shard;
            
        private IEnumerator _spellCastRoutine;
        private IEnumerator _globalCooldownRoutine;
        private bool _isCasting;
        private float _currentCastTime;
        private float _globalCooldown;
        private float _globalCooldownTimer;

        private SpellName _spellName;
        private Spell _spell;

        private Vector3 _mousePosition;
        private GameObject _ghost;
        private bool _isPlacing = false;
        
        public bool Casting => _isCasting;
        public bool IsPlacing => _isPlacing;
        public bool RequireTarget => SpellData.Instance.GetSpellConfig(_spellName).RequiresTarget;

        private void Awake()
        {
            _resources = GetComponent<SpellResources>();
            _targeting = GetComponent<PlayerTargeting>();
            _movement = GetComponent<PlayerMovement>();
            _ui = GetComponent<PlayerUI>();
            _stats = new PlayerStats();
            _globalCooldown = _stats.GlobalCooldown;
            
            _shard = FindObjectOfType<PlayersShard>();

            if (_shard == null)
            {
                Debug.LogError("NO SHARD!!!");
            }
        }

        private void Update()
        {
            if (_isPlacing) _ghost.transform.position = _mousePosition;
        }

        private IEnumerator CastSpellWithCastTime(SpellConfig config, ICast castConfig)
        {
            _ui.SetCastBarColor(castConfig.GetCastBarColor());
            _currentCastTime = castConfig.GetCastTime();
 
            float castTimer = 0;
            while (castTimer < _currentCastTime)
            {
                if (castTimer <= _currentCastTime)
                {
                    castTimer += Time.deltaTime;
                    _ui.UpdateCastBar(castTimer / _currentCastTime);
                }
                
                yield return null;
            }
            switch (config.GetSPellType())
            {
                case SpellType.Projectile:
                    DoProjectileSpell();
                    break;
                case SpellType.PlacedSpell:
                    StartDeployingSpell(config);
                    break;
            }
            
            _ui.UpdateCastBar(0f);
            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);
            _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed());
            _isCasting = false;
        }

        private void DoProjectileSpell()
        {
            if (_targeting.HasTarget == false) return;

            if ((_spell as ProjectileSpell)?.TrySetTarget(_targeting.GetTarget) == false) return;
            
            _spell.transform.position = _shard.transform.position;
            _spell.DoSpell();
        }

        private void StartDeployingSpell(SpellConfig config)
        {
            if (config is DeployableSpellConfig deployableSpellConfig)
            {
                Color ghostColor = deployableSpellConfig.GetGhostColor();
                Sprite ghostSprite = deployableSpellConfig.DeployedSprite;

                _ghost = new GameObject("ghost");
                
                SpriteRenderer ghostSpriteRenderer = _ghost.AddComponent<SpriteRenderer>();
                ghostSpriteRenderer.sprite = ghostSprite;
                ghostSpriteRenderer.color = ghostColor;

                _isPlacing = true;
            }
        }

        private void DoAoeInstantSpell()
        {
            _spell.transform.position = _shard.transform.position;
            _spell.DoSpell();
        }
        
        private void CastSpellInstantly (SpellConfig config)
        {
            switch (config.GetSPellType())
            {
                case SpellType.AoeInstantSpell:
                    DoAoeInstantSpell();
                    break;
            }

            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);
            _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed());
            _isCasting = false;
        }

        private IEnumerator GlobalCooldown()
        {
            _globalCooldownTimer = _globalCooldown;
            
            while (_globalCooldownTimer > 0)
            {
                if (_globalCooldownTimer >= 0)
                {
                    _globalCooldownTimer -= Time.deltaTime;
                    _ui.UpdateGcdBars(_globalCooldownTimer / _globalCooldown);
                }
                
                yield return null;
            }
            _ui.UpdateGcdBars(0f);
        }

        private bool IsCastAvailable(SpellName spellName)
        {
            if (spellName == SpellName.NoSpell) return false;
            if (_isCasting || _globalCooldownTimer > 0) return false;
            
            return true;
        }

        private bool IsSpellRequirementsMet(SpellConfig spellConfig)
        {
            if (_resources.HasEnoughResources(spellConfig.ShardCost, spellConfig.ReminderCost) == false) return false;
            if (spellConfig.RequiresTarget && _targeting.HasTarget == false) return false;
            return true;
        }
        
        public void StartCast(SpellName spellName)
        {
            if (IsCastAvailable(spellName) == false) return;
            
            SpellConfig spellConfig = SpellData.Instance.GetSpellConfig(spellName);
            
            if (IsSpellRequirementsMet(spellConfig) == false) return;
            
            _spell = SpellFactory.Instance.CreateSpell(spellName);

            if (_spell == null) return;

            _spellName = spellName;
            _spell.Initialize(spellConfig);    
            _isCasting = true;
            _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed() - 1);
            _globalCooldownRoutine = GlobalCooldown();
            StartCoroutine(_globalCooldownRoutine);

            if (spellConfig is ICast castSpellConfig)
            { 
                _spellCastRoutine = CastSpellWithCastTime(spellConfig, castSpellConfig);
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
            
            SpellFactory.Instance.ReturnSpell(_spellName ,_spell);
            
            StopCoroutine(_spellCastRoutine);
            StopCoroutine(_globalCooldownRoutine);
            _spellCastRoutine = null;
            _isCasting = false;
            _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed());
            _globalCooldownTimer = 0f;
            _ui.UpdateGcdBars(0f);
            _ui.UpdateCastBar(0f);
        }

        public void SetMousePosition(Vector3 position) => _mousePosition = position;

        public void Place()
        {
            _spell.transform.position = _ghost.transform.position;
            _spell.DoSpell();
            Destroy(_ghost);
            
            _isPlacing = false;
        }

        public void CancelPlacing()
        {
            
            Destroy(_ghost);
            _isPlacing = false;
            
            SpellFactory.Instance.ReturnSpell(_spellName ,_spell);
        }
    }
}