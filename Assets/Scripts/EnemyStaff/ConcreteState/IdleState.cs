using UnityEngine;
using Unity.Mathematics;
using Data;

namespace EnemyStaff.ConcreteState
{
    public class IdleState : EnemyState
    {
        private readonly int _playerMask = LayerMask.GetMask("Player");
        private readonly int _wallMask = LayerMask.GetMask("Walls");
        
        private readonly float _sightRange = 20f;
        private readonly float _yOffset = 0.5f;
        
        private readonly Transform _playerTransform;

        private void StartEngage()
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        private bool CheckLineOfSight()
        {
            for (int i = 1; i <= 2; i++)
            {
                float offset = _yOffset * math.pow(-1, i);
                
                Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y + offset);
                Vector2 direction = new Vector2(_playerTransform.position.x, _playerTransform.position.y - _yOffset) - startPosition;
            
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, _playerMask | _wallMask);

                var isNeedToReturn = hit.collider && ((1 << hit.collider.gameObject.layer) & _playerMask) != 0;

                if (isNeedToReturn) return true;
            }

            return false;
        }
        
        public IdleState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _playerTransform = G.PlayersHp.transform;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (Me.EngageCircle.NearbyPlayers.Count > 0)
            {
                if (CheckLineOfSight()) StartEngage();
            }
        }
    }
}
