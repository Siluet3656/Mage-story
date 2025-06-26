using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Shard;

namespace Spells
{
    public class SpellFactory : MonoBehaviour
    {
        [Header("Player's poll settings")]
        [SerializeField, Min(1)] private int _playerSpellPoolSize;
        [Header("Prefabs")]
        [SerializeField] private ProjectileSpell _projectileSpell;

        private readonly Queue<Spell> _playerSpellPool = new Queue<Spell>();
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
                _playerSpellPool.Enqueue(spell);
            }
        }

        public Spell CreateSpell(SpellName spellName)
        {
            if (_playerSpellPool == null)
            {
                Debug.LogError($"No pool found for spell type: {spellName}");
                return null;
            }

            Spell spell;
            if (_playerSpellPool.Count > 0)
            {
                spell = _playerSpellPool.Dequeue();
            }
            else
            {
                spell = Instantiate(_projectileSpell, transform);
                spell.gameObject.SetActive(false);
                Debug.LogWarning($"Expanding pool for {spellName}");
            }

            spell.transform.SetParent(null);
            return spell;
        }

        public void ReturnSpell(Spell spell)
        {
            spell.gameObject.SetActive(false);
            spell.transform.SetParent(transform);

            if (_playerSpellPool != null)
            {
                _playerSpellPool.Enqueue(spell);
            }
            else
            {
                Destroy(spell.gameObject);
            }
        }
    }
}