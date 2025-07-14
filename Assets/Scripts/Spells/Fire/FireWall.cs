using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityResources;

namespace Spells.Fire
{
    public class FireWall : DeployableSpell
    {
        private readonly Dictionary<Hp, IEnumerator> _damageRoutines = new Dictionary<Hp, IEnumerator>();
        private readonly float _size = 5f;
        private Vector3 _defaultScale;

        protected override void Awake()
        {
            _defaultScale = transform.localScale;
            
            BoxCollider2D collider2d = gameObject.AddComponent<BoxCollider2D>();
            collider2d.isTrigger = true;
            collider2d.size = new Vector2(1.6f, 1.2f);
            
            base.Awake();
        }

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

        public override void DoSpell()
        {
            transform.localScale *= _size;
            _damageRoutines.Clear();
            base.DoSpell();
        }
    }
}
