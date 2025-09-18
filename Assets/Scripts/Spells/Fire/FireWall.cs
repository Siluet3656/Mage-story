using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.SpellConfigs;
using EntityStaff;

namespace Spells.Fire
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FireWall : DeployableSpell
    {
        private readonly Dictionary<Hp, IEnumerator> _damageRoutines = new Dictionary<Hp, IEnumerator>();
        private Vector3 _defaultScale;
        private DeployableSpellConfig _config;

        private IEnumerator DealDamageEachSecond(Hp hp)
        {
            while (true)
            {
                hp.TryToTakeCriticalDamage(SpellDamage,CriticalMultiply,CriticalChance);
                yield return new WaitForSeconds(1f);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Hp hp = other.gameObject.GetComponent<Hp>();
                
            if (hp != null)
            {
                if (_damageRoutines.ContainsKey(hp) == false)
                {
                    IEnumerator routine = DealDamageEachSecond(hp);
                    _damageRoutines.Add(hp, routine);
                    StartCoroutine(routine);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Hp hp = other.gameObject.GetComponent<Hp>();
            if (hp != null)
            {
                if (_damageRoutines.ContainsKey(hp))
                {
                    StopCoroutine(_damageRoutines[hp]);
                    _damageRoutines.Remove(hp);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var routine in _damageRoutines)
            {
                if (routine.Value != null)
                {
                    StopCoroutine(routine.Value);
                }
            }
            _damageRoutines.Clear();
            transform.localScale = _defaultScale;
        }
        
        protected override void Awake()
        {
            _defaultScale = transform.localScale;
            
            base.Awake();
        }

        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);

            if (config is DeployableSpellConfig deployableSpellConfig)
            {
                _config = deployableSpellConfig;
            }
        }

        public override void DoSpell()
        {
            transform.localScale *= _config.ScaleFactor;
            _damageRoutines.Clear();
            base.DoSpell();
        }
    }
}
