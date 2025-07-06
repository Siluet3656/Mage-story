using System;
using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Data;
using Shard;
using Spells.Earth;
using Spells.Fire;
using Spells.Frost;


namespace Spells
{
    public class SpellFactory : MonoBehaviour
    {
        [Header("Base")] 
        [SerializeField, Min(1)] private int _numOfEachSpell;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _projectileSpell;
        [SerializeField] private GameObject _aoeInstantSpell;
        [SerializeField] private GameObject _deployableSpell;
        [SerializeField] private GameObject _summonSpell;
        [SerializeField] private GameObject _lineSpell;
        
        private readonly Dictionary<SpellName, Queue<Spell>> _playerSpellPools = new Dictionary<SpellName, Queue<Spell>>();

        private PlayersShard _shard;
       
        public static SpellFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            InitializePlayerPools();
        }
        
        
        private void InitializePlayerPools()
        {
            _playerSpellPools.Add(SpellName.NoSpell, new Queue<Spell>());
            
            #region SpellinitFire
            
            InstantiateSpells(SpellName.Fireball, typeof(Fireball), _projectileSpell);
            InstantiateSpells(SpellName.Explosion, typeof(Explosion), _aoeInstantSpell);
            InstantiateSpells(SpellName.Firewall, typeof(FireWall), _deployableSpell);
            InstantiateSpells(SpellName.FireSpirit, typeof(FireSpirit), _summonSpell);
            InstantiateSpells(SpellName.FireLaser, typeof(FireLaser), _lineSpell);
            InstantiateSpells(SpellName.FireMark, typeof(FireMark), _projectileSpell);
            
            #endregion
            
            #region SpellinitFrost
            
            InstantiateSpells(SpellName.FrostWhirlwind, typeof(FrostWhirlwind), _projectileSpell);
            
            #endregion
            
            #region SpellinitEarth
            
            InstantiateSpells(SpellName.Spike, typeof(Spike), _projectileSpell);
            
            #endregion
        }
        
        private void InstantiateSpells(SpellName spellName, Type spellType, GameObject prefab)
        {
            _playerSpellPools.Add(spellName, new Queue<Spell>());
            
            for (int i = 0; i < _numOfEachSpell; i++)
            {
                InstantiateSpell(spellName, spellType, prefab);
            }
        }

        private void InstantiateSpell(SpellName spellName, Type spellType, GameObject prefab)
        {
            GameObject projectile = Instantiate(prefab, transform);
            var spellComponent = (Spell)projectile.gameObject.AddComponent(spellType);
            _playerSpellPools[spellName].Enqueue(spellComponent);
            projectile.gameObject.SetActive(false);
        }
        
        private T GetSpell<T>(SpellName spellName, GameObject prefab) where T : class
        {
            if (_playerSpellPools == null) return null;

            T spell;

            if (_playerSpellPools.TryGetValue(spellName, out var spellQueue) && spellQueue.Count > 0)
            {
                if (spellQueue.Count > 1)
                {
                    spell = spellQueue.Dequeue() as T;
                }
                else
                {
                    var dequeuedSpell = spellQueue.Dequeue();
                    InstantiateSpell(spellName, dequeuedSpell.GetType(), prefab);
                    spell = dequeuedSpell as T;
                }
            }
            else
            {
                spell = null;
                Debug.LogError("NO SPELLS IN POOL WTF");
            }

            return spell;
        }

        public Spell CreateSpell(SpellName spellName)
        {
            Spell spell;
            SpellType type = SpellData.Instance.GetSpellConfig(spellName).GetSPellType();
            //Debug.Log(type);
            switch (type)
            {
                case SpellType.Projectile:
                    spell = GetSpell<ProjectileSpell>(spellName, _projectileSpell);
                    break;
                case SpellType.AoeInstantSpell:
                    spell = GetSpell<AoeInstantSpell>(spellName, _aoeInstantSpell);
                    break;
                case SpellType.PlacedSpell:
                    spell = GetSpell<DeployableSpell>(spellName, _deployableSpell);
                    break;
                case SpellType.SummonSpell:
                    spell = GetSpell<SummoningSpell>(spellName, _summonSpell);
                    break;
                case SpellType.LineSpell:
                    spell = GetSpell<LineSpell>(spellName, _lineSpell);
                    break;
                default:
                    return null;
            }
            
            spell.transform.SetParent(null);
            return spell;
        }

        public void ReturnSpell(SpellName spellName, Spell spell)
        {
            spell.gameObject.SetActive(false);
            spell.transform.SetParent(transform);

            if (_playerSpellPools != null)
            {
                _playerSpellPools[spellName].Enqueue(spell);
            }
            else
            {
                Destroy(spell.gameObject);
            }
        }
    }
}