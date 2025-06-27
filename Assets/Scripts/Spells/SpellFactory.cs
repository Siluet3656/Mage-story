using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Data;
using Data.SpellConfigs;
using Shard;

namespace Spells
{
    public class SpellFactory : MonoBehaviour
    {
        [Header("Player's poll settings")]
        [SerializeField, Min(1)] private int _playerSpellPoolSize;
        [Header("Prefabs")]
        [SerializeField] private ProjectileSpell _projectileSpell;

        private readonly Queue<Spell> _playerProjectileSpellPool = new Queue<Spell>();
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

            InitializePlayerPool();
        }

        private void InitializePlayerPool()
        {
            for (int i = 0; i < _playerSpellPoolSize; i++)
            {
                var spell = Instantiate(_projectileSpell, transform);
                spell.gameObject.SetActive(false);
                _playerProjectileSpellPool.Enqueue(spell);
            }
        }

        private ProjectileSpell GetProjectileSpell()
        {
            ProjectileSpell projectileSpell;
            
            if (_playerProjectileSpellPool == null)
            {
                return null;
            }
            
            if (_playerProjectileSpellPool.Count > 0)
            {
                projectileSpell = _playerProjectileSpellPool.Dequeue() as ProjectileSpell;
            }
            else
            {
                projectileSpell = Instantiate(_projectileSpell, transform);
                projectileSpell.gameObject.SetActive(false);
            }

            return projectileSpell;
        }

        public Spell CreateSpell(SpellConfig config)
        {
            Spell spell;
            SpellType type = config.Type;

            switch (type)
            {
                case SpellType.Projectile:
                    spell = GetProjectileSpell();
                    break;
                default:
                    return null;
            }
            
            spell.transform.SetParent(null);
            return spell;
        }

        public void ReturnSpell(Spell spell)
        {
            spell.gameObject.SetActive(false);
            spell.transform.SetParent(transform);

            if (_playerProjectileSpellPool != null)
            {
                _playerProjectileSpellPool.Enqueue(spell);
            }
            else
            {
                Destroy(spell.gameObject);
            }
        }
    }
}