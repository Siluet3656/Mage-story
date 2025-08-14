using Unity.Mathematics;
using UnityEngine;

namespace EnemyStaff.StateSO.Idle
{
    [CreateAssetMenu(fileName = "EnemyStandingStill", menuName = "Enemy logic/Idle/EnemyStandingStill", order = 30)]
    public class EnemyStandingStill : EnemyIdleSoBase
    {
        [SerializeField, Min(1f)] private float _sightRange = 20f;
        [SerializeField, Min(0.1f)] private float _yOffset = 0.5f;
        
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
                Vector2 direction = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y - _yOffset) - startPosition;
            
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, PlayerMask | WallMask);

                var isNeedToReturn = hit.collider && ((1 << hit.collider.gameObject.layer) & PlayerMask) != 0;

                if (isNeedToReturn) return true;
            }

            return false;
        }

        public override void DoFrameUpdateLogic()
        {
            if (Me.EngageCircle.NearbyPlayers.Count > 0)
            {
                if (CheckLineOfSight()) StartEngage();
            }
        }
    }
}
