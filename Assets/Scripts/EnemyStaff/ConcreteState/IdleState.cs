using UnityEngine;
using Data;
using Unity.Mathematics;

namespace EnemyStaff.ConcreteState
{
    public class IdleState : EnemyState
    {
        private readonly EnemyStateMachine _enemyStateMachine;
        
        private readonly int _playerMask = LayerMask.GetMask("Player");
        private readonly int _wallMask = LayerMask.GetMask("Walls");
        
        private readonly float _sightRange = 20f;
        private readonly float _yOffset = 0.5f;
        
        private readonly Transform _playerTransform;

        private void StartEngage()
        {
            Me.StateMachine.ChangeState(Me.EngageState);
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
        
        public IdleState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _enemyStateMachine = enemyStateMachine;
            _playerTransform = G.PlayersHp.transform;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (Me.EngageCircle.NearbyPlayers.Count > 0)
            {
                if (CheckLineOfSite()) StartEngage();
            }
        }
    }
}
