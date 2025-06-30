using System;
using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Data;
using Shard;
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
            
            _playerSpellPools.Add(SpellName.Fireball, new Queue<Spell>());
            InstantiateSpells(SpellName.Fireball, typeof(Fireball), _projectileSpell);
            
            _playerSpellPools.Add(SpellName.FrostWhirlwind, new Queue<Spell>());
            InstantiateSpells(SpellName.FrostWhirlwind, typeof(FrostWhirlwind), _projectileSpell);
        }

        private void InstantiateSpells(SpellName spellName, Type spellType, GameObject prefab)
        {
            for (int i = 0; i < _numOfEachSpell; i++)
            {
                GameObject projectile = Instantiate(prefab, transform);
                var spellComponent = (Spell)projectile.gameObject.AddComponent(spellType);
                _playerSpellPools[spellName].Enqueue(spellComponent);
                projectile.gameObject.SetActive(false);
            }
        }

        private ProjectileSpell GetProjectileSpell(SpellName spellName)
        {
            if (_playerSpellPools == null) return null;

            ProjectileSpell projectileSpell;
            
            if (_playerSpellPools[spellName].Count > 1)
            {
                projectileSpell = _playerSpellPools[spellName].Dequeue() as ProjectileSpell;
            }
            else if (_playerSpellPools[spellName].Count == 1)
            {
                var spell = _playerSpellPools[spellName].Dequeue();
                InstantiateSpells(spellName, spell.GetType(), _projectileSpell);
                projectileSpell = spell as ProjectileSpell;
            }
            else
            {
                projectileSpell = null;
                Debug.LogError("NO SPELLS IN POOL WTF");
            }

            return projectileSpell;
        }

        public Spell CreateSpell(SpellName spellName)
        {
            Spell spell;
            SpellType type = SpellData.Instance.GetSpellConfig(spellName).Type;

            switch (type)
            {
                case SpellType.Projectile:
                    spell = GetProjectileSpell(spellName);
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