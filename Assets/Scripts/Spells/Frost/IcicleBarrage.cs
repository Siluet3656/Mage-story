using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;
using UnityEngine;

namespace Spells.Frost
{
    public class IcicleBarrage : SummoningSpell
    {
        private ITargetable _target;
        private readonly int _amountOfIcicles = 20;
        private float _delay;

        private readonly List<ProjectileSpell> _subscribedSpells = new List<ProjectileSpell>();

        private IEnumerator SpawnIcicles()
        {
            for (int i = 0; i < _amountOfIcicles; i++)
            {
                Spell icicle = SpellFactory.Instance.PoolSpell(SpellName.Icicle);
                SpellConfig spellConfig = SpellData.Instance.GetSpellConfig(SpellName.Icicle);

                if (icicle is ProjectileSpell projectile)
                {
                    if (projectile.TrySetTarget(_target))
                    {
                        float randomPositionX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
                        float randomPositionY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
                        
                        projectile.Initialize(spellConfig, CriticalMultiply, CriticalChance);
                        projectile.transform.position = new Vector3(randomPositionX, randomPositionY, transform.position.z);
                        
                        _target.GameObject.GetComponent<Hp>().OnDeath += projectile.OnTargetDeath;
                        
                        _subscribedSpells.Add(projectile);
                        
                        projectile.DoSpell();
                    }
                }

                yield return new WaitForSeconds(_delay);
            }
        }

        protected override void OnExistingEnd(Enemy enemy)
        {
            /*foreach (var spell in _subscribedSpells)
            {
                if (_target != null) _target.GameObject.GetComponent<Hp>().OnDeath -= spell.OnTargetDeath;
            }*/ //Пока не нужно. Мб надо будет делать фабрику врагов и тогда
            _subscribedSpells.Clear();
            
            base.OnExistingEnd(enemy);
        }

        public override void DoSpell()
        {
            Vector3 direction = _target.GameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _target.GameObject.GetComponent<Hp>().OnDeath += () => _target = null; //это мб тоже потом удалить
            
            base.DoSpell();

            _delay = ExistTime / (_amountOfIcicles + 1);
            StartCoroutine(SpawnIcicles());
        }

        public bool TrySetTarget(ITargetable target)
        {
            if (target == null) return false;
            
            _target = target;

            return true;
        }
    }
}
