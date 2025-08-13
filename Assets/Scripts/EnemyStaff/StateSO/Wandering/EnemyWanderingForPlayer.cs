using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Data;
using Pathfinding;

namespace EnemyStaff.StateSO.Wandering
{
    [CreateAssetMenu(fileName = "EnemyWanderingForPlayer", menuName = "Enemy logic/Wandering/EnemyWanderingForPlayer", order = 10)]
    public class EnemyWanderingForPlayer : EnemyWanderingSoBase
    {
        [Header("Retreat settings")]
        [SerializeField, Min(0f)] private  float _thresholdHpPercent = 0.15f;
        
        [Header("Sight settings")]
        [SerializeField, Min(1f)] private  float _sightRange = 20f;
        [SerializeField, Min(0.1f)] private  float _yOffset = 0.5f;
        
        private List<Node> _path;
        private Vector2 _moveDirection;
        private Vector2 _playerPosition;
        private Node _nodePlayerOn;
        private Vector2 _myPosition;
        private Node _nodeMeOn;
        private float _distanceToNode;
        private bool _isNeedToGeneratePath;
        
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
        
        private bool CheckLineOfSight()
        {
            for (int i = 1; i <= 2; i++)
            {
                float offset = _yOffset * math.pow(-1, i);
                
                Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y + offset);
                Vector2 direction = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y - _yOffset) - startPosition;
            
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, PlayerMask | WallMask);

                var isNeedToReturn = hit.collider && ((1 << hit.collider.gameObject.layer) & PlayerMask) != 0;

                if (isNeedToReturn) return true;
            }

            return false;
        }
        
        private void MoveAlongPath()
        {
            _moveDirection = (_path.First().transform.position - Me.transform.position).normalized;
            _distanceToNode = Vector2.Distance(_path.First().transform.position, Me.transform.position);

            if (_distanceToNode > AStar.Instance.MinNodeDistance)

            {
                MyMovement.Move(_moveDirection);
            }
            else
            {
                _path.Remove(_path.First());
            }
        }
        
        private void FindPathToPlayer()
        {
            _playerPosition = G.PlayersHp.transform.position;
            _nodePlayerOn = AStar.Instance.FindNearestNode(_playerPosition);
            _myPosition = Me.transform.position;
            _nodeMeOn = AStar.Instance.FindNearestNode(_myPosition);
            _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodePlayerOn);
            
            if (_path != null && _path.Count > 0) _isNeedToGeneratePath = false;
        }

        public override void DoEnterLogic()
        {
            _isNeedToGeneratePath = true;
        }

        public override void DoFrameUpdateLogic()
        {
            if (MyHp.CurrentHealth < MyHp.MaxHealth * _thresholdHpPercent) StartRetreat();

            if (CheckLineOfSight())
            {
                if (Me.AttackCircle.NearbyPlayers.Count > 0) StartAttack();
                
                StartEngage();
            }

            if (_isNeedToGeneratePath) FindPathToPlayer();
            
            if (_isNeedToGeneratePath == false) MoveAlongPath();
            
            if (_path.Count == 0) GiveUp();
        }
    }
}