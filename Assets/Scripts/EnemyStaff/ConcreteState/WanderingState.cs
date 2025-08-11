using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Data;
using EntityStaff;
using Pathfinding;

namespace EnemyStaff.ConcreteState
{
    public class WanderingState : EnemyState
    {
        private readonly float _footprintFollowThreshold = 0.1f;
        private readonly float _thresholdHpPercent = 0.15f;
        
        private readonly float _sightRange = 20f;
        private readonly float _yOffset = 0.5f;
        
        private readonly int _playerMask = LayerMask.GetMask("Player");
        private readonly int _wallMask = LayerMask.GetMask("Walls");
        private readonly int _footprintMask = LayerMask.GetMask("FootPrint");
        
        private readonly EnemyMovement _myMovement;
        private readonly Transform _playerTransform;
        private readonly Hp _myHp;
       
        private Vector2 _moveDirection;
        private GameObject _footprint;
        
        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }
        
        private void StartAttack()
        {
            Me.StateMachine.ChangeState(Me.AttackState);
        }

        private void StartEngage()
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        private void GiveUp()
        {
            Me.StateMachine.ChangeState(Me.IdleState);
        }
        
        private void MoveDirectlyToFootprint(GameObject footprint)
        {
            _moveDirection = (footprint.transform.position - Me.transform.position).normalized;

            if (_moveDirection.magnitude > _footprintFollowThreshold)
            {
                _myMovement.Move(_moveDirection);
            }
        }
        
        private bool CheckLineOfSite()
        {
            bool toReturn = false;
            
            for (int i = 1; i <= 2; i++)
            {
                float offset = _yOffset * math.pow(-1, i);
                
                Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y - _yOffset);
                Vector2 direction = new Vector2(_playerTransform.position.x, _playerTransform.position.y - _yOffset) - startPosition;
            
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, _playerMask | _wallMask);

                toReturn = hit.collider && ((1 << hit.collider.gameObject.layer) & _playerMask) != 0;
            }

            return toReturn;
        }
        
        private bool TryToGetFootprint(out GameObject footprint)
        {
            List<GameObject> nearbyFootprints = new List<GameObject>();

            for (int i = 1; i <= 2; i++)
            {
                float offset = _yOffset * math.pow(-1,i);
                
                foreach (var activeFootprint in G.ActiveFootprints)
                {
                    Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y + offset);
                    Vector2 direction = new Vector2(activeFootprint.transform.position.x, _playerTransform.position.y) - startPosition;
            
                    RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, _footprintMask | _wallMask);

                    if (hit.collider && ((1 << hit.collider.gameObject.layer) & _footprintMask) != 0) nearbyFootprints.Add(activeFootprint);
                }
            }

            if (nearbyFootprints.Count > 0)
            {
                footprint = nearbyFootprints
                    .OrderBy(nearFootprint => nearFootprint.GetComponent<Footprint>().CurrentLifeTime)
                    .FirstOrDefault();
                
                return true;
            }

            footprint = null;
            return false;
        }
        
        public WanderingState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _myMovement = me.GetComponent<EnemyMovement>();
            _myHp = me.GetComponent<Hp>();
            _playerTransform = G.PlayersHp.transform;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent) StartRetreat();
            
            if (Me.AttackCircle.NearbyPlayers.Count > 0) StartAttack();

            if (CheckLineOfSite()) StartEngage();
            
            if (TryToGetFootprint(out _footprint))
            {
                MoveDirectlyToFootprint(_footprint);
            }
            else
            {
                GiveUp();
            }
        }
    }
}
