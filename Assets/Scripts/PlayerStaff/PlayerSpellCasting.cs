using System.Collections;
using UnityEngine;
using AllyStaff;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EntityStaff;
using Shard;
using Spells;
using Spells.Frost;
using Statuses;
using View;

namespace PlayerStaff
{
    [RequireComponent(typeof(SpellResources))]
    [RequireComponent(typeof(PlayerTargeting))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerUI))]
    [RequireComponent(typeof(StatusApplier))]
    [RequireComponent(typeof(Hp))]
    public class PlayerSpellCasting : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats;
        
        private SpellResources _resources;
        private PlayerTargeting _targeting;
        private PlayerMovement _movement;
        private PlayerUI _ui;
        private PlayersShard _shard;
        private StatusApplier _statusApplier;
        private Hp _playerHp;
            
        private IEnumerator _spellCastRoutine;
        private IEnumerator _globalCooldownRoutine;
        private bool _isCasting;
        private bool _isAbleToCast;
        private float _currentCastTime;
        private float _globalCooldown;
        private float _globalCooldownTimer;
        
        private float _adjustedFireCriticalMultiply;
        private float _adjustedFireCriticalChance;
        private float _adjustedFrostCriticalMultiply;
        private float _adjustedFrostCriticalChance;
        private float _adjustedEarthCriticalMultiply;
        private float _adjustedEarthCriticalChance;
        private float _adjustedNoElementalCriticalMultiply;
        private float _adjustedNoElementalCriticalChance;

        private SpellName _spellName;
        private Spell _spell;

        private Vector3 _mousePosition;
        private GameObject _ghost;
        private bool _isPlacing;
        private ITargetable _targetCastingTo;
        
        public bool Casting => _isCasting;
        public bool IsPlacing => _isPlacing;
        public bool RequireTarget => SpellData.Instance.GetSpellConfig(_spellName).RequiresTarget;
        public float AdjustedFireCriticalMultiply => _adjustedFireCriticalMultiply;
        public float AdjustedFireCriticalChance => _adjustedFireCriticalChance;
        public float CurrentGlobalCooldown => _globalCooldown;

        private void Awake()
        {
            _ui = GetComponent<PlayerUI>();
            _resources = GetComponent<SpellResources>();
            _targeting = GetComponent<PlayerTargeting>();
            _movement = GetComponent<PlayerMovement>();
            _statusApplier = GetComponent<StatusApplier>();
            _playerHp = GetComponent<Hp>();
            
            _globalCooldown = _stats.GlobalCooldown;
            
            _adjustedFireCriticalMultiply = _stats.FireCriticalMultiplier;
            _adjustedFireCriticalChance = _stats.FireCriticalChance;
            
            _adjustedFrostCriticalMultiply = _stats.FrostCriticalMultiplier;
            _adjustedFrostCriticalChance = _stats.FrostCriticalChance;
            
            _adjustedEarthCriticalMultiply = _stats.EarthCriticalMultiplier;
            _adjustedEarthCriticalChance = _stats.EarthCriticalChance;
            
            _adjustedNoElementalCriticalMultiply = _stats.NoElementCriticalMultiplier;
            _adjustedNoElementalCriticalChance = _stats.NoElementCriticalChance;
            
            _shard = FindObjectOfType<PlayersShard>();
            if (_shard == null)
            {
                Debug.LogError("NO SHARD!!!");
            }
            
            _isPlacing = false;
            _isAbleToCast = true;
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

            if (TryToChooseSpellAndDoIt(config))
            {
                _ui.UpdateCastBar(0f);
                _resources.ConsumeResources(config.ShardCost, config.ReminderCost);
                _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed());
                _isCasting = false;
                
            }
            else
            {
                StopCast();
            }
        }

        private bool TryToChooseSpellAndDoIt(SpellConfig config)
        {
            switch (config.GetSPellType())
            {
                case SpellType.Projectile:
                    DoProjectileSpell();
                    return true;
                case SpellType.PlacedSpell:
                    StartDeployingSpell(config);
                    return true;
                case SpellType.SummonSpell:
                    Summon();
                    return true;
                case SpellType.LineSpell:
                    LineSpellCast();
                    return true;
            }

            return false;
        }

        private void LineSpellCast()
        {
            if (_targeting.HasTarget == false) {StopCast(); return;}
            
            if ((_spell as LineSpell)?.TrySetTarget(_targetCastingTo) == false) {StopCast(); return;}
            
            _spell.transform.position = _shard.transform.position;
            _spell.DoSpell();
        }

        private void DoProjectileSpell()
        {
            if (_targeting.HasTarget == false) {StopCast(); return;}

            if ((_spell as ProjectileSpell)?.TrySetTarget(_targetCastingTo) == false) {StopCast(); return;}
            
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
                
                _ghost.transform.localScale = new Vector3(deployableSpellConfig.ScaleFactor,deployableSpellConfig.ScaleFactor,1);
                
                SpriteRenderer ghostSpriteRenderer = _ghost.AddComponent<SpriteRenderer>();
                ghostSpriteRenderer.sprite = ghostSprite;
                ghostSpriteRenderer.color = ghostColor;

                _isPlacing = true;
            }
        }

        private void Summon()
        {
            _spell.transform.position = _shard.transform.position;

            if (_spell is IcicleBarrage barrage) barrage.TrySetTarget(_targetCastingTo);
            
            _spell.DoSpell();
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
                case SpellType.SelfInstantSpell:
                    DoSelfBuff(config as SelfBuffSpellConfig);
                    break;
                case SpellType.ShieldSpell:
                    DoShield(config as ShieldSpellConfig);
                    break;
                case SpellType.HealSpell:
                    HealTarget(config as HealingSpellConfig);
                    break;
                case SpellType.InstantDebuffSpell:
                    DebuffTarget(config as InstantDebuffSpellConfig);
                    break;
            }

            _resources.ConsumeResources(config.ShardCost, config.ReminderCost);
            _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed());
            _isCasting = false;
        }

        private void DebuffTarget(InstantDebuffSpellConfig config)
        {
            _statusApplier.ApplyStatusToTarget(config.Debuff, _targetCastingTo.GameObject);
        }

        private void HealTarget(HealingSpellConfig config)
        {
            Hp allyHp = _targetCastingTo.GameObject.GetComponent<Hp>();
            Ally ally = _targetCastingTo.GameObject.GetComponent<Ally>();
            if (ally != null && allyHp != null)
            {
                allyHp.Heal(config.HealAmount);
            }
            else
            {
                _playerHp.Heal(config.HealAmount);
            }
        }

        private void DoShield(ShieldSpellConfig config)
        { 
            switch (config.ShieldType)
            {
                case ShieldType.FrostShield:
                    _playerHp.GetAdditionalHp(SpellName.FrostAegis);
                    break;
                case ShieldType.EarthShield:
                    _playerHp.GetShieldStacks(SpellName.EarthShield);
                    break;
                case ShieldType.Heal:
                    _playerHp.Heal(config.Amount);
                    break;
            }
        }

        private void DoSelfBuff(SelfBuffSpellConfig buffSpellConfig)
        {
            _statusApplier.ApplyStatusToTarget(buffSpellConfig.Buff, gameObject);
        }

        private IEnumerator GlobalCooldown()
        {
            _globalCooldownTimer = _globalCooldown;
            
            while (_globalCooldownTimer > 0f)
            {
                if (_globalCooldownTimer >= 0f)
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
            if (_isAbleToCast == false) return false;
            if (spellName == SpellName.NoSpell) return false;
            if (_isCasting || _globalCooldownTimer > 0f) return false;
            
            return true;
        }

        private bool IsSpellRequirementsMet(SpellConfig spellConfig)
        {
            if (_resources.HasEnoughResources(spellConfig.ShardCost, spellConfig.ReminderCost) == false) return false;
            if (spellConfig.RequiresTarget && _targeting.HasTarget == false) return false;
            return true;
        }
        
        private void InitializeSpell(SpellConfig spellConfig)
        {
            if (_spell == null) return; 
            
            switch (spellConfig.SpellElementType)
            {
                case SpellElementType.Fire:
                    _spell.Initialize(spellConfig, _adjustedFireCriticalMultiply, _adjustedFireCriticalChance);    
                    break;
                case SpellElementType.Frost:
                    _spell.Initialize(spellConfig, _adjustedFrostCriticalMultiply, _adjustedFrostCriticalChance);  
                    break;
                case SpellElementType.Earth:
                    _spell.Initialize(spellConfig, _adjustedEarthCriticalMultiply, _adjustedEarthCriticalChance);  
                    break;
                case SpellElementType.NoElemental:
                    _spell.Initialize(spellConfig, _adjustedNoElementalCriticalMultiply, _adjustedNoElementalCriticalChance);  
                    break;
            }
        }
        
        public void StartCast(SpellName spellName)
        {
            if (IsCastAvailable(spellName) == false) return;
            
            SpellConfig spellConfig = SpellData.Instance.GetSpellConfig(spellName);
            
            if (IsSpellRequirementsMet(spellConfig) == false) return;

            if (spellConfig is INeedPrefab)
            {
                _spell = SpellFactory.Instance.PoolSpell(spellName);
                
                if (_spell == null) return;
                
                _spell.SetSpellResources(_resources); 
                InitializeSpell(spellConfig);
            }
            
            _spellName = spellName;
            _isCasting = true;
            _movement.SetSpeed(_movement.GetAdjustedPlayerSpeed() - 1);
            _globalCooldownRoutine = GlobalCooldown();
            StartCoroutine(_globalCooldownRoutine);

            _targetCastingTo = _targeting.GetCurrentTarget;
            
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
            
            SpellFactory.Instance.ReturnSpell(_spellName, _spell);
            
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

        public void AdjustCriticalDamage(SpellElementType spellElementType,float multiply, float chance)
        {
            if (multiply < 1f) return;
            if (chance < 0f || chance > 1f) return;

            switch (spellElementType)
            {
                case SpellElementType.Fire:
                    _adjustedFireCriticalMultiply = multiply;
                    _adjustedFireCriticalChance = chance;
                    break;
                case SpellElementType.Frost:
                    _adjustedFrostCriticalMultiply = multiply;
                    _adjustedFrostCriticalChance = chance;
                    break;
                case SpellElementType.Earth:
                    _adjustedEarthCriticalMultiply = multiply;
                    _adjustedEarthCriticalChance = chance;
                    break;
                case SpellElementType.NoElemental:
                    _adjustedNoElementalCriticalMultiply = multiply;
                    _adjustedNoElementalCriticalChance = chance;
                    break;
                    
            }
        }

        public void DisableCasting()
        {
            _isAbleToCast = false;
        }
        
        public void EnableCasting()
        {
            _isAbleToCast = true;
        }

        public void SetGlobalCooldown(float cooldown)
        {
            _globalCooldown = cooldown;
        }
    }
}