using Unity.Mathematics;
using UnityEngine;

namespace EnemyStaff.StateSO.Engage
{
    [CreateAssetMenu(fileName = "EnemyChase", menuName = "Enemy logic/Engage/EnemyChase", order = 10)]
    public class EnemyChase : EnemyEngageSoBase
    {
        [Header("Retreat settings")]
        [SerializeField, Min(0f)] private  float _thresholdHpPercent = 0.15f;
        
        [Header("Sight settings")]
        [SerializeField, Min(1f)] private  float _sightRange = 20f;
        [SerializeField, Min(0.1f)] private  float _yOffset = 0.5f;

        private Vector2 _moveDirection;
        
        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }

        private void StartWandering()
        {
            Me.StateMachine.ChangeState(Me.WanderingState);
        }
        
        private void StartAttack()
        {
            Me.StateMachine.ChangeState(Me.AttackState);
        }

        private void MoveDirectlyToPlayer()
        {
            _moveDirection = (PlayerTransform.position - Me.transform.position).normalized;
            
            MyMovement.Move(_moveDirection);
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

        public override void DoFrameUpdateLogic()
        {
            if (MyHp.CurrentHealth < MyHp.MaxHealth * _thresholdHpPercent) StartRetreat();
            
            if (CheckLineOfSight())
            {
                if (Me.AttackCircle.NearbyPlayers.Count > 0) StartAttack();
                
                MoveDirectlyToPlayer();
            }
            else
            {
                StartWandering();
            }
        }
    }
}
