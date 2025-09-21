using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EntityStaff;
using Shard;

namespace Spells
{
    [RequireComponent(typeof(LineRenderer))]
    public abstract class LineSpell : Spell
    {
        private List<LineRenderer>  _renderers;
        private float _duration;
        private ITargetable _target;
        private PlayersShard _shard;
        
        protected override SpellName SpellName { get; set; }
        protected ITargetable Target => _target;

        protected override void Awake()
        {
            base.Awake();

            _renderers = new List<LineRenderer>();

            LineRenderer[] lineRenderersInChildren = GetComponentsInChildren<LineRenderer>();

            foreach (var lineRenderer in lineRenderersInChildren)
            {
                _renderers.Add(lineRenderer);
            }
        }
        
        private void Update()
        {
            UpdateLinePosition(_shard.transform.position, _target.GameObject.transform.position);
        }

        private void Initialize(LaserSpellConfig config)
        {
            SpellDamage = config.Damage;
            _duration = config.Duration;

            /*foreach (var lineRenderer in _renderers)
            {
                lineRenderer.colorGradient = config.Color;
            }*/
        }

        private IEnumerator Existing()
        {
            yield return new WaitForSeconds(_duration);
            
            base.ReturnToPool();
        }

        private void UpdateLinePosition(Vector3 startPosition, Vector3 endPositions)
        {
            foreach (var lineRenderer in _renderers)
            {
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, endPositions);
            }
        }

        public override void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            if (config is LaserSpellConfig deployableSpellConfig)
            {
                Initialize(deployableSpellConfig);
            }
            
            _shard = G.PlayersShard;
            
            base.Initialize(config, adjustedCriticalMultiply, adjustedCriticalChance);
        }
        
        public override void DoSpell()
        {
            
            UpdateLinePosition(_shard.transform.position, _target.GameObject.transform.position);
            
            gameObject.SetActive(true);

            StartCoroutine(Existing());
        }
        
        public bool TrySetTarget(ITargetable target)
        {
            if (target == null) return false;
            
            _target = target;

            return true;
        }
    }
}
