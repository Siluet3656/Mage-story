using UnityEngine;
using Unity.Mathematics;
using PlayerStaff;

namespace EnemyStaff.StateSO.Attack
{
    [CreateAssetMenu(fileName = "MeleeAttack", menuName = "Enemy logic/Attack/MeleeAttack", order = 30)]
    public class MeleeAttack : EnemyAttackSoBase
    {
        [Header("Retreat settings")]
        [SerializeField, Min(0f)]private  float _thresholdHpPercent = 0.1f;

        [Header("Attack settings")]
        [SerializeField, Range(1f,2f)] private float _attackRadiusExtendFactor = 1.2f;
        
        [Header("Sight settings")]
        [SerializeField, Min(1f)]private float _sightRange = 20f;
        [SerializeField, Min(0.1f)]private float _yOffset = 0.5f;
        
        private void StopAttack(PlayerMovement player)
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }
        
        private void StartWandering()
        {
            Me.StateMachine.ChangeState(Me.WanderingState);
        }

        private void SetDefaultAttackRadius()
        {
            AttackCircle.SetCircleRadius(DefaultRadius);
        }

        private void ExtendAttackRadius()
        {
            AttackCircle.SetCircleRadius(DefaultRadius * _attackRadiusExtendFactor);
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

        public override void DoEnterLogic()
        {
            Me.AttackCircle.OnPlayerExitCircle += StopAttack;

            ExtendAttackRadius();
        }

        public override void DoExitLogic()
        {
            Me.AttackCircle.OnPlayerExitCircle -= StopAttack;

            SetDefaultAttackRadius();
        }

        public override void DoFrameUpdateLogic()
        {
            if (MyHp.CurrentHealth < MyHp.MaxHealth * _thresholdHpPercent) StartRetreat();

            if (CheckLineOfSight() == false) StartWandering();
            
            if (MyAttack.IsReadyToAttack) MyAttack.PerformAttack();
        }
    }
}